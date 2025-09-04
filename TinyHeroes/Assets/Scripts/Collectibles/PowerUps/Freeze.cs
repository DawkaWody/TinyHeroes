using System.Collections;
using System.Collections.Generic;
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
        List<GameObject> frozenPlayers = new();

        foreach (GameObject pl in GameObject.FindGameObjectsWithTag(GLOBALS.PlayerTag))
        {
            PlayerPowerupController target = pl.transform.GetComponent<PlayerPowerupController>();
            if (!target || target.blockOffensive || playerIndex == pl.GetComponent<PlayerData>().index) continue;
            
            target.speedMultiplier = 0f;
            frozenPlayers.Add(pl);
        }

        yield return new WaitForSeconds(_duration);

        foreach (GameObject pl in frozenPlayers)
        {
            pl.transform.GetComponent<PlayerPowerupController>().speedMultiplier = 1f;
        }

        Destroy(gameObject);
    }
}
