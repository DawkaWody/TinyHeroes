using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InvertControls : MonoBehaviour, IPowerUp
{
    [SerializeField] private float _duration = 3.2f;

    public void Use(PlayerPowerupController player)
    {
        int playerIndex = player.GetComponent<PlayerData>().index;
        List<PlayerPowerupController> players = new();

        foreach (GameObject pl in GameObject.FindGameObjectsWithTag(GLOBALS.PlayerTag))
        {
            PlayerPowerupController target = pl.transform.GetComponent<PlayerPowerupController>();
            if (!target || target.blockOffensive || playerIndex == pl.GetComponent<PlayerData>().index) continue;

            players.Add(pl.transform.GetComponent<PlayerPowerupController>());
        }

        PlayerPowerupController randomPlayer = players[Random.Range(0, players.Count)];
        StartCoroutine(Invert(randomPlayer));
    }

    private IEnumerator Invert(PlayerPowerupController player)
    {
        player.invertControls = true;
        yield return new WaitForSeconds(_duration);
        player.invertControls = false;

        Destroy(gameObject);
    }
}
