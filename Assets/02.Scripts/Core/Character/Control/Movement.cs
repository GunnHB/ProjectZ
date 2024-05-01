using UnityEngine;
using UnityEngine.Events;

using System.Collections.Generic;

using Sirenix.OdinInspector;
using System;

namespace ProjectZ.Core.Characters
{
    public enum MovementType
    {
        None = -1,
        Normal,
        Sprint,
        Exhaust,
    }

    public class Movement : MonoBehaviour
    {
        private const string TITLE_GROUND_CHECK = "[Ground Check]";
        private const string TITLE_DEBUG = "[DEBUG]";

        [Title(TITLE_GROUND_CHECK)]
        [SerializeField] private Transform _groundTransform;
        [SerializeField] private Vector3 _boxSize;
        [SerializeField] private float _maxDistance;
        [SerializeField] private LayerMask _groundMask;

        [Title(TITLE_DEBUG)]
        [SerializeField] private bool _drawGizmo;

        // components
        private CharacterController _controller;

        // veriables
        private float _movementSpeed;
        private float _turnSmoothVelocity;
        private float _turnSmoothTime = .1f;

        // gravity
        private float _gravity = -9.81f;
        private float _jumpForce = 1.5f;
        private Vector3 _gravityVelocity;

        // flags
        private bool _wasPeaked = false;

        // actions
        public UnityAction<Vector3> MovementAction;
        public UnityAction JumpAction;

        // func
        public Func<bool> GroundCheckFunc;
        public Func<bool> PeakCheckFunc;

        // dictionary
        private Dictionary<MovementType, float> _speedDict;

        // properties
        private bool IsPlayer => TryGetComponent(out PlayerControls player);

        private void Awake()
        {
            _controller = GetComponent<CharacterController>();

            InitSpeed();
        }

        private void OnEnable()
        {
            MovementAction += MovementCallback;
            JumpAction += JumpCallback;
            GroundCheckFunc += IsGround;
            PeakCheckFunc += IsPeak;

            if (TryGetComponent(out CharacterControls controls))
                controls.ThisStats.OnDeathEvent.AddListener(OnDeathCallback);
        }

        private void OnDisable()
        {
            MovementAction -= MovementCallback;
            JumpAction -= JumpCallback;
            GroundCheckFunc -= IsGround;
            PeakCheckFunc -= IsPeak;

            if (TryGetComponent(out CharacterControls controls))
                controls.ThisStats.OnDeathEvent.RemoveListener(OnDeathCallback);
        }

        private void InitSpeed()
        {
            _speedDict = new()
            {
                {MovementType.Normal, 4f},
                {MovementType.Sprint, 6f},
                {MovementType.Exhaust, 2f},
            };

            _movementSpeed = _speedDict[MovementType.Normal];
        }

        public void MovementCallback(Vector3 moveDirection)
        {
            // 중력은 항상 적용됨
            ApplyGravity();

            if (moveDirection == Vector3.zero || moveDirection.magnitude < .1f)
                return;

            float targetAngle = GetTargetAngle(moveDirection);
            float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref _turnSmoothVelocity, _turnSmoothTime);

            transform.rotation = Quaternion.Euler(new Vector3(0f, angle, 0f));

            Vector3 targetDir;

            if (IsPlayer)
                targetDir = Quaternion.Euler(new Vector3(0f, targetAngle, 0f)) * Vector3.forward;
            else
                targetDir = (transform.position - moveDirection).normalized;

            _controller.Move(targetDir * _movementSpeed * Time.deltaTime);
        }

        private float GetTargetAngle(Vector3 direction)
        {
            float camAngle = IsPlayer ? Camera.main.transform.eulerAngles.y : 0f;
            Vector3 tempDir = IsPlayer ? direction : (direction - transform.position).normalized;

            return Mathf.Atan2(tempDir.x, tempDir.z) * Mathf.Rad2Deg + camAngle;
        }

        public void SetMovementSpeed(MovementType type)
        {
            _movementSpeed = _speedDict[type];
        }

        private void ApplyGravity()
        {
            _gravityVelocity.y += _gravity * Time.deltaTime;

            if (IsGround() && _gravityVelocity.y < 0f)
                _gravityVelocity.y = Mathf.Max(_gravityVelocity.y, -2f);

            _controller.Move(_gravityVelocity * Time.deltaTime);
        }

        private bool IsGround()
        {
            if (_groundTransform == null)
                return false;

            return Physics.BoxCast(_groundTransform.position, _boxSize, -transform.up, transform.rotation, _maxDistance, _groundMask);
        }

        // ground check gizmo
        private void OnDrawGizmos()
        {
            if (!_drawGizmo)
                return;

            Gizmos.color = IsGround() ? Color.red : Color.blue;
            Gizmos.DrawCube(_groundTransform.position - transform.up * _maxDistance, _boxSize);
        }

        // about jump
        private void JumpCallback()
        {
            if (IsGround())
                _gravityVelocity.y = Mathf.Sqrt(_jumpForce * -2f * _gravity);
        }

        private bool IsPeak()
        {
            PrevCheckPeak();

            if (_gravityVelocity.sqrMagnitude <= .1f && !_wasPeaked)
            {
                _wasPeaked = true;
                return true;
            }
            else
                return false;
        }

        private void PrevCheckPeak()
        {
            if (IsGround())
                _wasPeaked = false;
        }

        private void OnDeathCallback()
        {
            _controller.enabled = false;
        }
    }
}
