// using System.Globalization;
// using Chipmunk.GameEvents;
// using UnityEngine;
// using TMPro;
// using Work.CHUH._01Scripts.Enemies.Wave.GameEvents;
// using Work.yeonwoo._02_Scripts.Heros;
//
// namespace Work.yeonwoo._02_Scripts.Knight
// {
//     [DisallowMultipleComponent]
//     public class KnightSkill : Skill
//     {
//         private HeroSpawner _heroSpawner;
//         [SerializeField] protected GameObject _heroPrefab;
//
//         public void Awake()
//         {
//             EventBus<WaveEndEvent>.OnEvent += WaveEndEvent;
//             if (_heroSpawner == null)
//                 _heroSpawner = HeroSpawner.Instance;
//         }
//         
//         private void WaveEndEvent(WaveEndEvent evt)
//         {
//             EndCooldown();
//         }
//
//         public override void CallSkill()
//         {
//             if (_isOnCooldown)
//                 return;
//
//             int[] indices = { 5, 6, 7, 8, 9 };
//             _heroSpawner.SpawnHeroFreeIndex(_heroPrefab, indices);
//             StartCooldown();
//         }
//
//         private void StartCooldown()
//         {
//             _isOnCooldown = true;
//             UpdateCooldownUI();
//         }
//
//         public void Update()
//         {
//             if (!_isOnCooldown) return;
//
//             UpdateCooldownUI();
//         }
//
//         protected virtual void UpdateCooldownUI()
//         {
//             
//         }
//
//         private void EndCooldown()
//         {
//             _isOnCooldown = false;
//         }
//     }
// }