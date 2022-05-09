using System;
using PlayerMarkers.Asset;
using UnboundLib;
using UnityEngine;

namespace PlayerMarkers.Util
{
    public static class MarkerController
    {
        public static void SpriteColor(GameObject gameObject, Color color, float intensity)
        {
            var renderer = gameObject.GetComponentInChildren<SpriteRenderer>();

            renderer.color = color * intensity;
        }

        public static GameObject MarkerTypeObject(MarkerType markerType)
        {
            switch (markerType)
            {
                case MarkerType.Heart:
                    return AssetManager.Heart;
                case MarkerType.Triangle:
                    return AssetManager.Triangle;
                case MarkerType.Arrow:
                    return AssetManager.Arrow;
                case MarkerType.Circle:
                    return AssetManager.Circle;
                case MarkerType.Egg:
                    return AssetManager.Egg;
                case MarkerType.Plus:
                    return AssetManager.Plus;
                case MarkerType.Square:
                    return AssetManager.Square;
                case MarkerType.Star:
                    return AssetManager.Star;
                default:
                    return AssetManager.Triangle;
            }
        }

        public enum MarkerType
        {
            Heart,
            Triangle,
            Arrow,
            Circle,
            Egg,
            Plus,
            Square,
            Star
        }
    }
}
