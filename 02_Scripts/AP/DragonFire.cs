using System.Collections;
using ChipmunkKingdoms.Scripts.Utility;
using Chuh007Lib.StatSystem;
using UnityEngine;
using Work.Chipmunk._01.Scripts.Heros;
using Work.CHUH._01Scripts.Combat;
using Work.CHUH._01Scripts.Entities;

public class DragonFire : MonoBehaviour, IContainerComponent, IAfterInitailze
{
    [SerializeField] private StatSO _damageStat;
    [SerializeField] private LayerMask _whatIsTarget;
    [SerializeField] private Hero _hero;
    public ComponentContainer ComponentContainer { get; set; }

    [Header("도트데미지")]
    [SerializeField] private float dotDuration = 1.2f;
    [SerializeField] private float dotTickInterval = 0.2f;
    [SerializeField] private float dotTickDamageMultiplier = 0.2f;

    private EntityStat _ownerStats;
    
    public void Initialize(EntityStat ownerStats, Hero castHero)
    {
        _ownerStats = ownerStats;
        _hero = castHero;
        
        if (_ownerStats != null && _damageStat != null)
            _damageStat = _ownerStats.GetStat(_damageStat);
    }
    
    public void DragonBreath()
    {
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, 4.5f, _whatIsTarget);

        foreach (Collider2D hit in targets)
        {
            EntityHealth targetEntity = hit.GetComponentInChildren<EntityHealth>();
            if (targetEntity != null)
            {
                float dotDamagePerTick = _damageStat.Value * dotTickDamageMultiplier;
                if (dotDamagePerTick > 0f && dotDuration > 0f && dotTickInterval > 0f)
                    targetEntity.StartCoroutine(DoDotCoroutine(targetEntity, dotDamagePerTick, dotDuration, dotTickInterval));

                TakeDamage(targetEntity);
            }
        }
    }
    
    private void TakeDamage(EntityHealth targetEntity)
    {
        targetEntity.TakeDamage(_damageStat.Value * 2.5f);
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

    public void AfterInitailized() { }

    public void OnInitialize(ComponentContainer componentContainer) { }
}
