using System.Collections.Generic;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using Work.CHUH._01Scripts.Combat;
using Projectile = Work.Chipmunk._01.Scripts.Combat.Projectile;

public class ElfArrow : Projectile
{ 
    private HashSet<Collider2D> _hits = new HashSet<Collider2D>();
    
    protected override void TryAttack()
    {
        Collider2D hit = Physics2D.OverlapBox(transform.position,
            new Vector2(size.x * transform.localScale.x, size.y * transform.localScale.y), 0f, whatIsTarget);
        
        if (hit != null)
        {
            EntityHealth target = hit.GetComponentInChildren<EntityHealth>();
            if (target != null && _hits.Contains(hit))
            {
                Debug.Log("이미 엘프 화살에 적중한 적입니다, 데미지가 들어가지 않습니다.");
                return;
            }
            target.TakeDamage(DamageStat.Value);
            _hits.Add(hit);
        }
    }
}
