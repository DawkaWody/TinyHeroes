// attached to every player to handle race behaviour
using UnityEngine;

public class RaceStatus : MonoBehaviour
{
    public bool hasFinished = false;
    public float raceTime = 0;

    [SerializeField] private RaceManager raceManager;

    private void Update()
    {
        if (!hasFinished) {
            raceTime += Time.deltaTime;
        }
    }

    public void FinishRace()
    {
        if (!hasFinished) {
            hasFinished = true;

            raceManager.OnPlayerFinished();
        }
    }
}
