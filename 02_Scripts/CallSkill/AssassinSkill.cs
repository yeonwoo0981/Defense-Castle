using System;
using Chipmunk.GameEvents;
using ChipmunkKingdoms.Scripts.Utility;
using Chuh007Lib.StatSystem;
using DG.Tweening;
using UnityEngine;
using Work.CHUH._01Scripts.Combat;
using Work.CHUH._01Scripts.Enemies.Wave.GameEvents;
using Work.CHUH._01Scripts.Entities;
using Work.yeonwoo._02_Scripts;
using Work.yeonwoo._02_Scripts.CallSkill;

public class AssassinSkill : Skill
{
    [SerializeField] private GameObject _executionEffect;
    [SerializeField] private LayerMask _whatIsTarget;
    [SerializeField] private float _detectRangeStat = 15f;

    [SerializeField] private StatSO _attackStat;

    private void Awake()
    {
        EventBus<WaveEndEvent>.OnEvent += WaveEndEvent;
    }

    private void WaveEndEvent(WaveEndEvent evt)
    {
        CancelCooldown();
    }

    public override void CallSkill()
    {
        if (_isOnCooldown)
            return;
        base.CallSkill();
        Transform target = FindClosestTarget();
        if (target != null && target.TryGetComponent(out ComponentContainer container))
        {
            if (container.TryGetComponent(out EntityHealth targetHealth))
            {
                GameObject assassinEffectGameObject =
                    Instantiate(_executionEffect, target.position, Quaternion.identity);
                AssassinEffect assassinEffectEffect = assassinEffectGameObject.GetComponent<AssassinEffect>();
                assassinEffectEffect.Initialize(targetHealth, _attackStat, ComponentContainer.Get<EntityStat>());
                StartCooldown();
            }
        }
    }

    private Transform FindClosestTarget()
    {
        Vector2 origin = transform.position;
        Collider2D[] hits = Physics2D.OverlapCircleAll(origin, _detectRangeStat, _whatIsTarget);

        Transform closest = null;
        float closestDist = float.MaxValue;

        foreach (var col in hits)
        {
            float sq = ((Vector2)col.transform.position - origin).sqrMagnitude;
            if (sq < closestDist)
            {
                closestDist = sq;
                closest = col.transform;
            }
        }

        return closest;
    }
}