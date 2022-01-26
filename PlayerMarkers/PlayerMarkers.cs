using BepInEx;
using HarmonyLib;
using GameEnhancementCards.Utils;
using UnboundLib.GameModes;

namespace GameEnhancementCards
{
    // These are the mods required for our mod to work
    [BepInDependency("com.willis.rounds.unbound", BepInDependency.DependencyFlags.HardDependency)]
    [BepInDependency("pykess.rounds.plugins.moddingutils", BepInDependency.DependencyFlags.HardDependency)]
    // Declares our mod to Bepin
    [BepInPlugin(ModId, ModName, Version)]
    // The game our mod is associated with
    [BepInProcess("Rounds.exe")]
    public class PlayerMarkers : BaseUnityPlugin
    {
        private const string ModId = "ot.dan.rounds.PlayerMarkers";
        private const string ModName = "PlayerMarkers";
        public const string Version = "1.0.0";
        public const string ModInitials = "PM";
        public static PlayerMarkers instance { get; private set; }

        void Awake()
        {
            // Use this to call any harmony patch files your mod may have
            var harmony = new Harmony(ModId);
            harmony.PatchAll();
        }

        void Start()
        {
            instance = this;
            
            GameModeManager.AddHook(GameModeHooks.HookGameStart, GameActions.GameStart);
            GameModeManager.AddHook(GameModeHooks.HookGameEnd, GameActions.GameEnd);
        }
    }
}

