using System.Collections.Generic;
using HarmonyLib;
// ReSharper disable InconsistentNaming

namespace EasyAttack
{
    public static class Patches
    {
        [HarmonyPatch(typeof(CombatManager), "Awake")]
        public static void Postfix(List<BodyTargettingButton> ___m_bodyPartButtons)
        {
            foreach (var button in ___m_bodyPartButtons)
            {
                var clickAttack = button.gameObject.AddComponent<EasyAttack>();
                clickAttack.BodyPart = button.bodyPart;
            }
        }
    }
}
