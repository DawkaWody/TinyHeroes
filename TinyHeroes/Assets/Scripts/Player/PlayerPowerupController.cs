using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerPowerupController : MonoBehaviour
{
    private IPowerUp[] _powerups = new IPowerUp[2];

    private PlayerInputHandler _inputHandler;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _inputHandler = GetComponent<PlayerInputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputHandler.PowerUp1WasPressed)
        {
            UsePowerUp(0);
        }
        else if (_inputHandler.PowerUp2WasPressed)
        {
            UsePowerUp(1);
        }
    }

    public void CollectPowerUp(IPowerUp powerup)
    {
        for (int i = 0; i < _powerups.Length; i++)
        {
            if (_powerups[i] == null)
            {
                _powerups[i] = powerup;
                break;
            }
        }
    }

    public void UsePowerUp(int slot)
    {
        if (slot < 0 || slot >= _powerups.Length) return;
        _powerups[slot]?.Use();
        _powerups[slot] = null;
    }
}
