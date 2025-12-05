using Chuh007Lib.StatSystem;
using UnityEngine;
using Work.CHUH._01Scripts.Combat;

public class SecondThunder : FirstThunder
{
    [SerializeField] private float _extraDamagePercent = 12.5f;
    [SerializeField] private float _stunTime = 2.5f;

    protected override void TakeDamage(EntityHealth target)
    {
        if (_damageStat == null)
        {
            Debug.LogError("DamageStat이 초기화되지 않았습니다.");
            return;
        }

        float damage = _damageStat.Value * (1f + _extraDamagePercent / 100f);
        target.TakeDamage(damage);
    }

    public override void ThunderAttack()
    {
        base.ThunderAttack();
        
        Collider2D[] targets = Physics2D.OverlapCircleAll(transform.position, 3f, _whatIsTarget);

        foreach (Collider2D hits in targets)
        {
            IStunnable stunnable = hits.GetComponentInChildren<IStunnable>();
            if (stunnable != null)
            {
                stunnable.Stun(_stunTime);
                Debug.Log($"{hits.name}을(를) {_stunTime}초 동안 기절시켰습니다.");
            }
        }
    }
}