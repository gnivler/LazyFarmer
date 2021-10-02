using System.Linq;
using HarmonyLib;
using UnityEngine;

// ReSharper disable InconsistentNaming

namespace LazyFarmer
{
    public static class Patches
    {
        [HarmonyPatch(typeof(Object_Planter), "Update")]
        public static void Postfix(Object_Planter __instance)
        {
            if (__instance.GStage == Object_Planter.GrowingStage.Harvestable
                && !__instance.beingUsed)
            {
                DoFarming<ObjectInteraction_HarvestPlant>(__instance);
            }
        }

        [HarmonyPatch(typeof(ObjectWarning), "IsWarningIconNeeded")]
        public static void Postfix(int warning, Object_Base ___m_obj, bool __result)
        {
            if (!___m_obj.beingUsed
                && ___m_obj is Object_Planter)
            {
                if (__result
                    && warning == 6)
                {
                    DoFarming<ObjectInteraction_WaterPlant>(___m_obj);
                }
            }
        }

        private static void DoFarming<T>(Object_Base ___m_obj) where T : ObjectInteraction_Base
        {
            var members = MemberManager.instance.GetAllShelteredMembers()
                .Where(m => !m.member.OutOnExpedition)
                .OrderBy(m => Vector3.SqrMagnitude(m.transform.position));
            foreach (var member in members)
            {
                if (member.member.currentjob is not null
                    || ___m_obj.IsSurfaceObject
                    && WeatherManager.instance.weatherActive
                    && WeatherManager.instance.currentDaysWeather == WeatherManager.WeatherState.BlackRain)
                {
                    continue;
                }

                Mod.Log($"Sending {member.name} to {___m_obj.name} - {typeof(T)}");
                var interaction = ___m_obj.GetComponent<T>();
                var job = new Job(member, ___m_obj, interaction, ___m_obj.transform)
                {
                    canJog = true
                };

                member.member.AddJob(job);
                member.member.currentjob = job;
                break;
            }
        }
    }
}
