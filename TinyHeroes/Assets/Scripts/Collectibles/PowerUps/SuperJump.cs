using System.Collections;
using UnityEngine;

public class SuperJump : MonoBehaviour, IPowerUp
{
    [SerializeField] private float _jumpMultiplier = 1.4f;
    [SerializeField] private float _duration = 2f;

    public void Use(PlayerPowerupController player)
    {
        StartCoroutine(BoostJump(player));
    }

    private IEnumerator BoostJump(PlayerPowerupController player)
    {
        player.jumpMultiplier = _jumpMultiplier;
        yield return new WaitForSeconds(_duration);
        player.jumpMultiplier = 1f;

        Destroy(gameObject);
    }
}
