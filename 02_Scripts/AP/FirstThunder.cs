using ChipmunkKingdoms.Scripts.Utility;
using Chuh007Lib.StatSystem;
using UnityEngine;
using Work.CHUH._01Scripts.Combat;
using Work.CHUH._01Scripts.Entities;

public class FirstThunder : MonoBehaviour, IContainerComponent, IAfterInitailze
{
    [Header("Stats")]
    [SerializeField] protected StatSO _damageStat;
    [SerializeField] protected LayerMask _whatIsTarget;

    private EntityStat _ownerStats;

    public ComponentContainer ComponentContainer { get; set; }
    
    public void Initialize(EntityStat ownerStats, StatSO baseDamageStat)
    {
        _ownerStats = ownerStats;
        if (_ownerStats != null && baseDamageStat != null)
            _damageStat = _ownerStats.GetStat(baseDamageStat);
    }

    public virtual void ThunderAttack()
    {
        Collider2D[] target = Physics2D.OverlapCircleAll(transform.position, 3f, _whatIsTarget);

        foreach (Collider2D hits in target)
        {
            EntityHealth targetEntity = hits.GetComponentInChildren<EntityHealth>();
            if (targetEntity != null)
            {
                TakeDamage(targetEntity);
            }
        }
    }

    protected virtual void TakeDamage(EntityHealth target)
    {
        if (_damageStat == null)
        {
            Debug.LogError("DamageStat이 초기화되지 않았습니다.");
            return;
        }

        target.TakeDamage(_damageStat.Value);
    }

    public void OnInitialize(ComponentContainer componentContainer) { }
    public void AfterInitailized() { }
}