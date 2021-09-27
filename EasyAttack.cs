using System.Collections.Generic;
using System.Security.AccessControl;
using HarmonyLib;
using UnityEngine;

namespace EasyAttack
{
    public class EasyAttack : MonoBehaviour
    {
        internal CombatManager.BodyPartTargets BodyPart;
        private static EncounterMember Character => CombatManager.instance.CurrentCharacter;
        private static CombatManager Cm => CombatManager.instance;

        private void Update()
        {
            if (Input.GetMouseButtonDown(1)
                && IsPlayersTurn(Character)
                && PlayerInput.IsMouseOverObject(gameObject))
            {
                Cm.SetCurrentBodyPartTarget(BodyPart);
                var combatAction = CanShoot() &&
                                   !(Input.GetKey(KeyCode.LeftShift)
                                     || Input.GetKey(KeyCode.RightShift))
                    ? CombatManager.CombatActionEnum.ShootSingle
                    : CanMelee()
                        ? CombatManager.CombatActionEnum.Melee
                        : CombatManager.CombatActionEnum.Skip;

                if (combatAction == CombatManager.CombatActionEnum.Skip
                    || combatAction == CombatManager.CombatActionEnum.Melee
                    && !Cm.CheckCQCMoveValidityWithPassedTarget(Cm.TargetCharacter))
                {
                    return;
                }

                Cm.ExecuteAction(Character, new CombatManager.CombatActionInfo
                {
                    action = combatAction,
                    target = Cm.TargetCharacter,
                });
            }
        }

        private static bool CanShoot()
        {
            return Character.hasRangedWeapon
                   && Character.hasAmmo
                   && Character.Stamina >= Cm.GetStaminaUsedForAction(CombatManager.CombatActionEnum.ShootSingle);
        }

        private static bool CanMelee() => Character.Stamina >= Cm.GetStaminaUsedForAction(CombatManager.CombatActionEnum.Melee);

        private static bool IsPlayersTurn(EncounterMember memberReferenceHolder)
        {
            if (memberReferenceHolder != null && memberReferenceHolder.isPlayerControlled)
            {
                return true;
            }

            return false;
        }
    }
}
