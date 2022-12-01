using AddLuaMods.Tools;
using HarmonyLib;

namespace AddLuaMods.MainMenu
{
    [HarmonyPatch]
    public static class MainMenuPatch
    {
        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::MainMenu), "Start")]
        public static bool MainMenu_Start(global::MainMenu __instance)
        {
            Logging.LogDebug("MainMenu_Start");

            var bag = new MainMenu(__instance);
            bag.Start();

            return false;
        }
    }
}
