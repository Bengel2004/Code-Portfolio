using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace NielsDev.PlayerSkills
{
    public abstract class SkillBase : MonoBehaviour
    {
        [Header("Default Skill Settings")]
        public PlayerSkills.SkillType skillType;
        public float cooldownTime = 10f;
        public bool canUse = true;
        public bool isUnlocked => Player_Stats.P_Skills.IsSkillUnlocked(skillType);
        // Voor implementatie kan je kijken naar alle abilities, die enabled zijn en dan daarvan de icoontjes te plaatsen

        public virtual void TriggerAbility(InputAction.CallbackContext context)
        {
            if (context.performed && canUse && !Player_Stats.playerIsAttacking)
            {
                Ability();
                StartCooldown();
            }
        }
        public abstract void Ability();
        protected void StartCooldown()
        {
            StartCoroutine(Cooldown());
            IEnumerator Cooldown()
            {
                canUse = false;
                yield return new WaitForSeconds(cooldownTime);
                canUse = true;
            }
        }
    }
}