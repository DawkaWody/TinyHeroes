using System.Collections;
using UnityEngine;

public class SpeedBoost : MonoBehaviour, IPowerUp
{
    [SerializeField] private float _speedMultiplier = 2f;
    [SerializeField] private float _duration = 5f;
    public void Use(PlayerPowerupController player) 
    {
        Debug.Log("used!"); // wiêcej debuglogow (;
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
