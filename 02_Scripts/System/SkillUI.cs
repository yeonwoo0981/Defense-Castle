using System;
using Chipmunk.GameEvents;
using ChipmunkKingdoms.Scripts.Utility;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Work.Chipmunk._01.Scripts.Heros;
using Work.CHUH._01Scripts.Enemies.Wave.GameEvents;
using Work.yeonwoo._02_Scripts.Heros;

public class SkillUI : MonoBehaviour
{
    [SerializeField] private HeroPlaceTile _targetTile;
    [SerializeField] private Image _cooldownMask;
    [SerializeField] private Image _skillIcon;
    [SerializeField] private TextMeshProUGUI _cooldownText;
    private Skill _mySkill;

    private float _cooldownStartTime;
    private bool _wasOnCooldown;

    private RectTransform _rectTransform;
    private Vector2 defaultPosition;
    [SerializeField] private float yOffset = -120f;

    private Hero _currentHero;

    private void Awake()
    {
        EventBus<WaveStartEvent>.OnEvent += WaveStartHandler;
        EventBus<WaveEndEvent>.OnEvent += WaveEndHandler;

        _cooldownMask.gameObject.SetActive(false);

        _rectTransform = transform.GetComponent<RectTransform>();
        defaultPosition = _rectTransform.anchoredPosition;
        _rectTransform.anchoredPosition = defaultPosition + Vector2.up * yOffset;
    }

    private void OnEnable()
    {
        if (_targetTile != null)
            _targetTile.OnPlaceHero += HeroPlaceHandler;
    }

    private void OnDisable()
    {
        if (_targetTile != null)
            _targetTile.OnPlaceHero -= HeroPlaceHandler;
        UnsubscribeSkillEvents();
    }

    private void UnsubscribeSkillEvents()
    {
        if (_mySkill != null)
        {
            _mySkill.OnCooldownStart -= OnSkillCooldownStart;
            _mySkill.OnCooldownEnd -= OnSkillCooldownEnd;
        }
    }

    private void SubscribeSkillEvents()
    {
        if (_mySkill != null)
        {
            _mySkill.OnCooldownStart += OnSkillCooldownStart;
            _mySkill.OnCooldownEnd += OnSkillCooldownEnd;
        }
    }

    private void HeroPlaceHandler(Hero hero)
    {
        UnsubscribeSkillEvents();
        _currentHero = hero;
        if (hero == null)
        {
            _mySkill = null;
            _cooldownMask.gameObject.SetActive(false);
            _skillIcon.enabled = (false);
            _cooldownText.text = "";
            _wasOnCooldown = false;
            return;
        }

        _mySkill = hero.GetContainerComponent<Skill>(true);
        SubscribeSkillEvents();
        _skillIcon.sprite = _mySkill._skillIcon;
        _skillIcon.enabled = (true);
        UpdateSkillUI();
    }

    private void OnSkillCooldownStart()
    {
        _cooldownStartTime = Time.time;
        _wasOnCooldown = true;
        UpdateSkillUI();
    }

    private void OnSkillCooldownEnd()
    {
        _cooldownMask.fillAmount = 0f;
        _cooldownText.text = "";
        _wasOnCooldown = false;
    }

    private void UpdateSkillUI()
    {
        if (_mySkill == null) return;
        if (_mySkill._isOnCooldown)
        {
            float elapsed = Time.time - _cooldownStartTime;
            float remaining = Mathf.Max(_mySkill._coolTime - elapsed, 0f);
            _cooldownMask.fillAmount = 1 - remaining / _mySkill._coolTime;
            _cooldownText.text = remaining.ToString("F1");
        }
        else
        {
            _cooldownMask.fillAmount = 1f;
            _cooldownText.text = "";
            _wasOnCooldown = false;
        }
    }

    private void WaveStartHandler(WaveStartEvent evt)
    {
        _cooldownStartTime = Time.time;
        _wasOnCooldown = false;
        _cooldownMask.fillAmount = 1f;
        _cooldownMask.gameObject.SetActive(true);
        _cooldownText.text = "";
        _rectTransform.DOAnchorPosY(defaultPosition.y, 0.8f).SetEase(Ease.InOutSine).SetUpdate(true)
            .OnComplete(() => gameObject.SetActive(true));
    }

    private void WaveEndHandler(WaveEndEvent evt)
    {
        _rectTransform.DOAnchorPosY(defaultPosition.y + yOffset, 0.8f).SetEase(Ease.InOutSine).SetUpdate(true);
    }

    private void LateUpdate()
    {
        if (_mySkill != null && _mySkill._isOnCooldown)
        {
            UpdateSkillUI();
            return;
        }

        // 파괴된 타일 또는 Hero가 없는 경우만 체크
        if (_targetTile == null || _currentHero == null || _mySkill == null)
        {
            HeroPlaceHandler(null);
        }
    }

    public void CallSkill()
    {
        _targetTile?.CallHeroSkill();
    }
}