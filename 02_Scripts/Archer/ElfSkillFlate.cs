using ChipmunkKingdoms.Scripts.Utility;
using Chuh007Lib.StatSystem;
using DG.Tweening;
using UnityEngine;
using Work.CHUH._01Scripts.Combat;
using Work.CHUH._01Scripts.Entities;

public class ElfSkillFlate : MonoBehaviour
{
    [SerializeField] private StatSO _attackSpeedStat;
    [SerializeField] private StatSO _moveSpeedStat;
    [SerializeField] private LayerMask _whatIsTarget;
    [SerializeField] private float _slowValue = 0.6f;
    [SerializeField] private float _tickInterval = 0.5f;
    private float _radius = 3f;
    private float _tickTimer;
    
    public void Update()
    {
        _tickTimer -= Time.deltaTime;
        if (_tickTimer <= 0f)
        {
            ApplySlowToTargets();
            _tickTimer = _tickInterval;
        }
    }

    private void ApplySlowToTargets()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, _radius, _whatIsTarget);

        foreach (Collider2D hit in hits)
        {
            EntityHealth targetEntity =  hit.GetComponentInChildren<EntityHealth>();
            if (targetEntity != null)
            {
                EntityStat targetStat = targetEntity.entity.Get<EntityStat>();
                ChangeStat(_moveSpeedStat, targetStat, _slowValue);
                ChangeStat(_attackSpeedStat, targetStat, _slowValue);
            }
        }
    }
    
    private void ChangeStat(StatSO statToChange, EntityStat targetStatComponent, float value, float duration = 3)
    {
        StatSO targetRealStat = targetStatComponent.GetStat(statToChange);
        targetRealStat.RemoveModifier(this.GetType());
        targetRealStat.AddPercentModifier(this.GetType(), value);
        DOVirtual.DelayedCall(duration, () => targetRealStat.RemoveModifier(this));
    }
}
