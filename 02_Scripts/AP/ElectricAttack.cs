using UnityEngine;
using Work.Chipmunk._01.Scripts.Combat;
using Work.CHUH._01Scripts.Combat;
using DG.Tweening;
using System.Collections;
using Chuh007Lib.StatSystem;

public class ElectricAttack : MeleeAttackComponent
{
   [Header("Effect")]
   [SerializeField] ParticleSystem _particleSystem;
   [SerializeField] private float _duration = 0.5f;

   [SerializeField] private float _stunTime = 0.5f;
   
   private Coroutine _vfxCoroutine;
   
   public override void Attack(EntityHealth target)
   {
      base.Attack(target);
      StunAttack(target);
      
      if (_vfxCoroutine != null)
      {
         StopCoroutine(_vfxCoroutine);
      }
      _vfxCoroutine = StartCoroutine(ElectricVFX(target));
   }

   private IEnumerator ElectricVFX(EntityHealth target)
   {
      _particleSystem.gameObject.SetActive(true);
      _particleSystem.transform.position = transform.position;

      yield return null;
      _particleSystem.transform.position = target.transform.position;

      yield return new WaitForSeconds(_duration);

      _particleSystem.gameObject.SetActive(false);

      _vfxCoroutine = null;
   }

   private void StunAttack(EntityHealth target)
   {
      IStunnable stunnable = target.GetComponent<IStunnable>() ??
                             target.GetComponentInParent<IStunnable>() ??
                             target.GetComponentInChildren<IStunnable>();

      if (stunnable != null)
      {
         stunnable.Stun(_stunTime);
         Debug.Log($"{_stunTime}초 동안 기절한 청설모");
      }
   }
}
