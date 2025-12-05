using ChipmunkKingdoms.Scripts.Utility;
using Chuh007Lib.StatSystem;
using DG.Tweening;
using UnityEngine;
using Work.CHUH._01Scripts.Combat;
using Work.CHUH._01Scripts.Entities;
using Projectile = Work.Chipmunk._01.Scripts.Combat.Projectile;

namespace Work.yeonwoo._02_Scripts.AP
{
    public class IceBall : Projectile
    {
        [Header("슬로우")]
        [SerializeField] private StatSO moveSpeedStat;
        [SerializeField] private StatSO attackSpeedStat;
        [SerializeField] private float slowValue = 0.7f;

        public override void Update()
        {
            base.TryAttack();
            base.Move();
        }

        public override void Attack(EntityHealth target)
        {
            base.Attack(target);
            EntityStat targetStatComponent = target.entity.Get<EntityStat>();
            ChangeStat(moveSpeedStat, targetStatComponent, slowValue);
            ChangeStat(attackSpeedStat, targetStatComponent, slowValue);
        }

        private void ChangeStat(StatSO statToChange, EntityStat targetStatComponent, float value, float duration = 5)
        {
            StatSO targetRealStat = targetStatComponent.GetStat(statToChange);
            targetRealStat.AddPercentModifier(this.GetType(), slowValue);
            DOVirtual.DelayedCall(duration, () => targetRealStat.RemoveModifier(this));
        }
    }
}