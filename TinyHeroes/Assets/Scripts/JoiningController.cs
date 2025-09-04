using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class JoiningController : MonoBehaviour
{
    private Queue<int> rejoinQueue;
    private int _playerCount; 
    private int nextId;

    private CameraManager _cameraManager;
    private PlayerInputManager _playerInputManager;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _cameraManager = GameObject.FindGameObjectWithTag(GLOBALS.cameraManagerTag).GetComponent<CameraManager>();
        _playerInputManager = GetComponent<PlayerInputManager>();

        _playerCount = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (_playerCount > 1)
        {
            CameraManager.Instance.ChangeCameraToMultiplayer();
        }
        else
        {
            CameraManager.Instance.ChangeCameraToSingleplayer();
        }
    }

    public void OnPlayerJoin(PlayerInput playerInput)
    {
        StartCoroutine(SetupPlayerWithDelay(playerInput));
    }

    public void OnPlayerLeave(PlayerInput playerInput)
    {
        int playerIndex = playerInput.GetComponent<PlayerData>().index;

        CameraManager.Instance.RemovePlayerTarget(playerInput.transform);
        UiManager.Instance.HidePlayerInfo(playerIndex);

        rejoinQueue.Enqueue(playerIndex);
    }

    private void RejoinPlayer(PlayerInput playerInput)
    {
        int playerIdx = rejoinQueue.Peek();

        PlayerSpawningManager.Instance.players.Add(playerInput.transform);
        PlayerSpawningManager.Instance.killBound.players.Add(playerInput.GetComponent<PlayerDeathController>());

        CameraManager.Instance.SetPlayerTarget(playerInput.transform);
        CameraManager.Instance.AddPlayerTarget(playerInput.transform);
        UiManager.Instance.ShowPlayerInfo(playerIdx);

        rejoinQueue.Dequeue();
    }

    private IEnumerator SetupPlayerWithDelay(PlayerInput playerInput)
    {
        yield return new WaitForEndOfFrame();
        playerInput.GetComponent<PlayerData>().index = _playerCount;
        playerInput.GetComponent<PlayerAnimationController>().ActivateLayer(_playerCount);

        PlayerSpawningManager.Instance.players.Add(playerInput.transform);
        PlayerSpawningManager.Instance.killBound.players.Add(playerInput.GetComponent<PlayerDeathController>());

        CameraManager.Instance.SetPlayerTarget(playerInput.transform);
        CameraManager.Instance.AddPlayerTarget(playerInput.transform);
        UiManager.Instance.ShowPlayerInfo(_playerCount);

        _playerCount++;
    }
}
