using AddLuaMods.Tools;
using HarmonyLib;

namespace AddLuaMods.ObjectTypeList
{
#if DEBUG
    [HarmonyPatch]
    public static class ObjectTypeList
    {
        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::ObjectTypeList), MethodType.Constructor)]
        public static void ObjectTypeList_Constructor(global::ObjectTypeList __instance)
        {
            Logging.LogDebug("ObjectTypeList_Constructor");
            Logging.LogDebug($"ModManager.Instance.CustomCreations: {(global::ModManager.Instance.CustomCreations)}");
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::ObjectTypeList), nameof(global::ObjectTypeList.GetEnabled))]
        public static void ObjectTypeList_GetEnabled(global::ObjectTypeList __instance, ref bool __result, ObjectType NewType)
        {
            Logging.LogDebug($"ObjectTypeList_GetEnabled \"{NewType}\" = {__result}");
        }

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::ObjectTypeList), nameof(global::ObjectTypeList.SetEnabled))]
        public static void ObjectTypeList_SetEnabled(global::ObjectTypeList __instance, ObjectType NewType, bool Enabled)
        {
            Logging.LogDebug($"ObjectTypeList_SetEnabled \"{NewType}\" = {NewType}, Enabled = {Enabled}");
            if (NewType > ObjectType.Total)
            {
                Logging.LogDebug($"ObjectTypeList_SetEnabled CallStack:\n{string.Join("\n", DebugStack.GetStackTrace())}");
            }
        }
    }
#endif
}
