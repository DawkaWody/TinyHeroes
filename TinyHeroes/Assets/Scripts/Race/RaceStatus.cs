// attached to every player to handle race behaviour
using UnityEngine;

public class RaceStatus : MonoBehaviour
{
    public bool hasFinished = false;
    public float raceTime = 0;

    private void Update()
    {
        if (!hasFinished) {
            raceTime += Time.deltaTime;
        }
    }

    public void FinishRace()
    {
        hasFinished = true;
        Debug.Log(gameObject.name + " finished the race in " + raceTime.ToString("F3") + " seconds!"); //F3 zaokragla
        // (maybe)more logic later for updating leaderboard or sth
    }
}
