using UnityEngine;
using System.Collections;
public class Booster : MonoBehaviour
{
    [SerializeField] private float _speedMultiplier;
    [SerializeField] private float increaseTime;
    [SerializeField] private float decreaseTime;
    [SerializeField] private float particleEffectAfter;
    private bool active = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(GLOBALS.playerTag))
        {
            PlayerPowerupController playerPowerupController = other.GetComponentInParent<PlayerPowerupController>();
            PlayerFXController fxController = other.GetComponentInParent<PlayerFXController>();
            StartCoroutine(Boost(playerPowerupController, fxController));
        }
    }

    private IEnumerator Boost(PlayerPowerupController _playerPowerupController, PlayerFXController _fxController)
    {
        if (!active)
        {
            active = true;
            float targetSpeed = _speedMultiplier;
            float elapsedTime = 0f;
            GameObject particleEffect = _fxController.PlaySpeedBoostEffect(_playerPowerupController.transform.position);

            while (elapsedTime < increaseTime)
            {
                _playerPowerupController.speedMultiplier = Mathf.Lerp(1f, targetSpeed, elapsedTime / increaseTime);
                elapsedTime += Time.deltaTime;

                particleEffect.transform.position = _playerPowerupController.transform.position;
                yield return null;
            }
            _playerPowerupController.speedMultiplier = targetSpeed;
            elapsedTime = 0f;
            while (elapsedTime < decreaseTime)
            {
                _playerPowerupController.speedMultiplier = Mathf.Lerp(targetSpeed, 1f, elapsedTime / decreaseTime);
                elapsedTime += Time.deltaTime;

                particleEffect.transform.position = _playerPowerupController.transform.position;
                yield return null;
            }
            _playerPowerupController.speedMultiplier = 1f;
            elapsedTime = 0f;
            while (elapsedTime < particleEffectAfter)
            {
                elapsedTime += Time.deltaTime;
                particleEffect.transform.position = _playerPowerupController.transform.position;
                yield return null;
            }
            Destroy(particleEffect);
            active = false;
        }
    }
}
