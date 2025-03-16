using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerDeathController : MonoBehaviour
{
    [SerializeField] private float _respawnTime = 2f;

    private bool _isDead;

    private bool _hasSpriteRenderer;
    private bool _hasBodyColl;
    private bool _hasFeetColl;
    private bool _hasPlayerInput;
    private bool _hasMovementController;
    private bool _hasRigidbody;

    private SpriteRenderer _spriteRenderer;
    private CapsuleCollider2D _bodyColl;
    private BoxCollider2D _feetColl;
    private PlayerInput _playerInput;
    private PlayerMovementController _playerMovementController;
    private Rigidbody2D _rigidbody;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _isDead = false;

        _hasSpriteRenderer = TryGetComponent<SpriteRenderer>(out _spriteRenderer);
        _hasBodyColl = Utils.TryGetComponentInChildren<CapsuleCollider2D>(this.gameObject, out _bodyColl);
        _hasFeetColl = Utils.TryGetComponentInChildren<BoxCollider2D>(this.gameObject, out _feetColl);
        _hasPlayerInput = TryGetComponent<PlayerInput>(out _playerInput);
        _hasMovementController = TryGetComponent<PlayerMovementController>(out _playerMovementController);
        _hasRigidbody = TryGetComponent<Rigidbody2D>(out _rigidbody);
    }

    public void Die()
    {
        if (_isDead) return;
        _isDead = true;

        if (_hasSpriteRenderer) _spriteRenderer.enabled = false;
        if (_hasBodyColl) _bodyColl.enabled = false;
        if (_hasFeetColl) _feetColl.enabled = false;
        // if (_hasPlayerInput) _playerInput.enabled = false;
        if (_hasMovementController) _playerMovementController.enabled = false;
        if (_hasRigidbody) _rigidbody.linearVelocity = Vector2.zero;
        StartCoroutine(RespawnCo());
    }

    private IEnumerator RespawnCo()
    {
        yield return new WaitForSeconds(_respawnTime);

        transform.position = PlayerSpawningManager.Instance.GetRandomSpawnpoint().position;

        if (_hasSpriteRenderer) _spriteRenderer.enabled = true;
        if (_hasBodyColl) _bodyColl.enabled = true;
        if (_hasFeetColl) _feetColl.enabled = true;
        // if (_hasPlayerInput) _playerInput.enabled = true;
        if (_hasMovementController) _playerMovementController.enabled = true;

        _isDead = false;
    }
}
