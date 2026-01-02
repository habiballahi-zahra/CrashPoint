
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

    [DefaultExecutionOrder(-1)]
    public class PlayerController : MonoBehaviour
    {
        #region Class Variables
        [Header("Components")]
        [SerializeField] private CharacterController _characterController;
        [SerializeField] private Camera _playerCamera;
        public float RotationMismatch { get; private set; } = 0f;
        public bool IsRotatingToTarget { get; private set; } = false;

        [Header("Base Movement")]
        public float walkAcceleration = 25f;
        public float walkSpeed = 2f;
        public float runAcceleration = 35f;
        public float runSpeed = 4f;
        public float sprintAcceleration = 50f;
        public float sprintSpeed = 7f;
        public float inAirAcceleration = 25f;
        public float drag = 20f;
        public float inAirDrag=5f;
        public float gravity = 25f;
        public float terminalVelocity = 50f;
        public float jumpSpeed = 1.0f;
        public float movingThreshold = 0.01f;


        [Header("Jump Settings")]
        [SerializeField] private float baseJumpHeight = 1.2f;          // ارتفاع پایه‌ی پرش در حالت ایستاده
        [SerializeField] private float runJumpMultiplier = 1.1f;       // ضریب پرش هنگام دویدن
        [SerializeField] private float sprintJumpMultiplier = 1.25f;   // ضریب پرش هنگام اسپرینت
        [SerializeField] private float antiBumpForce = 2f;             // نیروی کوچک برای چسباندن پلیر به زمین




        [Header("Animation")]
        public float playerModelRotationSpeed = 10f;
        public float rotateToTargetTime = 0.67f;

        [Header("Camera Settings")]
        public float lookSenseH = 0.1f;
        public float lookSenseV = 0.1f;
        public float lookLimitV = 89f;
        public float rotationTolerance=90f;

        [Header("Environment Details")]
        [SerializeField] private LayerMask _groundLayers;

        private PlayerLocomotions _playerLocomotionInput;
        private PlayerState _playerState;

        private Vector2 _cameraRotation = Vector2.zero;
        private Vector2 _playerTargetRotation = Vector2.zero;

        private bool _jumpedLastFrame = false;
        private bool _isRotatingClockwise = false;
        private float _rotatingToTargetTimer = 0f;
        private float _verticalVelocity = 0f;
        private float _antiBump;
        private float _stepOffset;

        private PlayerMovementState _lastMovementState = PlayerMovementState.Falling;
        #endregion

        #region Startup
        private void Awake()
        {
            _playerLocomotionInput = GetComponent<PlayerLocomotions>(); // گرفتن ورودی‌های حرکت پلیر
            _playerState = GetComponent<PlayerState>();                 // گرفتن وضعیت فعلی پلیر (Idle, Run, Jump, ...)

            _antiBump = antiBumpForce;                                  // antiBump مستقل از سرعت حرکت تنظیم می‌شود
            _stepOffset = _characterController.stepOffset;              // ذخیره مقدار پیش‌فرض stepOffset
        }

        #endregion

        #region Update Logic
        private void Update()
        {
            UpdateMovementState();
            HandleVerticalMovement();
            HandleLateralMovement();
        }

        private void UpdateMovementState()
        {
            _lastMovementState = _playerState.CurrentPlayerMovementState;

            bool canRun = CanRun();
            bool isMovementInput = _playerLocomotionInput.MovementControls != Vector2.zero;             //order
            bool isMovingLaterally = IsMovingLaterally();                                            //matters
            bool isSprinting = _playerLocomotionInput.SprintToggledOn && isMovingLaterally;          //order
            bool isWalking = isMovingLaterally && (!canRun || _playerLocomotionInput.WalkToggledOn); //matters
            bool isGrounded = IsGrounded();

            PlayerMovementState lateralState = isWalking ? PlayerMovementState.Walking :
                                               isSprinting ? PlayerMovementState.Sprinting :
                                               isMovingLaterally || isMovementInput ? PlayerMovementState.Running : PlayerMovementState.Idling;

            _playerState.SetPlayerMovementState(lateralState);

            // Control Airborn State
            if ((!isGrounded || _jumpedLastFrame) && _characterController.velocity.y > 0f)
            {
                _playerState.SetPlayerMovementState(PlayerMovementState.Jumping);
                _jumpedLastFrame = false;
                _characterController.stepOffset = 0f;
            }
            else if ((!isGrounded || _jumpedLastFrame) && _characterController.velocity.y <= 0f)
            {
                _playerState.SetPlayerMovementState(PlayerMovementState.Falling);
                _jumpedLastFrame = false;
                _characterController.stepOffset = 0f;
            }
            else
            {
                _characterController.stepOffset = _stepOffset;
            }
        }

        private void HandleVerticalMovement()
        {
             bool isGrounded = _playerState.InGroundedState(); // بررسی اینکه پلیر روی زمین هست یا نه

    // اعمال گرانش به صورت پیوسته
    _verticalVelocity -= gravity * Time.deltaTime;

    // اگر روی زمین هستیم و سرعت عمودی منفی شد
    // مقدار کمی نیروی رو به پایین میدیم تا به زمین بچسبه
    if (isGrounded && _verticalVelocity < 0f)
    {
        _verticalVelocity = -antiBumpForce;
    }

    // اگر دکمه پرش زده شده و پلیر روی زمین است
    if (_playerLocomotionInput.JumpPressed && isGrounded)
    {
        float jumpMultiplier = GetJumpMultiplier(); 
        // تعیین ضریب پرش بر اساس حالت حرکت (ایستاده، دویدن، اسپرینت)

        float jumpForce = Mathf.Sqrt(baseJumpHeight * jumpMultiplier * 2f * gravity);
        // محاسبه نیروی پرش بر اساس فرمول فیزیکی

        _verticalVelocity = jumpForce; 
        // مقدار سرعت عمودی مستقیماً تنظیم می‌شود (نه +=)
        // تا پرش فقط یک بار و دقیق اعمال شود

        _jumpedLastFrame = true; 
        // علامت می‌زنیم که فریم قبل پرش انجام شده
    }

    // محدود کردن سرعت سقوط به terminal velocity
    if (Mathf.Abs(_verticalVelocity) > Mathf.Abs(terminalVelocity))
    {
        _verticalVelocity = -Mathf.Abs(terminalVelocity);
    }
            
        }

        private void HandleLateralMovement()
        {
            // Create quick references for current state
            bool isSprinting = _playerState.CurrentPlayerMovementState == PlayerMovementState.Sprinting;
            bool isGrounded = _playerState.InGroundedState();
            bool isWalking = _playerState.CurrentPlayerMovementState == PlayerMovementState.Walking;

            // State dependent acceleration and speed
            float lateralAcceleration = !isGrounded ? inAirAcceleration :
                                        isWalking ? walkAcceleration :
                                        isSprinting ? sprintAcceleration : runAcceleration;

            float clampLateralMagnitude = !isGrounded ? sprintSpeed :
                                          isWalking ? walkSpeed :
                                          isSprinting ? sprintSpeed : runSpeed;

            Vector3 cameraForwardXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
            Vector3 cameraRightXZ = new Vector3(_playerCamera.transform.right.x, 0f, _playerCamera.transform.right.z).normalized;
            Vector3 movementDirection = cameraRightXZ * _playerLocomotionInput.MovementControls.x + cameraForwardXZ * _playerLocomotionInput.MovementControls.y;

            Vector3 movementDelta = movementDirection * lateralAcceleration * Time.deltaTime;
            Vector3 newVelocity = _characterController.velocity + movementDelta;

            // Add drag to player

            float dragMagnitude = isGrounded ? drag : inAirDrag;
            Vector3 currentDrag = newVelocity.normalized * dragMagnitude * Time.deltaTime;
            newVelocity = (newVelocity.magnitude > drag * Time.deltaTime) ? newVelocity - currentDrag : Vector3.zero;
            newVelocity = Vector3.ClampMagnitude(new Vector3(newVelocity.x, 0f, newVelocity.z), clampLateralMagnitude);
            newVelocity.y += _verticalVelocity;
            newVelocity = !isGrounded ? HandleSteepWalls(newVelocity) : newVelocity;
            // Debug.Log(newVelocity);
            // Move character (Unity suggests only calling this once per tick)
            _characterController.Move(newVelocity * Time.deltaTime);
        }


        private float GetJumpMultiplier()
        {
            // بررسی وضعیت حرکتی پلیر
            switch (_playerState.CurrentPlayerMovementState)
            {
                case PlayerMovementState.Sprinting:
                    return sprintJumpMultiplier; // بیشترین پرش در حالت اسپرینت

                case PlayerMovementState.Running:
                    return runJumpMultiplier;    // پرش متوسط در حالت دویدن

                default:
                    return 1f;                   // پرش پایه در حالت ایستاده یا راه رفتن
            }
        }



        private Vector3 HandleSteepWalls(Vector3 velocity)
        {
            Vector3 normal = CharacterControllerUtils.GetNormalWithSphereCast(_characterController, _groundLayers);
            float angle = Vector3.Angle(normal, Vector3.up);
            bool validAngle = angle <= _characterController.slopeLimit;

            if (!validAngle && _verticalVelocity < 0f)
                velocity = Vector3.ProjectOnPlane(velocity, normal);

            return velocity;
        }
        #endregion

        #region Late Update Logic
        private void LateUpdate()
        {
            UpdateCameraRotation();
        }

        private void UpdateCameraRotation()
        {
            _cameraRotation.x += lookSenseH * _playerLocomotionInput.LookInput.x;
            _cameraRotation.y = Mathf.Clamp(_cameraRotation.y - lookSenseV * _playerLocomotionInput.LookInput.y, -lookLimitV, lookLimitV);

            _playerTargetRotation.x += transform.eulerAngles.x + lookSenseH * _playerLocomotionInput.LookInput.x;

            // rotationTolerance = 90f;
            bool isIdling = _playerState.CurrentPlayerMovementState == PlayerMovementState.Idling;
            IsRotatingToTarget = _rotatingToTargetTimer > 0;

            // ROTATE if we're not idling
            if (!isIdling)
            {
                RotatePlayerToTarget();
            }
            // If rotation mismatch not within tolerance, or rotate to target is active, ROTATE
            else if (Mathf.Abs(RotationMismatch) > rotationTolerance || IsRotatingToTarget)
            {
                UpdateIdleRotation(rotationTolerance);
            }

            _playerCamera.transform.rotation = Quaternion.Euler(_cameraRotation.y, _cameraRotation.x, 0f);

            // Get angle between camera and player
            Vector3 camForwardProjectedXZ = new Vector3(_playerCamera.transform.forward.x, 0f, _playerCamera.transform.forward.z).normalized;
            Vector3 crossProduct = Vector3.Cross(transform.forward, camForwardProjectedXZ);
            float sign = Mathf.Sign(Vector3.Dot(crossProduct, transform.up));
            RotationMismatch = sign * Vector3.Angle(transform.forward, camForwardProjectedXZ);
        }

        private void UpdateIdleRotation(float rotationTolerance)
        {
            // Initiate new rotation direction
            if (Mathf.Abs(RotationMismatch) > rotationTolerance)
            {
                _rotatingToTargetTimer = rotateToTargetTime;
                _isRotatingClockwise = RotationMismatch > rotationTolerance;
            }
            _rotatingToTargetTimer -= Time.deltaTime;

            // Rotate player
            if (_isRotatingClockwise && RotationMismatch > 0f ||
                !_isRotatingClockwise && RotationMismatch < 0f)
            {
                RotatePlayerToTarget();
            }
        }

        private void RotatePlayerToTarget()
        {
            Quaternion targetRotationX = Quaternion.Euler(0f, _playerTargetRotation.x, 0f);
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotationX, playerModelRotationSpeed * Time.deltaTime);
        }
        #endregion

        #region State Checks
        private bool IsMovingLaterally()
        {
            Vector3 lateralVelocity = new Vector3(_characterController.velocity.x, 0f, _characterController.velocity.z);

            return lateralVelocity.magnitude > movingThreshold;
        }

        private bool IsGrounded()
        {
            bool grounded = _playerState.InGroundedState() ? IsGroundedWhileGrounded() : IsGroundedWhileAirborne();

            return grounded;
        }

        private bool IsGroundedWhileGrounded()
        {
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - _characterController.radius, transform.position.z);

            bool grounded = Physics.CheckSphere(spherePosition, _characterController.radius, _groundLayers, QueryTriggerInteraction.Ignore);

            return grounded;
        }

        private bool IsGroundedWhileAirborne()
        {
            Vector3 normal = CharacterControllerUtils.GetNormalWithSphereCast(_characterController, _groundLayers);
            float angle = Vector3.Angle(normal, Vector3.up);
            print(angle);
            bool validAngle = angle <= _characterController.slopeLimit;

            return _characterController.isGrounded && validAngle;
        }

        private bool CanRun()
        {
            // This means player is moving diagonally at 45 degrees or forward, if so, we can run
            return _playerLocomotionInput.MovementControls.y >= Mathf.Abs(_playerLocomotionInput.MovementControls.x);
        }
        #endregion
    }

