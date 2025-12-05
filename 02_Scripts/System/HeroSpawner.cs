using System;
using ChipmunkKingdoms.Scripts.Utility;
using UnityEngine;
using Work.CHUH._01Scripts.Entities;

namespace Work.yeonwoo._02_Scripts.Heros
{
    public class HeroSpawner : MonoBehaviour
    {
        private static HeroSpawner instance;

        public static HeroSpawner Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindAnyObjectByType<HeroSpawner>();
                    if (instance == null)
                    {
                        Debug.LogError("히어로스포너 인스턴스가 존재하지 않습니다.");
                    }
                }

                return instance;
            }
        }

        private void Awake()
        {
            if (instance != null && instance != this)
            {
                Debug.Log(
                    $"Another Instance ({instance.gameObject.name}) is exits, Destroy This Instance({gameObject.name}).");
                Destroy(gameObject);
            }
            else
            {
                instance = this;
            }
        }

        [SerializeField] private Transform[] _spawnPoints;

        public void SpawnHeroAllIndex(GameObject prefab, EntityStat ownerStats)
        {
            for (int i = 0; i < _spawnPoints.Length; i++)
            {
                SpawnAtIndex(prefab, i, ownerStats);
            }
        }

        public void SpawnHeroFreeIndex(GameObject prefab, int[] indices, EntityStat ownerStats)
        {
            foreach (int index in indices)
            {
                SpawnAtIndex(prefab, index, ownerStats);
            }
        }

        public GameObject SpawnAtIndex(GameObject prefab, int index, EntityStat ownerStats)
        {
            if (_spawnPoints == null || _spawnPoints.Length == 0)
                return null;

            if (index < 0 || index >= _spawnPoints.Length)
                return null;

            Transform spawnTransform = _spawnPoints[index] != null ? _spawnPoints[index] : transform;
            GameObject hero = Instantiate(prefab, spawnTransform.position, Quaternion.Euler(0, 180, 0));
            
            ComponentContainer container = hero.GetComponentInChildren<ComponentContainer>();
            container.AddComponentToDictionary(ownerStats);
            container.InitializeComponentContainer();
            
            return hero;
        }
    }
}