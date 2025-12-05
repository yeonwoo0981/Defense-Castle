using System;
using Chipmunk.GameEvents;
using UnityEngine;
using Work.CHUH._01Scripts.Enemies.Wave.GameEvents;

public class RotationSystem : MonoBehaviour
{
   private void Awake()
   {
      EventBus<WaveEndEvent>.OnEvent += WaveEndEvent;
   }

   private void WaveEndEvent(WaveEndEvent evt)
   {
      gameObject.transform.rotation = Quaternion.Euler(0, 180, 0);
      EventBus<WaveEndEvent>.OnEvent -= WaveEndEvent;
   }
}
