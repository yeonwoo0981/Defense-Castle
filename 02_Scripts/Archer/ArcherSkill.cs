using Ami.BroAudio;
using ChipmunkKingdoms.Scripts.Utility;
using Chuh007Lib.StatSystem;
using DG.Tweening;
using UnityEngine;
using Work.CHUH._01Scripts.Entities;

public class ArcherSkill : Skill, IAfterInitailze
{
    [SerializeField] private StatSO attackSpeed;
    
    public void AfterInitailized()
    {
        attackSpeed = this.Get<EntityStat>().GetStat(attackSpeed);
    }    
    public override void CallSkill()
    {
        base.CallSkill();
        if (_isOnCooldown)
        {
            Debug.Log("Skill is on cooldown");
            return;
        }
        Debug.Log("Speed");
        attackSpeed.AddValueModifier(this, 3f);
        StartCooldown();
        DOVirtual.DelayedCall(3, (() => { attackSpeed.RemoveModifier(this); }));
    }
}
