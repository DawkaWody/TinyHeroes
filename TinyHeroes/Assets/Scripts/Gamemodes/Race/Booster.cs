using UnityEngine;
using System.Collections;
public class Booster : MonoBehaviour
{
    [SerializeField] private float _speedMultiplier;
    [SerializeField] private float increaseTime;
    [SerializeField] private float decreaseTime;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(GLOBALS.playerTag))
        {
            PlayerPowerupController playerPowerupController = other.GetComponentInParent<PlayerPowerupController>();
            StartCoroutine(Boost(playerPowerupController));
        }
    }

    private IEnumerator Boost(PlayerPowerupController player)
    {
        float targetSpeed = _speedMultiplier;
        float elapsedTime = 0f;
        while (elapsedTime < increaseTime)
        {
            player.speedMultiplier = Mathf.Lerp(1f, targetSpeed, elapsedTime / increaseTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        player.speedMultiplier = targetSpeed;
        elapsedTime = 0f;
        while (elapsedTime < decreaseTime)
        {
            player.speedMultiplier = Mathf.Lerp(targetSpeed, 1f, elapsedTime / decreaseTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        player.speedMultiplier = 1f;
    }
}
