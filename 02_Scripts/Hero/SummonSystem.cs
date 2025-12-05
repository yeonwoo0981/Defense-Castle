using System;
using Chipmunk.GameEvents;
using UnityEngine;
using Work.CHUH._01Scripts.Enemies.Wave.GameEvents;

public class SummonSystem : MonoBehaviour
{
    [SerializeField] private float _destrTime = 10f;

    private void Update()
    {
        Destroy(gameObject, _destrTime);
    }
}