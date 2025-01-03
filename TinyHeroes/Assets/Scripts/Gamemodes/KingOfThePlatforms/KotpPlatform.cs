using System.Collections.Generic;
using UnityEngine;

public class KotpPlatform : MonoBehaviour
{
    public string color;
    public float _recolorTime;

    private List<int> _playersOn = new();
    private float _standingTimer;
    private bool _capturing;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(GLOBALS.playerTag))
        {
            _playersOn.Add(other.GetComponentInParent<PlayerData>().index);
            Debug.Log(other.gameObject.name);
        }
    }

    void Update()
    {
        if (_playersOn.Count == 1)
        {
            if (!_capturing)
            {
                _capturing = true;
                _standingTimer = _recolorTime;
            }
            _standingTimer -= Time.deltaTime;

            if (_standingTimer <= 0f)
            {
                color = GLOBALS.playerColors[_playersOn[0]];
                Debug.Log(color);
            }
        }
        else
        {
            _capturing = false;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag(GLOBALS.playerTag))
            _playersOn.Remove(other.GetComponentInParent<PlayerData>().index);
    }
}
