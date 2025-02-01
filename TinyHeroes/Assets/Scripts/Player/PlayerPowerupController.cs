using UnityEngine;

[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerPowerupController : MonoBehaviour
{
    internal float speedMultiplier = 1f;

    private IPowerUp[] _powerUps = new IPowerUp[2];

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
            UiManager.Instance.HidePowerUp(GetComponent<PlayerData>().index, 1);
        }
        else if (_inputHandler.PowerUp2WasPressed)
        {
            UsePowerUp(1);
            UiManager.Instance.HidePowerUp(GetComponent<PlayerData>().index, 2);
        }
    }

    public int GetFirstAvailableSlot()
    {
        for (int i = 0; i < _powerUps.Length; i++)
        {
            if (_powerUps[i] == null)
                return i;
        }

        return -1;
    }

    public void CollectPowerUp(IPowerUp powerUp)
    {
        int idx = GetFirstAvailableSlot();
        if (idx == -1) return;
        _powerUps[idx] = powerUp;
    }

    public void UsePowerUp(int slot)
    {
        if (slot < 0 || slot >= _powerUps.Length) return;
        _powerUps[slot]?.Use(this);
        _powerUps[slot] = null;
    }
}
