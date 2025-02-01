using System.Collections;
using UnityEngine;

public class Freeze : MonoBehaviour, IPowerUp
{
    [SerializeField] private float _duration = 0.9f;
    public void Use(PlayerPowerupController player)
    {
        foreach (GameObject pl in GameObject.FindGameObjectsWithTag(GLOBALS.playerTag))
        {
            PlayerPowerupController target = pl.transform.GetComponent<PlayerPowerupController>();
            if (pl != player.gameObject && target)
                StartCoroutine(FreezePlayer(target));
        }
    }

    private IEnumerator FreezePlayer(PlayerPowerupController player)
    {
        player.speedMultiplier = 0f;
        yield return new WaitForSeconds(_duration);
        player.speedMultiplier = 1f;
    }
}
