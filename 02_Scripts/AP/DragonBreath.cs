using System.Collections;
using Ami.BroAudio;
using DG.Tweening;
using UnityEngine;
using UnityEngine.InputSystem;
using Work.Chipmunk._01.Scripts.Heros;
using Work.CHUH._01Scripts.Entities;

public class DragonBreath : Skill
{
    [Header("브레스")]
    [SerializeField] private GameObject _dragonBreathPrefab;

    [Header("범위")] 
    [SerializeField] private GameObject _magicCircle;
    private GameObject _circleInstance;
    public float _radius = 5f;

    private bool _isTargeting;
    private SelectPanel _selectPanel;
    private Vector2 _targetPosition;
    private Hero _castHero;

    protected override void Awake()
    {
        base.Awake();
        if (_selectPanel == null) _selectPanel = SelectPanel.Instance;
        _castHero = GetComponentInParent<Hero>();
    }

    public override void CallSkill()
    {
        base.CallSkill();
        EnterTargetingMode();
    }

    private void Update()
    {
        if (!_isTargeting) return;

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

    private IEnumerator CastSkillSequence(Vector2 pos)
    {
        GameObject fireObj = Instantiate(_dragonBreathPrefab, new Vector3(pos.x, pos.y, 0f), Quaternion.Euler(0, 180, 0));
        DragonFire dragonFire = fireObj.GetComponent<DragonFire>();
        if (dragonFire != null)
        {
            dragonFire.Initialize(ComponentContainer.Get<EntityStat>(), _castHero);
            dragonFire.DragonBreath();
        }

        yield break;
    }

    private void EnterTargetingMode()
    {
        if (_isOnCooldown) return;
        if (_isTargeting) return;
        
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
