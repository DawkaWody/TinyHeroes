using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class KotpPlatform : MonoBehaviour
{
    public ProgressBar capturingBar;

    [HideInInspector] public string color;
    [HideInInspector] public float captureTime;

    private List<int> _playersOn;
    private float _standingTimer;
    private bool _capturing;

    private int _pinkFlagTriggerId;
    private int _blueFlagTriggerId;
    private int _whiteFlagTriggerId;
    private int _defaultFlagTriggerId;

    private Animator _flagAnimator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created

    void Start()
    {
        _playersOn = new List<int>();

        _flagAnimator = GetComponentInChildren<Animator>();

        _pinkFlagTriggerId = Animator.StringToHash("PinkFlag");
        _blueFlagTriggerId = Animator.StringToHash("BlueFlag");
        _whiteFlagTriggerId = Animator.StringToHash("WhiteFlag");
        _defaultFlagTriggerId = Animator.StringToHash("DefaultFlag");

        _flagAnimator.SetTrigger(_defaultFlagTriggerId);
    }

    // Update is called once per frame
    void Update()
    {
        if (_playersOn.Count == 1)
        {
            if (!_capturing && color != GLOBALS.playerColorNames[_playersOn[0]])
            {
                _capturing = true;
                _standingTimer = captureTime;

                capturingBar.SetColor(GLOBALS.playerColors[_playersOn[0]]);
                capturingBar.Fill(captureTime);
            }
            _standingTimer -= Time.deltaTime;

            if (_standingTimer <= 0f)
            {
                color = GLOBALS.playerColorNames[_playersOn[0]];

                switch (color)
                {
                    case "pink":
                        _flagAnimator.SetTrigger(_pinkFlagTriggerId);
                        break;
                    case "blue":
                        _flagAnimator.SetTrigger(_blueFlagTriggerId);
                        break;
                    case "white":
                        _flagAnimator.SetTrigger(_whiteFlagTriggerId);
                        break;
                }

                capturingBar.Hide();
            }
        }
        else
        {
            _capturing = false;
            capturingBar.Hide();
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GLOBALS.playerTag))
            _playersOn.Add(other.GetComponentInParent<PlayerData>().index);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(GLOBALS.playerTag))
            _playersOn.Remove(other.GetComponentInParent<PlayerData>().index);
    }
}
