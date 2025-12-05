using System.Collections;
using Ami.BroAudio;
using Chuh007Lib.StatSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Work.CHUH._01Scripts.Entities;

public class ElfSkill : Skill
{
    [Header("스킬")]
    [SerializeField] private GameObject _flatePrefab;

    [Header("범위")]
    [SerializeField] private GameObject _magicCircle;
    private GameObject _circleInstance;
    public float _radius = 3f;

    private bool _isTargeting;
    private SelectPanel _selectPanel;
    private Vector2 _targetPosition;

    [Header("스탯")]
    [SerializeField] private StatSO _damageStat;

    protected override void Awake()
    {
        base.Awake();
        if (_selectPanel == null)
            _selectPanel = SelectPanel.Instance;
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
        GameObject flateObj = Instantiate(_flatePrefab, new Vector3(pos.x, pos.y, 0), Quaternion.identity);

        ElfSkillHit hitComp = flateObj.GetComponent<ElfSkillHit>();
        if (hitComp != null)
        {
            hitComp.Initialize(ComponentContainer.Get<EntityStat>(), _damageStat);
            hitComp.ElfHit();
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
