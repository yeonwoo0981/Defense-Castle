using System;
using ChipmunkKingdoms.Scripts.Utility;
using Chuh007Lib.StatSystem;
using UnityEngine;
using Work.Chipmunk._01.Scripts.Heros;
using Work.CHUH._01Scripts.Combat;
using Work.CHUH._01Scripts.Entities;

namespace Work.yeonwoo._02_Scripts.CallSkill
{
    public class AssassinEffect : MonoBehaviour
    {
        [SerializeField] private GameObject _heroPrefab;
        [SerializeField] private float _damage = 3000;
        private StatSO _powerStat;
        private EntityStat ownerStats;
        private EntityHealth target;

        public void Initialize(EntityHealth target, StatSO powerStat, EntityStat entityStat)
        {
            this.target = target;
            _powerStat = powerStat;
            this.ownerStats = entityStat;
        }

        public void InstEffect()
        {
            TakeDamage(target.gameObject);
        }

        private void TakeDamage(GameObject obj)
        {
            if (obj.layer == LayerMask.NameToLayer("Boss"))
            {
                target.TakeDamage(_powerStat.Value * 5);
            }
            else
            {
                target.TakeDamage(_powerStat.Value * _damage);
            }
        }
        
        public void InstAssassin()
        {
            GameObject heroObject = Instantiate(_heroPrefab, transform.position, Quaternion.Euler(0, 180, 0));
            Hero hero = heroObject.GetComponent<Hero>();
            if(hero == null)
                Debug.LogError("Hero object is null");
            ComponentContainer componentContainer = hero.GetComponentInChildren<ComponentContainer>();
            componentContainer.AddComponentToDictionary(ownerStats);
            componentContainer.InitializeComponentContainer();
            
        }
    }
}