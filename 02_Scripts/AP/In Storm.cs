using System.Collections;
using ChipmunkKingdoms.Scripts.Utility;
using Chuh007Lib.StatSystem;
using UnityEngine;
using Work.Chipmunk._01.Scripts.Heros;
using Work.CHUH._01Scripts.Combat;
using Work.CHUH._01Scripts.Entities;

public class InStorm : MonoBehaviour, IContainerComponent, IAfterInitailze
{
    [Header("SO")]
    [SerializeField] protected StatSO _damageStat;

    [SerializeField] private Hero _hero;

    [Header("적 정보")]
    [SerializeField] private LayerMask _whatIsTarget;
    
    [Header("뎀 간격")]
    [SerializeField] private float _tickInterval = 0.5f;
    [SerializeField] private float _duration = 3f;
    
    private Coroutine _stormCoroutine;
    private EntityStat _ownerStats;

    public void Initialize(EntityStat ownerStats, Hero castHero)
    {
        _ownerStats = ownerStats;
        _hero = castHero;

        if (_ownerStats != null && _damageStat != null)
            _damageStat = _ownerStats.GetStat(_damageStat);
    }

    public void StartWindStorm()
    {
        if (_stormCoroutine != null)
            StopCoroutine(_stormCoroutine);

        _stormCoroutine = StartCoroutine(WindStormCoroutine());
    }
    
    private IEnumerator WindStormCoroutine()
    {
        float elapsed = 0f;

        while (elapsed < _duration)
        {
            WindStorm();
            yield return new WaitForSeconds(_tickInterval);
            elapsed += _tickInterval;
        }

        _stormCoroutine = null;
    }

    protected void WindStorm()
    {
        Collider2D[] target = Physics2D.OverlapCircleAll(transform.position, 3f, _whatIsTarget);
        
        foreach (Collider2D hits in target)
        {
            EntityHealth targetEntity;
            targetEntity = hits.GetComponentInChildren<EntityHealth>();
            if (targetEntity != null)
            {
                TakeDamage(targetEntity, hits.gameObject);
            }
        }
    }

    protected void TakeDamage(EntityHealth target, GameObject flyObj)
    {
        float damage = _damageStat.Value;
        if (flyObj.layer == LayerMask.NameToLayer("FlyEnemy"))
        {
            damage *= 1.5f;
        }
        target.TakeDamage(damage);
    }

    public ComponentContainer ComponentContainer { get; set; }
    public void OnInitialize(ComponentContainer componentContainer)
    {
        
    }

    public void AfterInitailized()
    {
       
    }
}
