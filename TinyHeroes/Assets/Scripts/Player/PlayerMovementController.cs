using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(PlayerAnimationController))]
[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerFXController))]
public class PlayerMovementController : MonoBehaviour
{
    #region Variables

    [Header("References")]
    [SerializeField] private PlayerMovementStatsSO _movementStats;
    [SerializeField] private CapsuleCollider2D _bodyColl;
    [SerializeField] private BoxCollider2D _feetColl;
    [SerializeField] private CameraPlayerFollow _cameraFollow;

    // Movement variables
    [HideInInspector] public bool isFacingRight;
    private Vector2 _moveVelocity;
    private bool _isRunning;

    // Jumping variables
    private float _verticalVeocity;
    private float _fastFallTime;
    private float _fastFallReleaseSpeed;
    private float _inAirTimer;
    private bool _isJumping;
    private bool _isFalling;
    private bool _isFastFalling;
    private int _numberOfJumpsUsed;

    // Jump apex variables
    private float _apexPoint;
    private float _timePastApexThreshold;
    private bool _isPastApexThreshold;

    // Jump buffer variables
    private float _jumpBufferTimer;
    private bool _jumpReleasedDuringBuffer;

    // Coyote time variables
    private float _coyoteTimer;

    // Collision check variables
    private RaycastHit2D _groundHit;
    private RaycastHit2D _headHit;
    private bool _isGrounded;
    private bool _bumpedHead;

    private Rigidbody2D _rigidbody;
    private PlayerAnimationController _animationController;
    private PlayerInputHandler _inputHandler;
    private PlayerFXController _fxController;

    #endregion

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        _animationController = GetComponent<PlayerAnimationController>();
        _inputHandler = GetComponent<PlayerInputHandler>();
        _fxController = GetComponent<PlayerFXController>();

        isFacingRight = true;
    }

    private void Update()
    {
        JumpChecks();
        UpdateTimers();

        if (_rigidbody.linearVelocityY < CameraManager.Instance.fallSpeedYDampingChangeThreshold && 
            !CameraManager.Instance.isLerpingYDamping && !CameraManager.Instance.lerpedFromPlayerFalling)
        {
            CameraManager.Instance.LerpYDamping(true);
        }
        
        if (_rigidbody.linearVelocityY >= 0f && 
            !CameraManager.Instance.isLerpingYDamping && CameraManager.Instance.lerpedFromPlayerFalling)
        {
            CameraManager.Instance.lerpedFromPlayerFalling = false;

            CameraManager.Instance.LerpYDamping(false);
        }
    }

    private void FixedUpdate()
    {
        CollisionChecks();
        Jump();

        if (_isGrounded)
        {
            Move(_movementStats.GroundAcceleration, _movementStats.GroundDecceleration, _inputHandler.Movement);
        }
        else
        {
            Move(_movementStats.AirAcceleration, _movementStats.AirDecceleration, _inputHandler.Movement);
        }
    }

    #region Movement

    private void Move(float acceleration, float decceleration, Vector2 moveInput)
    {
        if (moveInput != Vector2.zero)
        {
            TurnCheck(moveInput);

            Vector2 targetVelocity = Vector2.zero;
            if (_inputHandler.RunIsHeld)
            {
                targetVelocity = new Vector2(moveInput.x, 0f) * _movementStats.MaxRunSpeed;
                _animationController.AnimateMovement(true, true);
                if (!_isRunning)
                {
                    // Play dust fx
                    _fxController.PlayRunDust(new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y));
                    _isRunning = true;
                }
            }
            else
            {
                targetVelocity = new Vector2(moveInput.x, 0f) * _movementStats.MaxWalkSpeed;
                _animationController.AnimateMovement(true, false);
                _isRunning = false;
            }

            _moveVelocity = Vector2.Lerp(_moveVelocity, targetVelocity, acceleration * Time.fixedDeltaTime);
        }
        else
        {
            _moveVelocity = Vector2.Lerp(_moveVelocity, Vector2.zero, decceleration * Time.fixedDeltaTime);
            _animationController.AnimateMovement(false, false);
        }

        _rigidbody.linearVelocity = new Vector2(_moveVelocity.x, _rigidbody.linearVelocityY);
    }
    

    private void TurnCheck(Vector2 moveInput)
    {
        if (isFacingRight && moveInput.x < 0)
            Turn(false);
        else if (!isFacingRight && moveInput.x > 0)
            Turn(true);
    }

    private void Turn(bool turnRight)
    {
        if (turnRight)
        {
            transform.Rotate(0f, 180f, 0f);
            isFacingRight = true;
            _cameraFollow.Turn();
        }
        else
        {
            transform.Rotate(0f, -180f, 0f);
            isFacingRight = false;
            _cameraFollow.Turn();
        }
    }

    #endregion

    #region Jumping

    private void JumpChecks()
    {
        // Input
        if (_inputHandler.JumpWasPressed)
        {
            _jumpBufferTimer = _movementStats.JumpBufferTime;
            _jumpReleasedDuringBuffer = false;
            _animationController.InitiateJump();
        }

        if (_inputHandler.JumpWasReleased)
        {
            if (_jumpBufferTimer > 0f)
                _jumpReleasedDuringBuffer = true;

            if (_isJumping && _verticalVeocity > 0f)
            {
                if (_isPastApexThreshold)
                {
                    _isPastApexThreshold = false;
                    _isFastFalling = true;
                    _fastFallTime = _movementStats.TimeForUpwardsCancel;
                    _verticalVeocity = 0f;
                }
                else
                {
                    _isFastFalling = true;
                    _fastFallReleaseSpeed = _verticalVeocity;
                }
            }
        }

        // Normal ground / coyote jump
        if (_jumpBufferTimer > 0f && !_isJumping && (_isGrounded || _coyoteTimer > 0f))
        {
            InitiateJump(1);

            if (_jumpReleasedDuringBuffer)
            {
                _isFastFalling = true;
                _fastFallReleaseSpeed = _verticalVeocity;
            }
        }

        // Double jump
        else if (_jumpBufferTimer > 0f && _isJumping & _numberOfJumpsUsed < _movementStats.NumberOfJumpsAllowed - 1)
        {
            _isFastFalling = false;
            InitiateJump(1);
        }

        // Air jump after coyote time
        else if (_jumpBufferTimer > 0f && _isFalling && _numberOfJumpsUsed < _movementStats.NumberOfJumpsAllowed)
        {
            InitiateJump(2);
            _isFastFalling = false;
        }

        // Landing
        if ((_isJumping || _isFalling) && _isGrounded && _verticalVeocity <= 0f)
        {
            _isJumping = false;
            _isFalling = false;
            _isFastFalling = false;
            _isPastApexThreshold = false;
            _fastFallTime = 0f;
            _numberOfJumpsUsed = 0;

            _verticalVeocity = Physics2D.gravity.y;
        }
    }

    private void InitiateJump(int numberOfJumps)
    {
        if (!_isJumping)
            _isJumping = true;

        _jumpBufferTimer = 0f;
        _numberOfJumpsUsed += numberOfJumps;
        _verticalVeocity = _movementStats.InitialJumpVelocity;

        if (numberOfJumps >= 2)
            _fxController.PlayDoubleJumpDust(new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y));

    }

    private void Jump()
    {
        if (_isJumping)
        {
            if (_bumpedHead)
                _isFastFalling = true;
            
            if (_verticalVeocity >= 0f)
            {
                _apexPoint = Mathf.InverseLerp(_movementStats.InitialJumpVelocity, 0f, _verticalVeocity);

                if (_apexPoint > _movementStats.ApexThreshold)
                {
                    if (!_isPastApexThreshold)
                    {
                        _isPastApexThreshold = true;
                        _timePastApexThreshold = 0f;
                    }
                    if (_isPastApexThreshold)
                    {
                        _timePastApexThreshold += Time.fixedDeltaTime;
                        if (_timePastApexThreshold < _movementStats.ApexHangTime)
                            _verticalVeocity = 0f;
                        else
                            _verticalVeocity = -0.01f;
                    }
                }

                else
                {
                    _verticalVeocity += _movementStats.Gravity * Time.fixedDeltaTime;
                    if (_isPastApexThreshold)
                        _isPastApexThreshold = false;
                }
            }

            else if (!_isFastFalling)
            {
                _verticalVeocity += _movementStats.Gravity * _movementStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            }

            else if (_verticalVeocity < 0f)
            {
                if (!_isFalling)
                    _isFalling = true;
            }
        }

        if (_isFastFalling)
        {
            if (_fastFallTime >= _movementStats.TimeForUpwardsCancel)
                _verticalVeocity += _movementStats.Gravity * _movementStats.GravityOnReleaseMultiplier * Time.fixedDeltaTime;
            else
                _verticalVeocity = Mathf.Lerp(_fastFallReleaseSpeed, 0f, (_fastFallTime / _movementStats.TimeForUpwardsCancel));

            _fastFallTime += Time.fixedDeltaTime;
        }

        if (!_isGrounded && !_isJumping)
        {
            if (!_isFalling)
                _isFalling = true;

            _verticalVeocity += _movementStats.Gravity * Time.fixedDeltaTime;
        }

        _verticalVeocity = Mathf.Clamp(_verticalVeocity, -_movementStats.MaxFallSpeed, 50f);
        _rigidbody.linearVelocity = new Vector2(_rigidbody.linearVelocityX, _verticalVeocity);
    }

    #endregion

    #region Collision Checks

    private void CollisionChecks()
    {
        GroundCheck();
        BumpedHead();
    }

    private void GroundCheck()
    {
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _feetColl.bounds.min.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x, _movementStats.GroundDetectionRayLength);

        _groundHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.down, 
            _movementStats.GroundDetectionRayLength, _movementStats.GroundLayer);
        if (_groundHit.collider)
        {
            _isGrounded = true;
            if (_animationController.IsInAirOrJumping() && _inAirTimer > 0f)
            {
                _animationController.Land();
                _inAirTimer = 0f;
            }
        }
        else
            _isGrounded = false;
    }

    private void BumpedHead()
    {
        Vector2 boxCastOrigin = new Vector2(_feetColl.bounds.center.x, _bodyColl.bounds.max.y);
        Vector2 boxCastSize = new Vector2(_feetColl.bounds.size.x * _movementStats.HeadWidth, _movementStats.HeadDetectionRayLength);

        _headHit = Physics2D.BoxCast(boxCastOrigin, boxCastSize, 0f, Vector2.up,
            _movementStats.HeadDetectionRayLength, _movementStats.GroundLayer);
        if (_headHit.collider)
            _bumpedHead = true;
        else
            _bumpedHead = false;
    }

    #endregion

    private void UpdateTimers()
    {
        _jumpBufferTimer -= Time.deltaTime;

        if (!_isGrounded)
            _coyoteTimer -= Time.deltaTime;
        else
            _coyoteTimer = _movementStats.JumpCoyoteTime;

        if (!_isGrounded)
            _inAirTimer += Time.deltaTime;
    }

}
