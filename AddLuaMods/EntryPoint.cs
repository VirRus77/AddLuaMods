using System;
using System.Reflection;
using AddLuaMods.Tools;
using HarmonyLib;
using MoonSharp.Interpreter;
using UnityModManagerNet;

namespace AddLuaMods
{
    public static class EntryPoint
    {
        private static Harmony _harmony;
        // Send a response to the mod manager about the launch status, success or not.
        public static bool Load(UnityModManager.ModEntry modEntry)
        {
            try
            {
                _harmony = new Harmony(modEntry.Info.Id);

                var assembly = Assembly.GetExecutingAssembly();
                UserData.RegisterAssembly(assembly);

                Logging.LogDebug($"{nameof(AddLuaMods)}.{nameof(EntryPoint)}.{nameof(Load)} {assembly?.FullName}");
                Logging.LogDebug($"Type Mod: {typeof(Mod)?.FullName}");
                _harmony.PatchAll(assembly);
            }
            catch (Exception e)
            {
                UnityModManager.Logger.Error(e.Message);
                return false;
            }

            return true; // If false the mod will show an error.
        }
    }
}
