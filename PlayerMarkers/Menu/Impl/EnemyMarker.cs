using PlayerMarkers.Util;
using TMPro;
using UnboundLib;
using UnboundLib.Utils.UI;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerMarkers.Menu.Impl
{
    public static class EnemyMarker
    {
        internal static void Menu(GameObject menu)
        {
            var enemyMarkerText = MenuHandler.CreateText("Enemy Marker", menu, out TextMeshProUGUI _, 60);
            Unbound.Instance.StartCoroutine(MenuController.FindScaler(enemyMarkerText.transform, MenuController.MarkerOwner.Enemy));

            void EnabledChanged(bool val)
            {
                ConfigController.EnemyEnabledConfig.Value = val;
                MenuController.enemyMarkerEnabled = ConfigController.EnemyEnabledConfig.Value;
            }
            var markerToggle = MenuHandler.CreateToggle(ConfigController.EnemyEnabledConfig.Value, "Marker Enabled", menu, EnabledChanged, 30);

            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);
            MenuHandler.CreateText("Marker Size:", menu, out TextMeshProUGUI _, 25);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            void HeightChanged(float val)
            {
                ConfigController.EnemyHeightConfig.Value = Mathf.Clamp(val, 0.1f, 1f);
                MenuController.enemyMarkerSize.x = ConfigController.EnemyHeightConfig.Value;
            }
            MenuHandler.CreateSlider("Marker height", menu, 25, 0.1f, 1f, ConfigController.EnemyHeightConfig.Value, HeightChanged, out Slider heightSlider, false);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            void WidthChanged(float val)
            {
                ConfigController.EnemyWidthConfig.Value = Mathf.Clamp(val, 0.1f, 1f);
                MenuController.enemyMarkerSize.y = ConfigController.EnemyWidthConfig.Value;
            }
            MenuHandler.CreateSlider("Marker width", menu, 25, 0.1f, 1f, ConfigController.EnemyWidthConfig.Value, WidthChanged, out Slider widthSlider, false);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            void BloomChanged(float val)
            {
                ConfigController.EnemyBloomConfig.Value = Mathf.Clamp(val, 1f, 5f);
                MenuController.enemyMarkerBloom = ConfigController.EnemyBloomConfig.Value;
            }
            MenuHandler.CreateSlider("Marker bloom", menu, 25, 1f, 5f, ConfigController.EnemyBloomConfig.Value, BloomChanged, out Slider bloomSlider, false);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            void ResetOptions()
            {
                EnabledChanged(true);
                markerToggle.GetComponent<Toggle>().isOn = true;
                HeightChanged(0.75f);
                heightSlider.value = 0.75f;
                WidthChanged(0.75f);
                widthSlider.value = 0.75f;
                BloomChanged(3f);
                bloomSlider.value = 3f;
            }
            MenuHandler.CreateButton("Reset Options", menu, ResetOptions, 30);
        }
    }
}
