using Ami.BroAudio;
using UnityEngine;
using UnityEngine.Serialization;
using Work.Chipmunk._01.Scripts.Heros;
using Work.CHUH._01Scripts.Entities;
using Work.yeonwoo._02_Scripts.Heros;

namespace Work.yeonwoo._02_Scripts
{
    public class SummonSkill : Skill
    {
        [SerializeField] protected Hero heroPrefab = null;

        public override void CallSkill()
        {
            if (_isOnCooldown)
                return;
            base.CallSkill();
            SummonHero();
            StartCooldown();
        }

        protected virtual void SummonHero()
        {
            HeroSpawner heroSpawner = HeroSpawner.Instance;
            heroSpawner.SpawnHeroAllIndex(heroPrefab.gameObject, ComponentContainer.Get<EntityStat>());
        }
    }
}