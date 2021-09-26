using UnityEngine;

namespace EasyAttack
{
    public class EasyAttack : MonoBehaviour
    {
        internal CombatManager.BodyPartTargets BodyPart;
        private static EncounterMember Character => CombatManager.instance.CurrentCharacter;

        private void Update()
        {
            if (Input.GetMouseButtonDown(1)
                && IsPlayersTurn(Character)
                && PlayerInput.IsMouseOverObject(gameObject))
            {
                CombatManager.instance.SetCurrentBodyPartTarget(BodyPart);
                var combatAction = CanShoot() &&
                                   !(Input.GetKey(KeyCode.LeftShift)
                                     || Input.GetKey(KeyCode.RightShift))
                    ? CombatManager.CombatActionEnum.ShootSingle
                    : CanMelee()
                        ? CombatManager.CombatActionEnum.Melee
                        : CombatManager.CombatActionEnum.Skip;

                if (combatAction == CombatManager.CombatActionEnum.Skip)
                {
                    return;
                }

                CombatManager.instance.ExecuteAction(Character, new CombatManager.CombatActionInfo
                {
                    action = combatAction,
                    target = Character.lastTargetedCharacter,
                });
            }
        }

        private bool CanShoot()
        {
            return Character.hasRangedWeapon
                   && Character.hasAmmo
                   && Character.Stamina >= CombatManager.instance.GetStaminaUsedForAction(CombatManager.CombatActionEnum.ShootSingle);
        }

        private bool CanMelee() => Character.Stamina >= CombatManager.instance.GetStaminaUsedForAction(CombatManager.CombatActionEnum.Melee);

        private bool IsPlayersTurn(EncounterMember memberReferenceHolder)
        {
            if (memberReferenceHolder != null && memberReferenceHolder.isPlayerControlled)
            {
                return true;
            }

            return false;
        }
    }
}
