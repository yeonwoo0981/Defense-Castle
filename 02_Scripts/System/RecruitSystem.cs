using System;
using Chipmunk.GameEvents;
using UnityEngine;
using Work.CHUH._01Scripts.Enemies.Wave.GameEvents;

public class RecruitSystem : MonoBehaviour
{
   private void Awake()
   {
      EventBus<WaveStartEvent>.OnEvent += WaveStartEvent;
      EventBus<WaveEndEvent>.OnEvent += WaveEndEvent;
   }
   
   private void WaveStartEvent(WaveStartEvent evt)
   {
      gameObject.SetActive(false);
   }
   private void WaveEndEvent(WaveEndEvent evt)
   {
      gameObject.SetActive(true);
   }
}
