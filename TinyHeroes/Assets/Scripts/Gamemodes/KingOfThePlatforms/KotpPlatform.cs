using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class KotpPlatform : MonoBehaviour
{
    [HideInInspector] public string color;
    [HideInInspector] public float captureTime;

    private List<int> _playersOn;
    private float _standingTimer;
    private bool _capturing;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playersOn = new List<int>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_playersOn.Count == 1)
        {
            if (!_capturing)
            {
                _capturing = true;
                _standingTimer = captureTime;
            }
            _standingTimer -= Time.deltaTime;

            if (_standingTimer <= 0f)
            {
                color = GLOBALS.playerColors[_playersOn[0]];

                if (color.Equals("pink"))
                    Debug.Log("pink");
                else if (color.Equals("blue"))
                    Debug.Log("blue");
                else if (color.Equals("white"))
                    Debug.Log("white");
            }
        }
        else
        {
            _capturing = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GLOBALS.playerTag))
        {
            _playersOn.Add(other.GetComponentInParent<PlayerData>().index);
            Debug.Log(other.gameObject.name);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(GLOBALS.playerTag))
            _playersOn.Remove(other.GetComponentInParent<PlayerData>().index);
    }
}
