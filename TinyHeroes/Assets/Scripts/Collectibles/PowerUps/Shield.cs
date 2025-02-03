using System.Collections;
using UnityEngine;

public class Shield : MonoBehaviour, IPowerUp
{
    [SerializeField] private float _duration = 3f;

    public void Use(PlayerPowerupController player)
    {
        StartCoroutine(BlockOffensive(player));
    }

    private IEnumerator BlockOffensive(PlayerPowerupController player)
    {
        player.blockOffensive = true;
        yield return new WaitForSeconds(_duration);
        player.blockOffensive = false;

        Destroy(gameObject);
    }
}
