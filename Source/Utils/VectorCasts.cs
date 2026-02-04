using System.Runtime.CompilerServices;
using UnityEngine;
using UnityVec2 = UnityEngine.Vector2;
using UnityVec3 = UnityEngine.Vector3;
using UnityVec4 = UnityEngine.Vector4;
using SystemVec2 = System.Numerics.Vector2;
using SystemVec3 = System.Numerics.Vector3;
using SystemVec4 = System.Numerics.Vector4;

namespace UImGui
{
    public static class VectorCasts
    {
        public static SystemVec2 ToSystem(this UnityVec2 input)
        {
            return Unsafe.As<UnityVec2, SystemVec2>(ref input);
        }
        
        public static ref SystemVec2 ToSystemRef(this ref UnityVec2 input)
        {
            return ref Unsafe.As<UnityVec2, SystemVec2>(ref input);
        }
        public static ref SystemVec3 ToSystemRef(this ref UnityVec3 input)
        {
            return ref Unsafe.As<UnityVec3, SystemVec3>(ref input);
        }

        public static UnityVec2 ToUnity(this SystemVec2 input)
        {
            return Unsafe.As<SystemVec2, UnityVec2>(ref input);
        }

        public static SystemVec4 ToSysVec4(this UnityEngine.Color input)
        {
            return new SystemVec4(input.r, input.g, input.b, input.a);
        }

        public static UnityEngine.Color ToUnityColor(this SystemVec4 input)
        {
            return new Color(input.X, input.Y, input.Z, input.W);
        }

        public static UnityVec4 ToUnity(this SystemVec4 input)
        {
            return new UnityVec4(input.X, input.Y, input.Z, input.W);
        }
    }
}