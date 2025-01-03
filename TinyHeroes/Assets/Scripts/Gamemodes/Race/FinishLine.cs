using UnityEngine;

public class FinishLine : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player")) {
            RaceStatus player = collision.GetComponentInParent<RaceStatus>();
            if (player != null) {
                player.FinishRace();
            }
        }
    }
}