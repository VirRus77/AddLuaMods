using System.Linq;
using AddLuaMods.Tools;
using HarmonyLib;

namespace AddLuaMods.VariableDataSurvival
{
#if !Autonaut
    [HarmonyPatch]
    public static class VariableDataSurvival
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::VariableDataSurvival), "SetupEnabledObjects")]
        public static void VariableDataSurvival_SetupEnabledObjects(global::VariableDataSurvival __instance)
        {
            Logging.LogDebug("VariableDataSurvival_SetupEnabledObjects");
            if (global::ObjectTypeList.Instance == null)
            {
                return;
            }

            Logging.LogDebug("VariableDataSurvival_SetupEnabledObjects set Enable.");
            foreach (var objectType in global::ModManager.Instance.CurrentMods.SelectMany(v => v.CustomIDs)
                .Distinct())
            {
                global::ObjectTypeList.Instance.SetEnabled(objectType, true);
            }
        }
    }
#endif
}
