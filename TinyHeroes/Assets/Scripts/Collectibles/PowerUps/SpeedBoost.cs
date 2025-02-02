using System.Collections;
using UnityEngine;

public class SpeedBoost : MonoBehaviour, IPowerUp
{
    [SerializeField] private float _speedMultiplier = 1.5f;
    [SerializeField] private float _duration = 2f;
    public void Use(PlayerPowerupController player) 
    {
        StartCoroutine(BoostSpeed(player));
    }

    private IEnumerator BoostSpeed(PlayerPowerupController player)
    {
        player.speedMultiplier = _speedMultiplier;
        yield return new WaitForSeconds(_duration);
        player.speedMultiplier = 1f;

        Destroy(gameObject);
    }
}
