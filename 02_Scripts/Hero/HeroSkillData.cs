using System;
using System.Collections.Generic;
using Chipmunk.GameEvents;
using UnityEngine;
using Work.Chipmunk._01.Scripts.Heros;
using Work.CHUH._01Scripts.Enemies.Wave.GameEvents;

namespace Work.yeonwoo._02_Scripts.Heros
{
    public class HeroSkillData : MonoBehaviour
    {
        public List<HeroPlaceTile> _skills = new List<HeroPlaceTile>();

        private bool _isBattle = false;
        
        private void Awake()
        {
            EventBus<WaveStartEvent>.OnEvent += WaveStartHandler;
            EventBus<WaveEndEvent>.OnEvent += WaveEndHandler;
        }

        private void WaveStartHandler(WaveStartEvent evt)
        {
            _isBattle = true;
        }

        private void WaveEndHandler(WaveEndEvent evt)
        {
            _isBattle = false;
        }

        private void Update()
        {
            if (!_isBattle)
            {
                return;
            }
            KeyBoardSkill();
        }

        private void KeyBoardSkill()
        {
            for (int i = 0; i < MathF.Min(_skills.Count, 9); i++)
            {
                if (Input.GetKeyDown(KeyCode.Alpha1 + i))
                {
                    HeroPlaceTile heroPlaceTile = _skills[i];
                    if (heroPlaceTile != null)
                    {
                        heroPlaceTile.CallHeroSkill();
                    }
                }
            }
        }
    }
}
        
    