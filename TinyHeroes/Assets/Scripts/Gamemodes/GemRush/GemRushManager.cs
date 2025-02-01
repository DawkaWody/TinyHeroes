using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class GemRushManager : MonoBehaviour
{
    [SerializeField] private PlayerHandler[] _players;
    [SerializeField] private float _gameDuration;

    //UI
    [SerializeField] private TMP_Text timerText;

    private float _gameTimer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _gameTimer = _gameDuration;
    }

    // Update is called once per frame
    void Update()
    {
        _gameTimer -= Time.deltaTime;

        if (_gameTimer <= 0)
        {
            Debug.Log("Game end");
            foreach (int playerIndex in GetWinners())
            {
                Debug.Log("Player " + GLOBALS.playerColors[playerIndex] + " wins!");
            }    
        }
        else 
        {
            UpdateTimerText();
        }
    }

    void UpdateTimerText() 
    {
        int minutes = Mathf.FloorToInt(_gameTimer / 60);
        int seconds = Mathf.FloorToInt(_gameTimer % 60);
        timerText.text = minutes + ":" + seconds.ToString("D2");
    }

    List<int> GetWinners()
    {
        List<int> scores = new List<int>(new int[_players.Length]);

        for (int i = 0; i < scores.Count; i++)
        {
            PlayerHandler player = _players[i];
            scores[player.GetComponent<PlayerData>().index] = player.diamonds;
        }

        int maxScore = scores.Max();

        return Enumerable.Range(0, scores.Count)
            .Where(i => scores[i] == maxScore)
            .ToList();
    }
}
