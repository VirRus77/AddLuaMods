using System;
using System.Linq;
using AddLuaMods.Tools;
using HarmonyLib;
using UnityEngine;
using UnityModManagerNet;

namespace AddLuaMods.ObjectTypeList
{
#if !Autonauts
    [HarmonyPatch]
    public static class ModelManagerPatch
    {
        private static bool ApplyPatch = false;
        private readonly static Version ApplyVersion = new Version(141, 14);

        private static bool _showEx = false;

        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::ModelManager), MethodType.Constructor)]
        public static void ModelManager_Constructor(
            global::ModelManager __instance
        )
        {
            Logging.LogDebug($"ModelManager_Constructor");

            ApplyPatch = UnityModManager.gameVersion <= ApplyVersion;
            Logging.LogDebug($"ModelManager_Constructor ApplyPatch: {ApplyPatch}");
            if (!ApplyPatch)
            {
                return;
            }

            var modManager = new ModelManager.ModelManager(__instance);
            Logging.LogDebug($"ModelManager_Constructor {modManager.m_TestObject} -> {(global::ObjectTypeList.m_Total)}");
            modManager.m_TestObject = global::ObjectTypeList.m_Total;
        }

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::ModelManager), nameof(global::ModelManager.MakeModelList))]
        public static void ModelManager_MakeModelList(global::ModelManager __instance)
        {
            if (!ApplyPatch)
            {
                return;
            }

            Logging.LogDebug($"ModelManager_MakeModelList");
            var modManager = new ModelManager.ModelManager(__instance);
            modManager.MakeModelList();
            Logging.LogDebug($"ModelManager_MakeModelList m_TestObject: {(int)modManager.m_TestObject} {modManager.m_TestObject}");
        }

#if DEBUG
        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::ModelManager), nameof(global::ModelManager.AddModel))]
        public static void ModelManager_AddModel(
            global::ModelManager __instance,
            string Name,
            ObjectType NewType,
            bool RandomVariants = false,
            bool ForceBuilding = false
        )
        {
            Logging.LogDebug($"ModelManager_AddModel");
            Logging.LogDebug($"ModelManager.AddModel \"{Name}\" {NewType} {RandomVariants} {ForceBuilding}");

            //var modManager = new ModelManager.ModelManager(__instance);
            //modManager.AddModel(Name, NewType, RandomVariants, ForceBuilding);
            //Logging.LogDebug($"ModelManager_MakeModelList m_TestObject: {(int)modManager.m_TestObject} {modManager.m_TestObject}");
            //return false;
        }
#endif

        [HarmonyPrefix]
        [HarmonyPatch(typeof(global::ModelManager), nameof(global::ModelManager.Load))]
        public static void ModelManager_Load(
            global::ModelManager __instance,
            ref GameObject __result,
            ObjectType NewType,
            string Name,
            bool RandomVariants
        )
        {
            Logging.LogDebug($"ModelManager_Load {(int)NewType}({NewType}), {Name}, {RandomVariants}");
            if (!ApplyPatch)
            {
                return;
            }

            if (_showEx)
            {
                return;
            }

            Logging.LogDebug($"ModelManager_Load Apply");

            var bag = new ModelManager.ModelManager(__instance);
            try
            {
                __result = bag.Load(NewType, Name, RandomVariants);
            }
            catch (Exception e)
            {
                Logging.LogException(Logging.LogLevel.Fatal, e, "ModelManager_Load");
                _showEx = true;
                throw;
            }
        }

#if DEBUG
        [HarmonyPostfix]
        [HarmonyPatch(typeof(global::ModelManager), "LoadNew")]
        public static void ModelManager_LoadNew(
            global::ModelManager __instance,
            string Name,
            ObjectType NewType = ObjectType.Nothing
        )
        {
            Logging.LogDebug($"ModelManager_LoadNew \"{Name}\", {(int)NewType}({NewType})");
            Logging.LogDebug($"ModelManager_LoadNew CallStack:\n{string.Join("\n", DebugStack.GetStackTrace().Select(v=>$" - {v}"))}");
        }
#endif

    }
#endif
    }
