using System.Collections;
using UnityEngine;
using Work.CHUH._01Scripts.Combat; 
using Projectile = Work.Chipmunk._01.Scripts.Combat.Projectile;

namespace Work.yeonwoo._02_Scripts.AP
{
    public class FireBall : Projectile
    {
        [Header("도트데미지")]
        [SerializeField] private float dotDuration = 5f;
        [SerializeField] private float dotTickInterval = 1f;
        [SerializeField] private float dotTickDamageMultiplier = 0.2f;

        public override void Update()
        {
            base.Move();
            base.TryAttack();
        }

        public override void Attack(EntityHealth target)
        {
            if (target == null) return;
            float dotDamagePerTick = DamageStat.Value * dotTickDamageMultiplier;
            if (dotDamagePerTick > 0f && dotDuration > 0f && dotTickInterval > 0f)
            {
                target.StartCoroutine(DoDotCoroutine(target, dotDamagePerTick, dotDuration, dotTickInterval));
            }
            else
            {
                base.Attack(target);
            }
        }

        private static IEnumerator DoDotCoroutine(EntityHealth target, float damagePerTick, float duration, float tickInterval)
        {
            float elapsed = 0f;
            while (elapsed < duration)
            {
                if (target == null) yield break;
                target.TakeDamage(damagePerTick);
                yield return new WaitForSeconds(tickInterval);
                elapsed += tickInterval;
            }
        }
    }
}


