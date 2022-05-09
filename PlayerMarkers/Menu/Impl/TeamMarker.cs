using PlayerMarkers.Util;
using TMPro;
using UnboundLib;
using UnboundLib.Utils.UI;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerMarkers.Menu.Impl
{
    public static class TeamMarker
    {
        internal static void Menu(GameObject menu)
        {
            var teamMarkerText = MenuHandler.CreateText("Team Marker", menu, out TextMeshProUGUI _, 60);
            Unbound.Instance.StartCoroutine(MenuController.FindScaler(teamMarkerText.transform, MenuController.MarkerOwner.Team));

            void EnabledChanged(bool val)
            {
                ConfigController.TeamEnabledConfig.Value = val;
                MenuController.teamMarkerEnabled = ConfigController.TeamEnabledConfig.Value;
            }
            var markerToggle = MenuHandler.CreateToggle(ConfigController.TeamEnabledConfig.Value, "Marker Enabled", menu, EnabledChanged, 30);

            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);
            MenuHandler.CreateText("Marker Size:", menu, out TextMeshProUGUI _, 25);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            void HeightChanged(float val)
            {
                ConfigController.TeamHeightConfig.Value = Mathf.Clamp(val, 0.1f, 1f);
                MenuController.teamMarkerSize.x = ConfigController.TeamHeightConfig.Value;
            }
            MenuHandler.CreateSlider("Marker height", menu, 25, 0.1f, 1f, ConfigController.TeamHeightConfig.Value, HeightChanged, out Slider heightSlider, false);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            void WidthChanged(float val)
            {
                ConfigController.TeamWidthConfig.Value = Mathf.Clamp(val, 0.1f, 1f);
                MenuController.teamMarkerSize.y = ConfigController.TeamWidthConfig.Value;
            }
            MenuHandler.CreateSlider("Marker width", menu, 25, 0.1f, 1f, ConfigController.TeamWidthConfig.Value, WidthChanged, out Slider widthSlider, false);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            void BloomChanged(float val)
            {
                ConfigController.TeamBloomConfig.Value = Mathf.Clamp(val, 1f, 5f);
                MenuController.teamMarkerBloom = ConfigController.TeamBloomConfig.Value;
            }
            MenuHandler.CreateSlider("Marker bloom", menu, 25, 1f, 5f, ConfigController.TeamBloomConfig.Value, BloomChanged, out Slider bloomSlider, false);
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
