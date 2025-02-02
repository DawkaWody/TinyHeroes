using System.Collections;
using UnityEngine;

public class Armor : MonoBehaviour, IPowerUp
{
    [SerializeField] private float _duration = 1.5f;
    
    public void Use(PlayerPowerupController player)
    {
        StartCoroutine(MakeInvincible(player));
    }

    private IEnumerator MakeInvincible(PlayerPowerupController player)
    {
        player.isInvincible = true;
        yield return new WaitForSeconds(_duration);
        player.isInvincible = false;

        Destroy(gameObject);
    }
}
