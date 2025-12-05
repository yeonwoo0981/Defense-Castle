using ChipmunkKingdoms.Scripts.Utility;
using Chuh007Lib.StatSystem;
using UnityEngine;
using Work.Chipmunk._01.Scripts.Heros;
using Work.CHUH._01Scripts.Combat;
using Work.CHUH._01Scripts.Entities;

public class Frost : MonoBehaviour, IContainerComponent, IAfterInitailze
{
    [SerializeField] private StatSO _damageStat;
    [SerializeField] private Hero _hero;
    [SerializeField] private LayerMask _whatIsTarget;
    public ComponentContainer ComponentContainer { get; set; }

    private float _stunTime = 1000;
    private EntityStat _ownerStats;
    
    public void Initialize(EntityStat ownerStats, StatSO baseDamageStat)
    {
        _ownerStats = ownerStats;
        if (_ownerStats != null && baseDamageStat != null)
            _damageStat = _ownerStats.GetStat(baseDamageStat);
    }

    private void TakeDamage(EntityHealth target)
    {
        if (_damageStat == null)
        {
            Debug.LogError("DamageStat이 초기화되지 않았습니다.");
            return;
        }

        target.TakeDamage(_damageStat.Value * 2);
    }

    private void Blizzard(GameObject obj)
    {
        Collider2D[] target = Physics2D.OverlapCircleAll(transform.position, 3f, _whatIsTarget);

        foreach (Collider2D hits in target)
        {
            IStunnable stunnable = hits.GetComponentInChildren<IStunnable>();
            if (stunnable != null)
            {
                if (obj.layer == LayerMask.NameToLayer("Boss"))
                {
                    stunnable.Stun(5);
                }
                else
                {
                    stunnable.Stun(_stunTime);
                }
            }
        }
    }

    public void EverFrost()
    {
        Collider2D[] target = Physics2D.OverlapCircleAll(transform.position, 3f, _whatIsTarget);

        foreach (Collider2D hits in target)
        {
            EntityHealth targetEntity = hits.GetComponentInChildren<EntityHealth>();
            if (targetEntity != null)
            {
                TakeDamage(targetEntity);
                Blizzard(hits.gameObject);
            }
        }
    }

    public void OnInitialize(ComponentContainer componentContainer) { }

    public void AfterInitailized() { }
}
