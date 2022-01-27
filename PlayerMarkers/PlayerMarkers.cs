using BepInEx;
using HarmonyLib;
using GameEnhancementCards.Utils;
using UnboundLib.GameModes;
using UnboundLib;
using UnboundLib.Utils.UI;
using UnityEngine;
using TMPro;
using BepInEx.Configuration;
using Photon.Pun;
using System.Collections;
using UnityEngine.UI;

namespace GameEnhancementCards
{
    // These are the mods required for our mod to work
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    // Declares our mod to Bepin
    [BepInPlugin(ModId, ModName, Version)]
    // The game our mod is associated with
    [BepInProcess("Rounds.exe")]
    public class PlayerMarkers : BaseUnityPlugin
    {
        private const string ModId = "ot.dan.rounds.PlayerMarkers";
        private const string ModName = "Player Markers";
        public const string Version = "1.1.0";
        public const string ModInitials = "PM";
        private const string CompatibilityModName = "PlayerMarkers";
        public static PlayerMarkers instance { get; private set; }

        public static bool inGame;

        private static GameObject ownMarker;
        public static ConfigEntry<bool> OwnEnabledConfig;
        public static bool ownMarkerEnabled;
        public static ConfigEntry<float> OwnHeightConfig;
        public static ConfigEntry<float> OwnWidthConfig;
        public static Vector3 ownMarkerSize;

        private static GameObject teamMarker;
        public static ConfigEntry<bool> TeamEnabledConfig;
        public static bool teamMarkerEnabled;
        public static ConfigEntry<float> TeamHeightConfig;
        public static ConfigEntry<float> TeamWidthConfig;
        public static Vector3 teamMarkerSize;

        private static GameObject enemyMarker;
        public static ConfigEntry<bool> EnemyEnabledConfig;
        public static bool enemyMarkerEnabled;
        public static ConfigEntry<float> EnemyHeightConfig;
        public static ConfigEntry<float> EnemyWidthConfig;
        public static Vector3 enemyMarkerSize;

        void Awake()
        {
            instance = this;

            // Use this to call any harmony patch files your mod may have
            var harmony = new Harmony(ModId);
            harmony.PatchAll();

            inGame = false;

            OwnEnabledConfig = Config.Bind(CompatibilityModName, "OwnEnabled", true, "Own Marker Enabled");
            OwnHeightConfig = Config.Bind(CompatibilityModName, "OwnHeight", 0.75f, "Own Marker Height");
            OwnWidthConfig = Config.Bind(CompatibilityModName, "OwnWidth", 0.75f, "Own Marker Width");

            TeamEnabledConfig = Config.Bind(CompatibilityModName, "TeamEnabled", true, "Team Marker Enabled");
            TeamHeightConfig = Config.Bind(CompatibilityModName, "TeamHeight", 0.75f, "Team Marker Height");
            TeamWidthConfig = Config.Bind(CompatibilityModName, "TeamWidth", 0.75f, "Team Marker Width");

            EnemyEnabledConfig = Config.Bind(CompatibilityModName, "EnemyEnabled", false, "Enemy Marker Enabled");
            EnemyHeightConfig = Config.Bind(CompatibilityModName, "EnemyHeight", 0.75f, "Enemy Marker Height");
            EnemyWidthConfig = Config.Bind(CompatibilityModName, "EnemyWidth", 0.75f, "Enemy Marker Width");
        }

        void Start()
        {
            Unbound.RegisterMenu("Player Markers", () => { }, GeneralSettings, null, false);

            GameModeManager.AddHook(GameModeHooks.HookGameStart, GameActions.GameStart);
            GameModeManager.AddHook(GameModeHooks.HookGameEnd, GameActions.GameEnd);

            Unbound.Instance.StartCoroutine(MarkerUpdater());
        }

        private static void GeneralSettings(GameObject menu)
        {
            var ownMarkerMenu = MenuHandler.CreateMenu("Own Marker Options", () => { }, menu, 60, true, true, menu.transform.parent.gameObject);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 5);
            OwnMarkerSettings(ownMarkerMenu);

            var teamMarkerMenu = MenuHandler.CreateMenu("Team Marker Options", () => { }, menu, 60, true, true, menu.transform.parent.gameObject);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 5);
            TeamMarkerSettings(teamMarkerMenu);

            var enemyMarkerMenu = MenuHandler.CreateMenu("Enemy Marker Options", () => { }, menu, 60, true, true, menu.transform.parent.gameObject);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 5);
            EnemyMarkerSettings(enemyMarkerMenu);
        }

        private static void OwnMarkerSettings(GameObject menu)
        {
            var ownMarkerText = MenuHandler.CreateText("Own Marker", menu, out TextMeshProUGUI _, 60);
            Unbound.Instance.StartCoroutine(FindScaler(ownMarkerText.transform, MarkerType.OWN));

            void EnabledChanged(bool val)
            {
                OwnEnabledConfig.Value = val;
                ownMarkerEnabled = OwnEnabledConfig.Value;
            }
            var markerToggle = MenuHandler.CreateToggle(OwnEnabledConfig.Value, "Marker Enabled", menu, EnabledChanged, 30);

            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);
            MenuHandler.CreateText("Marker Size:", menu, out TextMeshProUGUI _, 25);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            void HeightChanged(float val)
            {
                OwnHeightConfig.Value = UnityEngine.Mathf.Clamp(val, 0.1f, 1f);
                ownMarkerSize.x = OwnHeightConfig.Value;
            }
            MenuHandler.CreateSlider("Marker height", menu, 25, 0.1f, 1f, OwnHeightConfig.Value, HeightChanged, out UnityEngine.UI.Slider heightSlider, false);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            void WidthChanged(float val)
            {
                OwnWidthConfig.Value = UnityEngine.Mathf.Clamp(val, 0.1f, 1f);
                ownMarkerSize.y = OwnWidthConfig.Value;
            }
            MenuHandler.CreateSlider("Marker width", menu, 25, 0.1f, 1f, OwnWidthConfig.Value, WidthChanged, out UnityEngine.UI.Slider widthSlider, false);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            void ResetOptions()
            {
                EnabledChanged(true);
                markerToggle.GetComponent<Toggle>().isOn = true;
                HeightChanged(0.75f);
                heightSlider.value = 0.75f;
                WidthChanged(0.75f);
                widthSlider.value = 0.75f;
            }
            MenuHandler.CreateButton("Reset Options", menu, ResetOptions, 30);
        }

        private static void TeamMarkerSettings(GameObject menu)
        {
            var teamMarkerText = MenuHandler.CreateText("Team Marker", menu, out TextMeshProUGUI _, 60);
            Unbound.Instance.StartCoroutine(FindScaler(teamMarkerText.transform, MarkerType.TEAM));

            void EnabledChanged(bool val)
            {
                TeamEnabledConfig.Value = val;
                teamMarkerEnabled = TeamEnabledConfig.Value;
            }
            var markerToggle = MenuHandler.CreateToggle(TeamEnabledConfig.Value, "Marker Enabled", menu, EnabledChanged, 30);

            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);
            MenuHandler.CreateText("Marker Size:", menu, out TextMeshProUGUI _, 25);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            void HeightChanged(float val)
            {
                TeamHeightConfig.Value = UnityEngine.Mathf.Clamp(val, 0.1f, 1f);
                teamMarkerSize.x = TeamHeightConfig.Value;
            }
            MenuHandler.CreateSlider("Marker height", menu, 25, 0.1f, 1f, TeamHeightConfig.Value, HeightChanged, out UnityEngine.UI.Slider heightSlider, false);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            void WidthChanged(float val)
            {
                TeamWidthConfig.Value = UnityEngine.Mathf.Clamp(val, 0.1f, 1f);
                teamMarkerSize.y = TeamWidthConfig.Value;
            }
            MenuHandler.CreateSlider("Marker width", menu, 25, 0.1f, 1f, TeamWidthConfig.Value, WidthChanged, out UnityEngine.UI.Slider widthSlider, false);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            void ResetOptions()
            {
                EnabledChanged(true);
                markerToggle.GetComponent<Toggle>().isOn = true;
                HeightChanged(0.75f);
                heightSlider.value = 0.75f;
                WidthChanged(0.75f);
                widthSlider.value = 0.75f;
            }
            MenuHandler.CreateButton("Reset Options", menu, ResetOptions, 30);
        }

        private static void EnemyMarkerSettings(GameObject menu)
        {
            var enemyMarkerText = MenuHandler.CreateText("Enemy Marker", menu, out TextMeshProUGUI _, 60);
            Unbound.Instance.StartCoroutine(FindScaler(enemyMarkerText.transform, MarkerType.ENEMY));

            void EnabledChanged(bool val)
            {
                EnemyEnabledConfig.Value = val;
                enemyMarkerEnabled = EnemyEnabledConfig.Value;
            }
            var markerToggle = MenuHandler.CreateToggle(EnemyEnabledConfig.Value, "Marker Enabled", menu, EnabledChanged, 30);

            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);
            MenuHandler.CreateText("Marker Size:", menu, out TextMeshProUGUI _, 25);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            void HeightChanged(float val)
            {
                EnemyHeightConfig.Value = UnityEngine.Mathf.Clamp(val, 0.1f, 1f);
                enemyMarkerSize.x = EnemyHeightConfig.Value;
            }
            MenuHandler.CreateSlider("Marker height", menu, 25, 0.1f, 1f, EnemyHeightConfig.Value, HeightChanged, out UnityEngine.UI.Slider heightSlider, false);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            void WidthChanged(float val)
            {
                EnemyWidthConfig.Value = UnityEngine.Mathf.Clamp(val, 0.1f, 1f);
                enemyMarkerSize.y = EnemyWidthConfig.Value;
            }
            MenuHandler.CreateSlider("Marker width", menu, 25, 0.1f, 1f, EnemyWidthConfig.Value, WidthChanged, out UnityEngine.UI.Slider widthSlider, false);
            MenuHandler.CreateText(" ", menu, out TextMeshProUGUI _, 15);

            void ResetOptions()
            {
                EnabledChanged(true);
                markerToggle.GetComponent<Toggle>().isOn = true;
                HeightChanged(0.75f);
                heightSlider.value = 0.75f;
                WidthChanged(0.75f);
                widthSlider.value = 0.75f;
            }
            MenuHandler.CreateButton("Reset Options", menu, ResetOptions, 30);
        }

        private static IEnumerator FindScaler(Transform transform, MarkerType markerType)
        {
            while (FindInActiveObjectByName("PlayerScaler_Small", "FaceSelector") == null) 
            {
                yield return new WaitForSeconds(1);
            }

            var playerObject = FindInActiveObjectByName("PlayerScaler_Small", "FaceSelector");
            var activePlayerObject = Instantiate(playerObject, transform);

            activePlayerObject.name = "TestObjectPlayer";
            activePlayerObject.transform.localPosition = new Vector3(-420, 0, 0);
            activePlayerObject.transform.localScale = new Vector3(60, 60, 60);
            activePlayerObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
            var spriteRenderer = activePlayerObject.GetComponentInChildren<SpriteRenderer>();

            switch (markerType)
            {
                case (MarkerType.OWN):
                    {
                        var color = new Color(0.747f, 0.061f, 0.022f, 0.58f);
                        spriteRenderer.color = color;

                        ownMarker = ObjectManager.CreateObject("OwnMarker", color);
                        ownMarker.transform.SetParent(activePlayerObject.transform);
                        ownMarker.transform.localPosition = new Vector3(0, 2, 0);
                        ownMarker.transform.localScale = new Vector3(0.75f, 0.75f, 1);
                        ownMarker.transform.localRotation = Quaternion.Euler(0, 0, 180);
                        break;
                    }
                case (MarkerType.TEAM):
                    {
                        var color = new Color(0.047f, 0.061f, 0.722f, 0.58f);
                        spriteRenderer.color = color;

                        teamMarker = ObjectManager.CreateObject("TeamMarker", color);
                        teamMarker.transform.SetParent(activePlayerObject.transform);
                        teamMarker.transform.localPosition = new Vector3(0, 2, 0);
                        teamMarker.transform.localScale = new Vector3(0.75f, 0.75f, 1);
                        teamMarker.transform.localRotation = Quaternion.Euler(0, 0, 180);
                        break;
                    }
                case (MarkerType.ENEMY):
                    {
                        var color = new Color(0.047f, 0.761f, 0.122f, 0.58f);
                        spriteRenderer.color = color;

                        enemyMarker = ObjectManager.CreateObject("EnemyMarker", color);
                        enemyMarker.transform.SetParent(activePlayerObject.transform);
                        enemyMarker.transform.localPosition = new Vector3(0, 2, 0);
                        enemyMarker.transform.localScale = new Vector3(0.75f, 0.75f, 1);
                        enemyMarker.transform.localRotation = Quaternion.Euler(0, 0, 180);
                        break;
                    }
            }
            

            var gameObject = FindInActiveObjectByName("P_E_Neutral 33", "");
            var activeGameObject = Instantiate(gameObject, activePlayerObject.transform);

            activeGameObject.name = "TestDecoration";
            activeGameObject.transform.localPosition = new Vector3(0, 0, 0);
            activeGameObject.transform.localScale = new Vector3(0.15f, 0.15f, 1);
            activeGameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);

            spriteRenderer = activeGameObject.GetComponentInChildren<SpriteRenderer>();
            spriteRenderer.color = new Color(1f, 1f, 1f, 1f);

            gameObject = FindInActiveObjectByName("P_A_X12", "");
            activeGameObject = Instantiate(gameObject, activePlayerObject.transform);

            activeGameObject.name = "TestDecoration";
            activeGameObject.transform.localPosition = new Vector3(0, -0.5f, 0);
            activeGameObject.transform.localScale = new Vector3(0.235f, 0.235f, 1);
            activeGameObject.transform.localRotation = Quaternion.Euler(0, 180, 0);
        }

        private static IEnumerator MarkerUpdater()
        {
            while (!inGame)
            {
                if (ownMarker != null)
                {
                    ownMarker.SetActive(OwnEnabledConfig.Value);
                    ownMarker.transform.localScale = new Vector3(ownMarkerSize.y, ownMarkerSize.x, 1f);
                }
                if (teamMarker != null)
                {
                    teamMarker.SetActive(TeamEnabledConfig.Value);
                    teamMarker.transform.localScale = new Vector3(teamMarkerSize.y, teamMarkerSize.x, 1f);
                }
                if (enemyMarker != null)
                {
                    enemyMarker.SetActive(EnemyEnabledConfig.Value);
                    enemyMarker.transform.localScale = new Vector3(enemyMarkerSize.y, enemyMarkerSize.x, 1f);
                }
                yield return new WaitForEndOfFrame();
            }

            yield return new WaitForSeconds(10);
        }

        private static GameObject FindInActiveObjectByName(string name, string parent)
        {
            Transform[] objs = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
            for (int i = 0; i < objs.Length; i++)
            {
                if (objs[i].hideFlags == HideFlags.None)
                {
                    if (objs[i].name == name)
                    {
                        if (parent != "")
                        {
                            if (objs[i].parent.parent.parent.name == parent)
                            {
                                return objs[i].gameObject;
                            }
                        }
                        else
                        {
                            return objs[i].gameObject;
                        }
                    }
                }
            }               
            return null;
        }

        enum MarkerType
        {
            OWN,
            TEAM,
            ENEMY
        }
    }
}

