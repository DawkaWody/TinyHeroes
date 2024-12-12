using UnityEngine;

[RequireComponent(typeof(PlayerAnimationController))]
[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerAttackController : MonoBehaviour
{
    [Header("Attack Settings")]
    [SerializeField] private Transform _handPoint;
    [SerializeField] private float _attackRange;
    [SerializeField] private LayerMask _attackableLayers;

    [Header("Parts to Ingore")]
    [SerializeField] private Collider2D _bodyColl;

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
                Debug.Log(hit);
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(_handPoint.position, _attackRange);
    }
}
