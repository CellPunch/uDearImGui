using System;
using Hexa.NET.ImGui;
using UnityEngine;

public sealed class CustomFontBootstrap : MonoBehaviour
{
    [SerializeField] private CustomFontByteData fontData;
    [SerializeField, Range(16, 96)] private int fontSizePx = 48;
    [SerializeField] private int minTextureWidth = 4096;
    [SerializeField] private int maxTextureWidth = 4096;

    private ImFontConfigPtr imFontConfig;
    
    public unsafe void OnFontInit(ImGuiIOPtr io)
    {
        if (fontData == null || fontData.FontData.Length == 0)
        {
            io.Fonts.Clear();
            io.Fonts.AddFontDefault();
            return;
        }

        io.Fonts.Clear();
        
        io.Fonts.TexMinWidth = minTextureWidth;
        io.Fonts.TexMaxWidth = maxTextureWidth;
        io.Fonts.TexGlyphPadding *= 2; //Double padding to preserve mipmapping problems


        imFontConfig = ImGui.ImFontConfig();
        imFontConfig.RasterizerMultiply = 0.9f;
        imFontConfig.FontDataOwnedByAtlas = false;

        fixed (byte* pFont = fontData.FontData)
        {
            io.Fonts.AddFontFromMemoryTTF(
                pFont,
                fontData.FontData.Length,
                fontSizePx,
                imFontConfig,
                io.Fonts.GetGlyphRangesDefault()
            );
        }
    }

    private unsafe void OnDestroy()
    {
        imFontConfig.Destroy();
    }
    
}