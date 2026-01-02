#ifndef DEARIMGUI_UNIVERSAL_INCLUDED
#define DEARIMGUI_UNIVERSAL_INCLUDED

#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#ifndef UNITY_COLORSPACE_GAMMA
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
#endif

#include "./Common.hlsl"

TEXTURE2D(_Texture);
SAMPLER(sampler_Texture);
float2 _ClipRectMin;
float2 _ClipRectMax;

half4 unpack_color(uint c)
{
    half4 color = half4(
        (c      ) & 0xff,
        (c >>  8) & 0xff,
        (c >> 16) & 0xff,
        (c >> 24) & 0xff
    ) / 255;
#ifndef UNITY_COLORSPACE_GAMMA
    color.rgb = FastSRGBToLinear(color.rgb);
#endif
    return color;
}

float PointInRect01(float2 p, float2 rectMin, float2 rectMax)
{
    float2 insideMin = step(rectMin, p);
    float2 insideMax = step(p, rectMax);
    return insideMin.x * insideMin.y * insideMax.x * insideMax.y;
}

Varyings ImGuiPassVertex(ImVert input)
{
    Varyings output  = (Varyings)0;

    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_TRANSFER_INSTANCE_ID(input, output);
    UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(output);

    output.screenVertex = input.vertex;
    output.vertex    = TransformWorldToHClip(TransformObjectToWorld(float3(input.vertex, 0.0)));
    output.uv        = float2(input.uv.x, 1 - input.uv.y);
    output.color     = unpack_color(input.color);
    return output;
}

half4 ImGuiPassFrag(Varyings input) : SV_Target
{
    UNITY_SETUP_INSTANCE_ID(input);
    UNITY_SETUP_STEREO_EYE_INDEX_POST_VERTEX(input);

    float inside = PointInRect01(input.screenVertex, _ClipRectMin, _ClipRectMax);
    //clip(inside - 0.5);
    
    return input.color * SAMPLE_TEXTURE2D(_Texture, sampler_Texture, input.uv);
}

#endif
