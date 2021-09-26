using BepInEx;
using HarmonyLib;

// ReSharper disable InconsistentNaming

namespace EasyAttack
{
    [BepInPlugin(PluginGUID, PluginName, PluginVersion)]
    public class Mod : BaseUnityPlugin
    {
        private const string PluginGUID = "ca.gnivler.sheltered2.EasyAttack";
        private const string PluginName = "EasyAttack";
        private const string PluginVersion = "1.0.0";

        private void Awake()
        {
            Harmony harmony = new("ca.gnivler.sheltered2.EasyAttack");
            Log("EasyAttack Startup");
            harmony.PatchAll(typeof(Patches));
        }

        internal static void Log(object input)
        {
            //File.AppendAllText("log.txt", $"{input ?? "null"}\n");
        }
    }
}
