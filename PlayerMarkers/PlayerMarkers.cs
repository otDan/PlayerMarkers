using BepInEx;
using HarmonyLib;
using UnboundLib.GameModes;
using UnboundLib;
using UnboundLib.Utils.UI;
using UnityEngine;
using TMPro;
using BepInEx.Configuration;
using Photon.Pun;
using System.Collections;
using UnityEngine.UI;
using PlayerMarkers.Menu;
using PlayerMarkers.Util;

namespace PlayerMarkers
{
    // These are the mods required for our mod to work
    [BepInDependency("com.willis.rounds.unbound")]
    // Declares our mod to Bepin
    [BepInPlugin(ModId, ModName, Version)]
    // The game our mod is associated with
    [BepInProcess("Rounds.exe")]
    public class PlayerMarkers : BaseUnityPlugin
    {
        private const string ModId = "ot.dan.rounds.playermarkers";
        private const string ModName = "Player Markers";
        public const string Version = "2.0.0";
        public const string ModInitials = "PM";
        private const string CompatibilityModName = "PlayerMarkers";
        public static PlayerMarkers instance { get; private set; }

        private void Awake()
        {
            instance = this;
            Unbound.RegisterClientSideMod(ModId);

            // Use this to call any harmony patch files your mod may have
            var harmony = new Harmony(ModId);
            harmony.PatchAll();

            SetupConfig();
        }

        private void Start()
        {
            GameModeManager.AddHook(GameModeHooks.HookGameStart, GameActions.GameStart);
            GameModeManager.AddHook(GameModeHooks.HookGameEnd, GameActions.GameEnd);

            MenuController.Initialize();
        }

        private void SetupConfig()
        {
            ConfigController.OwnEnabledConfig = Config.Bind(CompatibilityModName, "OwnEnabled", true, "Own Marker Enabled");
            ConfigController.OwnHeightConfig = Config.Bind(CompatibilityModName, "OwnHeight", 0.55f, "Own Marker Height");
            ConfigController.OwnWidthConfig = Config.Bind(CompatibilityModName, "OwnWidth", 0.55f, "Own Marker Width");
            ConfigController.OwnBloomConfig = Config.Bind(CompatibilityModName, "OwnBloom", 3f, "Own Marker Bloom");
            ConfigController.OwnMarkerTypeConfig = Config.Bind(CompatibilityModName, "OwnMarkerType", 1, "Own Marker Type");

            ConfigController.TeamEnabledConfig = Config.Bind(CompatibilityModName, "TeamEnabled", true, "Team Marker Enabled");
            ConfigController.TeamHeightConfig = Config.Bind(CompatibilityModName, "TeamHeight", 0.75f, "Team Marker Height");
            ConfigController.TeamWidthConfig = Config.Bind(CompatibilityModName, "TeamWidth", 0.75f, "Team Marker Width");
            ConfigController.TeamBloomConfig = Config.Bind(CompatibilityModName, "TeamBloom", 3f, "Team Marker Bloom");
            ConfigController.TeamMarkerTypeConfig = Config.Bind(CompatibilityModName, "TeamMarkerType", 1, "Team Marker Type");

            ConfigController.EnemyEnabledConfig = Config.Bind(CompatibilityModName, "EnemyEnabled", false, "Enemy Marker Enabled");
            ConfigController.EnemyHeightConfig = Config.Bind(CompatibilityModName, "EnemyHeight", 0.75f, "Enemy Marker Height");
            ConfigController.EnemyWidthConfig = Config.Bind(CompatibilityModName, "EnemyWidth", 0.75f, "Enemy Marker Width");
            ConfigController.EnemyBloomConfig = Config.Bind(CompatibilityModName, "EnemyBloom", 3f, "Enemy Marker Bloom");
            ConfigController.EnemyMarkerTypeConfig = Config.Bind(CompatibilityModName, "EnemyMarkerType", 1, "Enemy Marker Type");
        }
    }
}

