using System.Collections.Generic;
using UnityEngine;

public class KillBound : MonoBehaviour
{
    public List<PlayerDeathController> players;

    void Update()
    {
        foreach (PlayerDeathController player in players)
        {
            if (player.transform.position.y < transform.position.y)
            {
                player.Die();
            }
        }
    }
}
