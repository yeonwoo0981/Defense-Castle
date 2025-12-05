using System;
using System.Collections;
using Ami.BroAudio;
using Chipmunk.GameEvents;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using Work.Chipmunk._01.Scripts.Heros;
using Work.CHUH._01Scripts.Enemies.Wave.GameEvents;
using Work.CHUH._01Scripts.Entities;

public class WindStorm : Skill
{
    [Header("바람")]
    [SerializeField] private GameObject _windStormPrefab;

    [Header("범위")] 
    [SerializeField] private GameObject _magicCircle;
    private GameObject _circleInstance;
    public float _radius = 3f;
    
    private bool _isTargeting;
    private SelectPanel _selectPanel;
    private Vector2 _targetPosition;

    private Hero _castHero;

    protected override void Awake()
    {
        if (_selectPanel == null)
            _selectPanel = SelectPanel.Instance;

        _castHero = GetComponentInParent<Hero>();
    }
    
    public override void CallSkill()
    {
        EnterTargetingMode();
    }
    
    public void EnterTargetingMode()
    {
        if (_isOnCooldown) return;
            
        if (!_isTargeting) EnterTargeting();
    }
    private void Update()
    {
        if (_isTargeting == true)
        {
            Vector2 mousePos = Mouse.current.position.ReadValue();
            Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
            _targetPosition = new Vector2(worldPos.x, worldPos.y);
                
            if (_circleInstance != null)
            {
                _circleInstance.transform.position = new Vector3(_targetPosition.x, _targetPosition.y, 0f);
                _circleInstance.transform.localScale = new Vector3(_radius * 2, _radius * 2, 1f); 
            }
           
            if (Mouse.current.leftButton.wasPressedThisFrame)
            {
                BroAudio.Play(sound);
                ExitTargeting();
                StartCoroutine(CastSkillSequence(_targetPosition));
            }
        }
    }

    private IEnumerator CastSkillSequence(Vector2 pos)
    {
        GameObject stormObj = Instantiate(_windStormPrefab, new Vector3(pos.x, pos.y, 0), Quaternion.identity);
        
        InStorm inStorm = stormObj.GetComponent<InStorm>();
        if (inStorm != null)
        {
            inStorm.Initialize(ComponentContainer.Get<EntityStat>(), _castHero);
            inStorm.StartWindStorm();
        }
        yield break;
    }
    
    private void EnterTargeting()
    {
        _isTargeting = true;
        _selectPanel.Show();
        Time.timeScale = 0f;
            
        Vector2 mousePos = Mouse.current.position.ReadValue();
        Vector3 worldPos = Camera.main.ScreenToWorldPoint(mousePos);
        _targetPosition = new Vector2(worldPos.x, worldPos.y);
            
        if (_magicCircle != null)
            _circleInstance = Instantiate(_magicCircle, _targetPosition, Quaternion.identity);
    }
    
    private void ExitTargeting()
    {
        StartCooldown();
        _selectPanel.Hide();
        _isTargeting = false; 
        Time.timeScale = 1f;
        if (_circleInstance != null)
            Destroy(_circleInstance);
    }
}
