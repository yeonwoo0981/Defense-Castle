using System.Collections;
using Ami.BroAudio;
using Chuh007Lib.StatSystem;
using UnityEngine;
using UnityEngine.InputSystem;
using Work.CHUH._01Scripts.Entities;

namespace Work.yeonwoo._02_Scripts.AP
{
    public class EMagicCircle : Skill
    {
        [Header("천둥")]
        [SerializeField] private GameObject _firstThunder;
        [SerializeField] private GameObject _secondThunder;

        [Header("범위")]
        [SerializeField] private GameObject _magicCircle;
        private GameObject _circleInstance;
        public float _radius = 2f;

        private bool _isTargeting;
        private SelectPanel _selectPanel;
        private Vector2 _targetPosition;

        [Header("스탯")]
        [SerializeField] protected StatSO _damageStat;

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
            GameObject thunder1 = Instantiate(_firstThunder, new Vector3(pos.x, pos.y, 0f), Quaternion.identity);
            FirstThunder thunderComp1 = thunder1.GetComponent<FirstThunder>();
            if (thunderComp1 != null)
            {
                thunderComp1.Initialize(ComponentContainer.Get<EntityStat>(), _damageStat);
                thunderComp1.ThunderAttack();
            }

            yield return new WaitForSecondsRealtime(0.45f);
            
            GameObject thunder2 = Instantiate(_secondThunder, new Vector3(pos.x, pos.y, 0f), Quaternion.identity);
            FirstThunder thunderComp2 = thunder2.GetComponent<FirstThunder>();
            if (thunderComp2 != null)
            {
                thunderComp2.Initialize(ComponentContainer.Get<EntityStat>(), _damageStat);
                thunderComp2.ThunderAttack();
            }
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
            _isTargeting = false;
            _selectPanel.Hide();
            Time.timeScale = 1f;
            if (_circleInstance != null)
                Destroy(_circleInstance);
        }
    }
}
