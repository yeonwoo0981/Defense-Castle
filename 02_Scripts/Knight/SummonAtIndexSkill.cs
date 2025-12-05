using System;
using Chipmunk.GameEvents;
using UnityEngine;
using Work.CHUH._01Scripts.Enemies.Wave.GameEvents;
using Work.CHUH._01Scripts.Entities;
using Work.yeonwoo._02_Scripts.Heros;

namespace Work.yeonwoo._02_Scripts.Knight
{
    [DisallowMultipleComponent]
    public class SummonAtIndexSkill : SummonSkill
    {
        [SerializeField] int[] indices = { 0, 1, 2, 3, 4 };
        
        protected override void SummonHero()
        {
            HeroSpawner heroSpawner = HeroSpawner.Instance;
            heroSpawner.SpawnHeroFreeIndex(heroPrefab.gameObject,  indices,ComponentContainer.Get<EntityStat>());
        }
    }
}