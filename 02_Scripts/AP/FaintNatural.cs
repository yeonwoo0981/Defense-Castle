using System.Collections.Generic;
using ChipmunkKingdoms.Scripts.Utility;
using UnityEngine;
using Chuh007Lib.StatSystem;
using Work.CHUH._01Scripts.Combat;
using Work.CHUH._01Scripts.Entities;

public class FaintNatural : MonoBehaviour, IContainerComponent, IAfterInitailze
{
    [Header("SO")]
    [SerializeField] private StatSO _detectRangeStat;

    [Header("능력치")] 
    [SerializeField] private float _stunDuration = 1.5f;
    [SerializeField] private float _skillCool = 1.5f;

    [Header("적 찾기")] 
    [SerializeField] private LayerMask _whatIsTarget;
    
    [Header("VFX")]
    [SerializeField] private GameObject _skillVFX;

    private float _coolTimer;
    
    public ComponentContainer ComponentContainer { get; set; }
    
    public void OnInitialize(ComponentContainer componentContainer)
    {
    }
    
    private void Update()
    {
        UpdateCooldown();

        if (IsSkillReady())
        {
            if (TryAutoStun())
            {
                ResetCooldown();
            }
        }
    }
    
    private void UpdateCooldown()
    {
        _coolTimer = Mathf.Max(0, _coolTimer - Time.deltaTime);
    }
    
    private bool IsSkillReady() => _coolTimer <= 0f;
    
    private void ResetCooldown() => _coolTimer = _skillCool;
    
    private bool TryAutoStun()
    {
        var target = FindClosestStunnable();
        if (target == null) return false;

        ApplyStun(target);
        SpawnVFX(target);
        return true;
    }
    
    private IStunnable FindClosestStunnable()
    {
        float detectRange = _detectRangeStat.Value; 
        Vector2 origin = transform.position;

        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, detectRange, _whatIsTarget);

        IStunnable closest = null;
        float closestDist = float.MaxValue;

        foreach (var col in hits)
        {
            if (col.TryGetComponent<IStunnable>(out var stun))
            {
                float sq = ((Vector2)col.transform.position - origin).sqrMagnitude;
                if (sq < closestDist)
                {
                    closestDist = sq;
                    closest = stun;
                }
            }
        }

        return closest;
    }
    
    private void ApplyStun(IStunnable target)
    {
        target.Stun(_stunDuration);
    }
    
    private void SpawnVFX(IStunnable target)
    {
        if (_skillVFX == null) return;
        if (target is Component comp)
        {
            Instantiate(_skillVFX, comp.transform.position, Quaternion.identity);
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (_detectRangeStat == null) return;
        Gizmos.color = new Color(0.1f, 1f, 0.4f, 0.8f);
        Gizmos.DrawWireSphere(transform.position, _detectRangeStat.Value);
    }

    public void AfterInitailized()
    {
        _detectRangeStat = this.Get<EntityStat>().GetStat(_detectRangeStat);
    }
}
