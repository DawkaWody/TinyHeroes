using System.Collections;
using UnityEngine;

public class CameraPlayerFollow : MonoBehaviour
{
    [SerializeField] private Transform _player;
    private float _flipYRotationTime = 0.5f;

    private Coroutine _turnCo;
    private bool _isFacingRight;
    private bool _isFlipping;

    private PlayerMovementController _playerMovement;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerMovement = _player.GetComponent<PlayerMovementController>();

        _isFacingRight = _playerMovement.isFacingRight;
    }

    // Update is called once per frame
    void Update()
    {
        // Follow player
        transform.position = _player.position;
    }

    #region Camera Turn

    public void Turn()
    {
        if (_isFlipping)
        {
            StopCoroutine(_turnCo);
            ResetRotation();
        }
        _turnCo = StartCoroutine(FlipYLerp());
    }

    private IEnumerator FlipYLerp()
    {
        _isFlipping = true;

        float startRotation = transform.localEulerAngles.y;
        float endRotation = EndRotation();
        float yRotation = 0f;

        float elapsedTime = 0f;
        while (elapsedTime < _flipYRotationTime)
        {
            elapsedTime += Time.deltaTime;

            yRotation = Mathf.Lerp(startRotation, endRotation, elapsedTime / _flipYRotationTime);
            transform.rotation = Quaternion.Euler(0f, yRotation, 0f);

            yield return null;
        }

        _isFlipping = false;
    }

    private float EndRotation()
    {
        _isFacingRight = !_isFacingRight;
        return (_isFacingRight) ? 180f : 0f;
    }

    private void ResetRotation()
    {
        transform.rotation = Quaternion.Euler(0f, EndRotation(), 0f);
    }

    #endregion
}
