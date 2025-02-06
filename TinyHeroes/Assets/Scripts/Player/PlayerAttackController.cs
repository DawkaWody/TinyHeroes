using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimationController))]
[RequireComponent(typeof(PlayerInputHandler))]
[RequireComponent(typeof(PlayerPowerupController))]
public class PlayerAttackController : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private Transform _handPoint;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _attackableLayers;

    [Header("Combo Settings")] 
    [SerializeField] private float _comboTime;
    [SerializeField] private float _attackSpeedMultiplier;
    [SerializeField] private int _maxCombo;

    [Header("Knockback Settings")]
    [SerializeField] private float _knockbackStrength;
    [SerializeField] private float _knockbackTime;

    [Header("Parts to Ignore")]
    [SerializeField] private Collider2D _bodyColl;
    [SerializeField] private Collider2D _feetColl;

    private float _attackCooldown;
    private float _comboTimer;
    private int _comboStage;
    private float _attackSpeed;

    private Coroutine _knockbackResetCo;
    private PlayerAnimationController _animationController;
    private PlayerInputHandler _inputHandler;
    private PlayerPowerupController _powerupController;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animationController = GetComponent<PlayerAnimationController>();
        _inputHandler = GetComponent<PlayerInputHandler>();
        _powerupController = GetComponent<PlayerPowerupController>();

        _attackCooldown = 0;
        _comboTimer = 0;
        _comboStage = 0;
        _attackSpeed = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if (_attackCooldown > 0)
            _attackCooldown -= Time.deltaTime;
        if (_comboTimer > 0)
            _comboTimer -= Time.deltaTime;

        if (_comboTimer <= 0)
        {
            _comboStage = 0;
            _attackSpeed = 1f;
            _animationController.SetAttackSpeed(_attackSpeed);
            _comboTimer = 0;
        }

        if (_inputHandler.Attack1WasPressed && !_animationController.IsInAirOrJumping() && _attackCooldown <= 0)
        {
            _animationController.AnimateAttack(1);
            _attackCooldown = _animationController.AttackAnimationLength(1) / _attackSpeed;
        }
    }

    // Invoked by an animation event
    public void Attack1()
    {
        Vector2 attackPoint = new Vector2(_handPoint.position.x, _handPoint.position.y);
        foreach (Collider2D hit in Physics2D.OverlapCircleAll(attackPoint, 
                     _attackRange * _powerupController.attackRangeMultiplier))
        {
            if (hit.CompareTag(GLOBALS.playerTag) && hit != _bodyColl && hit != _feetColl 
                && !hit.GetComponentInParent<PlayerPowerupController>().isInvincible)
            {
                ApplyKnockback(hit.transform);
                IncreaseCombo();
            }
        }
    }

    private void ApplyKnockback(Transform target)
    {
        if (_knockbackResetCo != null) StopCoroutine(_knockbackResetCo);
        Rigidbody2D targetRigidbody = target.GetComponentInParent<Rigidbody2D>();
        PlayerMovementController targetMovementController = target.GetComponentInParent<PlayerMovementController>();

        if (targetMovementController != null) targetMovementController.enabled = false;
        Vector2 direction = (target.position - transform.position).normalized;
        direction += Vector2.up * _knockbackStrength / 3;

        targetRigidbody.AddForce(direction * _knockbackStrength, ForceMode2D.Impulse);
        _knockbackResetCo = StartCoroutine(ResetKnockback(targetRigidbody, targetMovementController));
    }

    private IEnumerator ResetKnockback(Rigidbody2D targetRigidbody, PlayerMovementController targetMovementController)
    {
        yield return new WaitForSeconds(_knockbackTime);
        targetRigidbody.linearVelocity = Vector2.zero;
        if (targetMovementController != null) targetMovementController.enabled = true;
    }

    private void IncreaseCombo()
    {
        if (_comboStage < _maxCombo)
        {
            _comboStage++;
            _attackSpeed *= _attackSpeedMultiplier;
            _animationController.SetAttackSpeed(_attackSpeed);
            _comboTimer = _comboTime;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_handPoint.position, _attackRange);
    }
}
