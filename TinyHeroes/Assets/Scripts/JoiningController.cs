using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoiningController : MonoBehaviour
{
    private int _playerCount = 0;

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
        StartCoroutine(SetupPlayerWithDelay(playerInput));
    }

    private IEnumerator SetupPlayerWithDelay(PlayerInput playerInput)
    {
        yield return new WaitForEndOfFrame();
        _playerCount++;
        playerInput.GetComponent<PlayerData>().index = _playerCount;
        playerInput.GetComponent<PlayerAnimationController>().ActivateLayer(_playerCount - 1);

        PlayerSpawningManager.Instance.players.Add(playerInput.transform);
        PlayerSpawningManager.Instance.killBound.players.Add(playerInput.GetComponent<PlayerDeathController>());
    }
}
