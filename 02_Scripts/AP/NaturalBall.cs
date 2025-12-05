using UnityEngine;
using Work.CHUH._01Scripts.Combat;
using Projectile = Work.Chipmunk._01.Scripts.Combat.Projectile;

public class NaturalBall : Projectile
{
    public override void Update()
    {
        base.TryAttack();
        base.Move();
    }
}
