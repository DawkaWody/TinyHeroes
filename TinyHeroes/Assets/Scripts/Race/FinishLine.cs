using UnityEngine;
[RequireComponent(typeof(RaceStatus))]
public class FinishLine : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D collision)
    {
        RaceStatus participant = collision.GetComponentInParent<RaceStatus>();
        if (participant != null && !participant.hasFinished) {
            participant.FinishRace();
        }
    }
}