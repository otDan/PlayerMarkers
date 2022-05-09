using BepInEx.Configuration;

namespace PlayerMarkers.Util
{
    public static class ConfigController
    {
        public static ConfigEntry<bool> OwnEnabledConfig;
        public static ConfigEntry<float> OwnHeightConfig;
        public static ConfigEntry<float> OwnWidthConfig;
        public static ConfigEntry<float> OwnBloomConfig;
        public static ConfigEntry<int> OwnMarkerTypeConfig;

        public static ConfigEntry<bool> TeamEnabledConfig;
        public static ConfigEntry<float> TeamHeightConfig;
        public static ConfigEntry<float> TeamWidthConfig;
        public static ConfigEntry<float> TeamBloomConfig;
        public static ConfigEntry<int> TeamMarkerTypeConfig;

        public static ConfigEntry<bool> EnemyEnabledConfig;
        public static ConfigEntry<float> EnemyHeightConfig;
        public static ConfigEntry<float> EnemyWidthConfig;
        public static ConfigEntry<float> EnemyBloomConfig;
        public static ConfigEntry<int> EnemyMarkerTypeConfig;
    }
}
