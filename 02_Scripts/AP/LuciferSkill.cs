using System;
using Chipmunk.GameEvents;
using ChipmunkKingdoms.Scripts.Utility;
using Chuh007Lib.StatSystem;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Serialization;
using Work.CHUH._01Scripts.Enemies.Wave.GameEvents;
using Work.CHUH._01Scripts.Entities;

namespace Work.yeonwoo._02_Scripts.AP
{
    public class LuciferSkill : Skill, IAfterInitailze
    {
        [Header("이펙트와 힐량")]
        [SerializeField] private GameObject _healEffect;
        [field:SerializeField] protected StatSO _damageStat { get; set; }
        
        [FormerlySerializedAs("_castleHealth")] 
        [Header("성 정보")]
        public Castle castle;
        
        protected override void Awake()
        {
            EventBus<WaveEndEvent>.OnEvent += WaveEndEvent;
            castle = FindAnyObjectByType<Castle>();
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
            float heal = _damageStat.Value * 3;
            castle.Heal(heal);
            Debug.Log($"성 체력이 {heal}만큼 회복된 다람쥐, {_coolTime}초 뒤에 다시 사용할 수 있는 다람쥐.");
            Instantiate(_healEffect, castle.transform.position, Quaternion.identity);
            StartCooldown();
        }

        public void AfterInitailized()
        {
            _damageStat = this.Get<EntityStat>().GetStat(_damageStat);
        }
    }
}
