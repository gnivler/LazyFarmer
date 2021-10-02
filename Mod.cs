using BepInEx;
using HarmonyLib;

// ReSharper disable InconsistentNaming

namespace LazyFarmer
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Mod : BaseUnityPlugin
    {
        private const string PluginGUID = "ca.gnivler.sheltered2.LazyFarmer";
        private const string PluginName = "LazyFarmer";
        private const string PluginVersion = "1.0.0";

        private void Awake()
        {
            Harmony harmony = new("ca.gnivler.sheltered2.LazyFarmer");
            Log("LazyFarmer Startup");
            harmony.PatchAll(typeof(Patches));
        }

        internal static void Log(object input)
        {
            //File.AppendAllText("log.txt", $"{input ?? "null"}\n");
        }
    }
}
