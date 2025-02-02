using System.Collections;
using UnityEngine;

public class Freeze : MonoBehaviour, IPowerUp
{
    [SerializeField] private float _duration = 1.2f;
    public void Use(PlayerPowerupController player)
    {
        StartCoroutine(FreezeOtherPlayers(player));
    }

    private IEnumerator FreezeOtherPlayers(PlayerPowerupController player)
    {
        int playerIndex = player.GetComponent<PlayerData>().index;

        foreach (GameObject pl in GameObject.FindGameObjectsWithTag(GLOBALS.playerTag))
        {
            PlayerPowerupController target = pl.transform.GetComponent<PlayerPowerupController>();
            if (target && playerIndex != pl.GetComponent<PlayerData>().index)
                target.speedMultiplier = 0f;
        }

        yield return new WaitForSeconds(_duration);

        foreach (GameObject pl in GameObject.FindGameObjectsWithTag(GLOBALS.playerTag))
        {
            PlayerPowerupController target = pl.transform.GetComponent<PlayerPowerupController>();
            if (target && playerIndex != pl.GetComponent<PlayerData>().index)
                target.speedMultiplier = 1f;
        }

        Destroy(gameObject);
    }
}
