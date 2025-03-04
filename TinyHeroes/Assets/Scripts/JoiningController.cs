using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoiningController : MonoBehaviour
{

    private PlayerInputManager _playerInputManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _playerInputManager = GetComponent<PlayerInputManager>();    
    }

    // Update is called once per frame
    void Update()
    {
        if (InputSystem.devices.OfType<Gamepad>().Count() > 1)
        {
            Debug.Log("Multiple gamepads connected");
        }
    }

    public void SetupPlayer(PlayerInput playerInput)
    {
        _playerInputManager.playerPrefab = this.gameObject;
    }
}
