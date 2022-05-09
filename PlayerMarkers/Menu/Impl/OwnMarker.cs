using System;
using PlayerMarkers.Util;
using TMPro;
using UnboundLib;
using UnboundLib.Utils.UI;
using UnityEngine;
using UnityEngine.UI;

namespace PlayerMarkers.Menu.Impl
{

    public static class OwnMarker
    {
        public static GameObject typeSlider;

        internal static void Menu(GameObject menu)
        {
            var ownMarkerText = MenuHandler.CreateText("Own Marker", menu, out TextMeshProUGUI _);
            Unbound.Instance.StartCoroutine(MenuController.FindScaler(ownMarkerText.transform, MenuController.MarkerOwner.Own));

            void EnabledChanged(bool val)
            {
                ConfigController.OwnEnabledConfig.Value = val;
                MenuController.ownMarkerEnabled = ConfigController.OwnEnabledConfig.Value;
            }
            var markerToggle = MenuHandler.CreateToggle(ConfigController.OwnEnabledConfig.Value, "Marker Enabled", menu, EnabledChanged, 30);

            int types = Enum.GetValues(typeof(MarkerController.MarkerType)).Length;
            void TypeChanged(float val)
            {
                var oldValue = ConfigController.OwnMarkerTypeConfig.Value;
                ConfigController.OwnMarkerTypeConfig.Value = Mathf.RoundToInt(Mathf.Clamp(val, 1, types));
                MenuController.ownMarkerType = ConfigController.OwnMarkerTypeConfig.Value - 1;

                if (oldValue == ConfigController.OwnMarkerTypeConfig.Value) return;
                MenuController.CreateMarker(MenuController.MarkerOwner.Own);
            }
            typeSlider = MenuHandler.CreateSlider("Marker type", menu, 25, 1, types, ConfigController.OwnMarkerTypeConfig.Value, TypeChanged, out Slider slider, true);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);
            MenuHandler.CreateText("Marker Size:", menu, out TextMeshProUGUI _, 25);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            MenuHandler.CreateSlider("Marker height", menu, 25, 0.1f, 1f, ConfigController.OwnHeightConfig.Value, HeightChanged, out Slider heightSlider);
            void HeightChanged(float val)
            {
                ConfigController.OwnHeightConfig.Value = Mathf.Clamp(val, 0.1f, 1f);
                MenuController.ownMarkerSize.x = ConfigController.OwnHeightConfig.Value;
            }
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            MenuHandler.CreateSlider("Marker width", menu, 25, 0.1f, 1f, ConfigController.OwnWidthConfig.Value, WidthChanged, out Slider widthSlider);
            void WidthChanged(float val)
            {
                ConfigController.OwnWidthConfig.Value = Mathf.Clamp(val, 0.1f, 1f);
                MenuController.ownMarkerSize.y = ConfigController.OwnWidthConfig.Value;
            }
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            MenuHandler.CreateSlider("Marker bloom", menu, 25, 1f, 5f, ConfigController.OwnBloomConfig.Value, BloomChanged, out Slider bloomSlider);
            void BloomChanged(float val)
            {
                ConfigController.OwnBloomConfig.Value = Mathf.Clamp(val, 1f, 5f);
                MenuController.ownMarkerBloom = ConfigController.OwnBloomConfig.Value;
            }
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            MenuHandler.CreateButton("Reset Options", menu, ResetOptions, 30);
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
                TypeChanged(1);
                slider.value = 1;
            }
        }
    }
}
