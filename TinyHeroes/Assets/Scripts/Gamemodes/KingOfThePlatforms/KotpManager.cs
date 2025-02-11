using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class KotpManager : MonoBehaviour
{
    [SerializeField] private KotpPlatform[] _platforms;
    [SerializeField] private float _captureTime;
    [SerializeField] private float _gameTime;

    //UI
    [SerializeField] private TMP_Text timerText;

    private float _gameTimer;
    private bool _isPlaying;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        EnableAllPlatforms();

        _gameTimer = _gameTime;
        _isPlaying = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (_isPlaying)
        {
            _gameTimer -= Time.deltaTime;

            if (_gameTimer <= 0)
            {
                Debug.Log("Game has ended!!");
                DisableAllPlatforms();
                foreach (int playerIndex in GetWinners())
                {
                    Debug.Log("Player " + GLOBALS.playerColorNames[playerIndex] + " wins");
                }

                _isPlaying = false;
            }
            else 
            {
                UpdateTimerText();
            }
        }
    }

    void UpdateTimerText() 
    {
        int minutes = Mathf.FloorToInt(_gameTimer / 60);
        int seconds = Mathf.FloorToInt(_gameTimer % 60);
        timerText.text = minutes + ":" + seconds.ToString("D2");
    }

    void EnableAllPlatforms()
    {
        foreach (KotpPlatform platform in _platforms)
        {
            platform.enabled = true;
            platform.color = "";
            platform.captureTime = _captureTime;
        }
    }
    void DisableAllPlatforms()
    {
        foreach (KotpPlatform platform in _platforms)
        {
            platform.enabled = false;
            platform.capturingBar.Hide();
        }
    }

    List<int> GetWinners()
    {
        List<int> scores = new List<int>(new int[GLOBALS.playerColorNames.Count]);
        foreach (KotpPlatform platform in _platforms)
        {
            if (platform.color == "") continue;

            scores[GLOBALS.playerColorNames.IndexOf(platform.color)]++;
        }

        int maxScore = scores.Max();

        return Enumerable.Range(0, scores.Count)
            .Where(i => scores[i] == maxScore)
            .ToList();
    }
}
