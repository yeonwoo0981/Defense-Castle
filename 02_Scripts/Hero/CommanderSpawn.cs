using Chipmunk.GameEvents;
using UnityEngine;
using Work.CHUH._01Scripts.Enemies.Wave.GameEvents;
using Work.CHUH._01Scripts.Entities;
using Work.yeonwoo._02_Scripts.Heros;

public class CommanderSpawn : Skill
{
    [SerializeField] protected GameObject _commanderPrefab;
    private HeroSpawner _heroSpawner;

    protected override void Awake()
    {
        base.Awake();
        if (_heroSpawner == null)
            _heroSpawner = HeroSpawner.Instance;
    }

    public override void CallSkill()
    {
        if (_isOnCooldown)
            return;
        base.CallSkill();
        _heroSpawner.SpawnAtIndex(_commanderPrefab, 7, ComponentContainer.Get<EntityStat>());
        StartCooldown();
    }

    private void Update()
    {
        if (_isOnCooldown)
        {
            UpdateCooldownUI();
        }
    }

    private void UpdateCooldownUI()
    {
        
    }
}