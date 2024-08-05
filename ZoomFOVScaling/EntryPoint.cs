using BepInEx;
using BepInEx.Unity.IL2CPP;
using HarmonyLib;

namespace ZoomFOVScaling
{
    [BepInPlugin("Dinorush." + MODNAME, MODNAME, "1.0.0")]
    [BepInDependency("dev.gtfomodding.gtfo-api", BepInDependency.DependencyFlags.HardDependency)]
    internal sealed class EntryPoint : BasePlugin
    {
        public const string MODNAME = "ZoomFOVScaling";

        public override void Load()
        {
            Log.LogMessage("Loading " + MODNAME);
            Configuration.Init();
            new Harmony(MODNAME).PatchAll();
            Log.LogMessage("Loaded " + MODNAME);
        }
    }
}