using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace PlayerMarkers.Asset
{
    internal class AssetManager
    {
        private static readonly AssetBundle MarkersAssetsBundle = Jotunn.Utils.AssetUtils.LoadAssetBundleFromResources("playermarkers_assets", typeof(PlayerMarkers).Assembly);

        public static GameObject Heart = MarkersAssetsBundle.LoadAsset<GameObject>("Heart");
        public static GameObject Triangle = MarkersAssetsBundle.LoadAsset<GameObject>("Triangle");
        public static GameObject Arrow = MarkersAssetsBundle.LoadAsset<GameObject>("Arrow");
        public static GameObject Circle = MarkersAssetsBundle.LoadAsset<GameObject>("Circle");
        public static GameObject Egg = MarkersAssetsBundle.LoadAsset<GameObject>("Egg");
        public static GameObject Plus = MarkersAssetsBundle.LoadAsset<GameObject>("Plus");
        public static GameObject Square = MarkersAssetsBundle.LoadAsset<GameObject>("Square");
        public static GameObject Star = MarkersAssetsBundle.LoadAsset<GameObject>("Star");
    }
}
