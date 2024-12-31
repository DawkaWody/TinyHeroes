using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using TMPro;

public class RaceManager : MonoBehaviour
{
    [Header("Race Settings")]
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private List<GameObject> players;
    [SerializeField] private float countdownTime = 3f; 

    [Header("UI Elements")]
    [SerializeField] private TMP_Text countdownText; 
    [SerializeField] private TMP_Text raceTimerText;

    private float raceStartTime;
    private bool raceInProgress = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PositionPlayersAtSpawn();
        StartCoroutine(StartRace());
    }

    private void PositionPlayersAtSpawn()
    {
        foreach (GameObject player in players) {
            player.transform.position = spawnPoint.position;
        }
    }

    private IEnumerator StartRace()
    {
        EnablePlayerMovement(false);

        countdownText.gameObject.SetActive(true);

        for (float time = countdownTime; time > 0; time -= 1f) {
            countdownText.text = Mathf.Ceil(time).ToString();
            yield return new WaitForSeconds(1f);
        }
        countdownText.text = "GO!";
        yield return new WaitForSeconds(1f);
        countdownText.gameObject.SetActive(false);

        raceStartTime = Time.time;
        raceInProgress = true;

        EnablePlayerMovement(true);
    }

    // Update is called once per frame
    void Update()
    {
        if (raceInProgress) {
            UpdateRaceTimer();
        }
        
    }

    private void UpdateRaceTimer()
    {
        float elapsedTime = Time.time - raceStartTime;
        raceTimerText.text = FormatTime(elapsedTime);
    }

    private string FormatTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);
        int milliseconds = Mathf.FloorToInt((time * 1000) % 1000);

        return string.Format("{0:00}.{1:00}.{2:000}", minutes, seconds, milliseconds);
    }

    private void EnablePlayerMovement(bool enable)
    {
        foreach (GameObject player in players) {
            var controller = player.GetComponent<PlayerMovementController>();
            if (controller != null) {
                controller.EnableMovement(enable);
            }
        }
    }

    public void OnPlayerFinished()
    {
        if (raceInProgress) {
            raceInProgress = false;
        }
    }
}
