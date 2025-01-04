using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KotpManager : MonoBehaviour
{
    [SerializeField] private KotpPlatform[] _platforms;
    [SerializeField] private float _captureTime;
    [SerializeField] private float _gameTime;

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
                    Debug.Log("Player " + GLOBALS.playerColors[playerIndex] + " wins");
                }

                _isPlaying = false;
            }
        }
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
        }
    }

    List<int> GetWinners()
    {
        List<int> scores = new List<int>(new int[GLOBALS.playerColors.Count]);
        foreach (KotpPlatform platform in _platforms)
        {
            if (platform.color == "") continue;

            scores[GLOBALS.playerColors.IndexOf(platform.color)]++;
        }

        int maxScore = scores.Max();

        return Enumerable.Range(0, scores.Count)
            .Where(i => scores[i] == maxScore)
            .ToList();
    }
}
