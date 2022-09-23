using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sirenix.OdinInspector;

namespace NielsDev.PlayerSkills
{
    public class StompAttackSkill : SkillBase
    {
        [Header("Stomp Attack Skill Settings")]
        [SerializeField] private float stompKillRadius = 5.0f;
        [SerializeField] private float stompFallOverRadius = 15.0f;
        [SerializeField] private float forceMultiplier = 1500;
        [SerializeField] private float upwardsModifier = 1500f;
        [SerializeField] private Transform explosionTransform = default;

        private bool debugStompRadius = false;

        [SerializeField] private ParticleSystem shockwave = default;
        public override void Ability()
        {
            Player_Animation.Instance.DoAnimation("StompAttack");
        }

        /// <summary>
        /// Is triggered by the Animation.
        /// </summary>
        public void StompDamage()
        {
            Vector3 stompPosition = explosionTransform.position;
            Collider[] colliders = Physics.OverlapSphere(stompPosition, stompKillRadius);

            foreach (Collider hit in colliders)
            {
                AI_Physics ai = hit.GetComponent<AI_Physics>();

                if (ai != null)
                    ai.OnStomped(forceMultiplier, stompPosition, stompKillRadius, upwardsModifier);
            }

            colliders = Physics.OverlapSphere(stompPosition, stompFallOverRadius);

            foreach (Collider hit in colliders)
            {
                AICharacterStateMachine ai = hit.GetComponent<AICharacterStateMachine>();

                if (ai != null)
                    ai.InvokeStumble();
            }

            shockwave.Play();
        }

        
        [Button("Show Stomp Radius")]
        private void ShowStompRadius()
        {
            debugStompRadius = !debugStompRadius;
        }

        private void OnDrawGizmosSelected()
        {
            if (debugStompRadius)
            {
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(explosionTransform.position, stompKillRadius);
                Gizmos.color = Color.yellow;
                Gizmos.DrawWireSphere(explosionTransform.position, stompFallOverRadius);
            }
        }
        
    }
}