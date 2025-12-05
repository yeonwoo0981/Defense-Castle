using System;
using UnityEngine;

public class SummonerDestroy : MonoBehaviour
{
    [SerializeField] private float _destroyTime;

    private void Start()
    {
        Destroy(gameObject, _destroyTime);
    }
}
