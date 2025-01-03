// attached to every player to handle race behaviour
using UnityEngine;

public class RaceStatus : MonoBehaviour
{
    public bool hasFinished = false;

    [SerializeField] private RaceManager raceManager;

    public void FinishRace()
    {
        if (!hasFinished) {
            hasFinished = true;
            raceManager.OnPlayerFinished();
        }
    }
}
