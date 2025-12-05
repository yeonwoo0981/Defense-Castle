using System;
using Ami.BroAudio;
using Chipmunk.GameEvents;
using ChipmunkKingdoms.Scripts.Utility;
using DG.Tweening;
using UnityEngine;
using Work.CHUH._01Scripts.Enemies.Wave.GameEvents;

public abstract class Skill : MonoBehaviour, IContainerComponent
{
    public Sprite _skillIcon;
    public float _coolTime;
    public bool _isOnCooldown;

    [SerializeField] protected SoundID sound;
    private Tween _cooldownTween;

    public event Action OnCooldownStart;
    public event Action OnCooldownEnd;

    protected virtual void Awake()
    {
        EventBus<WaveEndEvent>.OnEvent += WaveEndEvent;
    }

    private void WaveEndEvent(WaveEndEvent evt)
    {
        CancelCooldown();
    }

    public ComponentContainer ComponentContainer { get; set; }

    public virtual void OnInitialize(ComponentContainer componentContainer)
    {
    }

    public void TryCallSkill()
    {
        // if (!_isOnCooldown)
        {
            CallSkill();
            // StartCooldown();
        }
    }

    public virtual void CallSkill()
    {
        BroAudio.Play(sound);
    }

    protected void StartCooldown()
    {
        _isOnCooldown = true;
        _cooldownTween = DOVirtual.DelayedCall(_coolTime, EndCooldown);
        OnCooldownStart?.Invoke();
    }

    protected void EndCooldown()
    {
        _isOnCooldown = false;
        _cooldownTween = null;
        OnCooldownEnd?.Invoke();
    }

    protected void CancelCooldown()
    {
        if (_cooldownTween != null && _cooldownTween.IsActive())
        {
            _cooldownTween.Kill();
            _cooldownTween = null;
        }

        _isOnCooldown = false;
    }
}