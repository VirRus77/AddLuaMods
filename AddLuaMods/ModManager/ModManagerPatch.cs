using AddLuaMods.Tools;
using HarmonyLib;

namespace AddLuaMods.ModManager
{
#if DEBUG
    [HarmonyPatch]
    public static class ModManagerPatch
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::ModManager), nameof(global::ModManager.GetCustomClassData))]
        public static void ModManager_GetCustomClassData(
            global::ModelManager __instance,
            ObjectType ObjID,
            ref string UniqueName,
            ref string PrefabLocation,
            ref ObjectSubCategory SubCat,
            ref bool CanStack
        )
        {
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::ModManager), nameof(global::ModManager.GetModObjectTypeFromName))]
        public static void ModManager_GetModObjectTypeFromName(
            global::ModManager __instance,
            ref global::ObjectType __result,
            string Name
        )
        {
            Logging.LogDebug($"ModManager_GetModObjectTypeFromName \"{Name}\" = \"{__result}[{(int)__result}]\"");
            Logging.LogDebug($"ModManager_GetModObjectTypeFromName CallStack:\n{string.Join("\n", DebugStack.GetStackTrace())}");
        }
    }
#endif
}
