using Unity.Entities;
using Unity.Mathematics;
using UnityEngine;

namespace Arkanoid.Scripts
{
    public static class DotsTools
    {
        public static Entity ToEntity(this GameObject go)
        {
            return GameObjectConversionUtility.ConvertGameObjectHierarchy(go,
                GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, null));
        }

        public static float3 ToFloat3(this Vector3 v)
        {
            return new float3(v.x, v.y, v.z);
        }

        public static float3 ToFloat3(this Vector2 v)
        {
            return new float3(v.x, v.y, 0);
        }

        public static float2 ToFloat2(this Vector3 v)
        {
            return new float2(v.x, v.y);
        }

        public static float2 ToFloat2(this Vector2 v)
        {
            return new float2(v.x, v.y);
        }
    }
}