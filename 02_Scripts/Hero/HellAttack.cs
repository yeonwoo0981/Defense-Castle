using UnityEngine;
using Work.Chipmunk._01.Scripts.Combat;
using Work.CHUH._01Scripts.Combat;

public class HellAttack : AttackComponent
{
    private float _radius = 2.5f;
    [SerializeField] private LayerMask _whatIsTarget;
    public override void Attack(EntityHealth target)
    {
        if (target == null) return;
        
        Collider2D[] collider2Ds = Physics2D.OverlapCircleAll(transform.position, _radius, _whatIsTarget);
        
        foreach (Collider2D c in collider2Ds)
        {
            EntityHealth entity = c.GetComponentInChildren<EntityHealth>();
            if (entity != null)
            { 
                entity.TakeDamage(DamageStat.Value);
                Debug.Log("범위 데미지를 입히는 다람쥐");
            }
        }
    }
}
