using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace NielsDev.PlayerSkills
{
    public class RootAttackSkill : SkillBase
    {
        [Header("Root Attack Skill Settings")]
        [SerializeField] private ParticleSystem rootParticle = default;
        [SerializeField] private Transform particlePosition = default;
        public override void Ability()
        {
            Player_Animation.Instance.DoAnimation("RootAttack");
        }

        /// <summary>
        /// Is triggered by the RootAttack animation.
        /// </summary>
        public void TriggerRootAttack()
        {
            rootParticle.transform.rotation = particlePosition.rotation;
            rootParticle.transform.position = particlePosition.position;
            rootParticle.gameObject.SetActive(true);
            rootParticle.Play();
        }
    }
}