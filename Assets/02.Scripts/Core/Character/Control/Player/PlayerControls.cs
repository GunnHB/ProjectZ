using System.Collections;
using UnityEngine;

using Cinemachine;
using Sirenix.OdinInspector;

using ProjectZ.Item;

namespace ProjectZ.Core.Characters
{
    public partial class PlayerControls : CharacterControls, IAttackable, IDamagable
    {
        private const string TITLE_COMPONENTS = "[Components]";

        [Title(TITLE_COMPONENTS)]
        // [SerializeField] private WeaponTester _weaponTester;
        [SerializeField] private CinemachineFreeLook _playerCam;

        // for movement
        private float _vertical;
        private float _horizontal;
        private float _moveAmount;
        private Vector3 _moveDirection;

        // properties
        public float Vertical => _vertical;
        public float Horizontal => _horizontal;
        public float MoveAmount => _moveAmount;

        // flags
        private bool _canMoveFlag;

        // flags property
        public bool CanMoveFlag
        {
            get => _canMoveFlag;
            set
            {
                _canMoveFlag = value;

                if (!value)
                    ResetMovementValuesAction?.Invoke();
            }
        }
        public bool PerformingActionFlag { get; set; }

        // stat
        public PlayerStats ThisPlayerStats => _stats as PlayerStats;

        // coroutine
        private Coroutine _exhaustCoroutine;

        protected override void Awake()
        {
            base.Awake();

            InitInputs();
            InitAnimData();
            InitStates();
            InitAllEvents();
            InitAttackDatas();
        }

        protected override void Update()
        {
            base.Update();

            _stateMachine.DoOperatorUpdate();

            _movement.MovementAction?.Invoke(_moveDirection);
        }

        private void InitAnimData()
        {
            _animData = new Data.PlayerAnimaionData();

            _animData.InitializeData();
        }

        protected override void OnEnable()
        {
            base.OnEnable();

            RegistAllInputs();
            RegistAllFSMActions();
        }

        protected override void OnDisable()
        {
            base.OnDisable();

            UnregistAllInputs();
            UnregistAllFSMActions();
        }

        // 각종 이벤트의 콜백 등록 (Action, Func, ...) 
        private void InitAllEvents()
        {
            // 중복 등록 방지...
            ThisPlayerStats.OnExhaustedEvent -= OnExhaustedCallback;
            ResetMovementValuesAction -= ResetMovementValuesCallback;
            ResetAttackIndexAction -= () => { _attackIndex = 0; };

            ThisPlayerStats.OnExhaustedEvent += OnExhaustedCallback;
            ResetMovementValuesAction += ResetMovementValuesCallback;
            ResetAttackIndexAction += () => { _attackIndex = 0; };
        }

        #region Stats
        private void OnExhaustedCallback()
        {
            RegistCoroutine();
            CancelSprint();
        }

        private void RegistCoroutine()
        {
            if (_exhaustCoroutine != null)
            {
                StopCoroutine(_exhaustCoroutine);
                _exhaustCoroutine = null;
            }

            StartCoroutine(nameof(Cor_Exhaust));
        }

        private IEnumerator Cor_Exhaust()
        {
            float currTime = 0f;

            while (currTime < Manager.GameValue.STAMINA_DELAY_TIME)
            {
                currTime += Time.deltaTime / Manager.GameValue.STAMINA_DELAY_TIME;
                ActiveOperateChargeStamina(false);

                yield return null;
            }

            ActiveOperateChargeStamina(true);
        }

        private void ActiveOperateChargeStamina(bool active)
        {
            ThisPlayerStats.OperateChargeStamina = active;

            // 충전 불가능한 상태에서는 속도가 느려짐
            DoSetMovementSpeed(!active ? MovementType.Exhaust : MovementType.Normal);
        }
        #endregion

        #region Attack
        public void AttackTarget(CharacterControls controls)
        {

        }

        public void InitAttackDatas()
        {
            _lightAttackDataList = new()
            {
                new AttackData(_animData.AnimNameLightAttack01, .45f),
                new AttackData(_animData.AnimNameLightAttack02, .5f),
            };

            _heavyAttackDataList = new();
        }

        public override void AnimEventHitCheakStart()
        {
            base.AnimEventHitCheakStart();

            // if (_weaponTester == null)
            //     return;

            // _weaponTester.HitCollider.enabled = true;
        }

        public override void AnimEventHitCheakFinish()
        {
            base.AnimEventHitCheakFinish();

            // if (_weaponTester == null)
            //     return;

            // _weaponTester.HitCollider.enabled = false;
        }
        #endregion

        #region Damage
        public void TakeDamage(float damageAmount)
        {
            Debug.Log(damageAmount);
        }
        #endregion
    }
}
