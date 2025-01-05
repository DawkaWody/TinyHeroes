using System.Collections;
using UnityEngine;

[RequireComponent(typeof(PlayerAnimationController))]
[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerAttackController : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private Transform _handPoint;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _attackableLayers;

    [Header("Knockback Settings")]
    [SerializeField] private float _knockbackStrength;
    [SerializeField] private float _knockbackTime;

    [Header("Parts to Ingore")]
    [SerializeField] private Collider2D _bodyColl;

    private Coroutine _knockbackResetCo;
    private PlayerAnimationController _animationController;
    private PlayerInputHandler _inputHandler;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animationController = GetComponent<PlayerAnimationController>();
        _inputHandler = GetComponent<PlayerInputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputHandler.Attack1WasPressed && !_animationController.IsInAirOrJumping())
        {
            _animationController.AnimateAttack(1);
        }
    }

    // Invoked by an animation event
    public void Attack1()
    {
        Vector2 attackPoint = new Vector2(_handPoint.position.x, _handPoint.position.y);
        foreach (Collider2D hit in Physics2D.OverlapCircleAll(attackPoint, _attackRange))
        {
            if (hit != _bodyColl)
            {
                ApplyKnockback(hit.transform);
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
        direction += Vector2.up * _knockbackStrength / 2;

        targetRigidbody.AddForce(direction * _knockbackStrength, ForceMode2D.Impulse);
        _knockbackResetCo = StartCoroutine(ResetKnockback(targetRigidbody, targetMovementController));
    }

    IEnumerator ResetKnockback(Rigidbody2D targetRigidbody, PlayerMovementController targetMovementController)
    {
        yield return new WaitForSeconds(_knockbackTime);
        targetRigidbody.linearVelocity = Vector2.zero;
        if (targetMovementController != null) targetMovementController.enabled = true;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_handPoint.position, _attackRange);
    }
}
