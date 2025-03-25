using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class PlayerKnockbackController : MonoBehaviour
{
    [SerializeField] private float _knockbackTime;
    [SerializeField] private float _hitDirectionForce;
    [SerializeField] private float _constForce;
    [SerializeField] private float _inputForce;

    public bool IsBeingKnockedBack { get; private set; }

    private Rigidbody2D _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
    }

    public IEnumerator KnockbackAction(Vector2 hitDirection, Vector2 constForceDirection, float inputDirection)
    {
        IsBeingKnockedBack = true;

        Vector2 hitForce = hitDirection * _hitDirectionForce;
        Vector2 constantForce = constForceDirection * _constForce;
        Vector2 knockbackForce;
        Vector2 combinedForce;

        float elapsedTime = 0;
        while (elapsedTime < _knockbackTime)
        {
            elapsedTime += Time.fixedDeltaTime;
            
            knockbackForce = hitForce + constantForce;
            combinedForce = knockbackForce + new Vector2(inputDirection * _inputForce, 0f);

            _rigidbody.linearVelocity = combinedForce;

            yield return new WaitForFixedUpdate();
        }

        IsBeingKnockedBack = false;
    }

}
