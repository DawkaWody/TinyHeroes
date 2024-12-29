using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;

[RequireComponent(typeof(CinemachineCamera))]
public class RaceCamera : MonoBehaviour
{
    [SerializeField] private List<Transform> players;
    [SerializeField] private float minZoom = 5f;     
    [SerializeField] private float maxZoom = 20f;     
    [SerializeField] private float zoomBuffer = 2f;   
    [SerializeField] private float zoomSmoothing = 0.2f;
    [SerializeField] private float marginX = 0.2f; // in percentage of screen
    [SerializeField] private float marginY = 0.2f; // in percentage of screen
    [SerializeField] private BoxCollider2D eliminationCollider; 
    [SerializeField] private Transform cameraTarget; 

    private CinemachineCamera _camera;
    private Vector3 targetPosition;
    private Vector3 velocity = Vector3.zero;
    private float currentZoomVelocity;

    void Start()
    {
        _camera = GetComponent<CinemachineCamera>();
        if (cameraTarget == null) {
            Debug.LogError("CameraTarget is not assigned in RaceCamera.");
        }
    }

    void LateUpdate()
    {
        if (players.Count == 0) return;

        UpdateCameraPositionAndZoom();
        UpdateEliminationCollider();
    }

    private void UpdateCameraPositionAndZoom()
    {
        Transform raceLeader = GetRaceLeader();

        Bounds playerBounds = CalculatePlayerBounds();
        playerBounds.Encapsulate(raceLeader.position);

        Vector3 centerPosition = playerBounds.center;

        float distance = Mathf.Max(playerBounds.size.x, playerBounds.size.y);
        float targetZoom = Mathf.Clamp(distance + zoomBuffer, minZoom, maxZoom);

        float currentZoom = _camera.Lens.OrthographicSize;
        _camera.Lens.OrthographicSize = Mathf.SmoothDamp(currentZoom, targetZoom, ref currentZoomVelocity, zoomSmoothing);

        // change this to variables later
        float cameraHeight = _camera.Lens.OrthographicSize * 2f;
        float cameraWidth = cameraHeight * Screen.width / Screen.height;

        Vector3 cameraMin = centerPosition - new Vector3(cameraWidth / 2 - (cameraWidth * marginX), cameraHeight / 2 - (cameraHeight * marginY), 0);
        Vector3 cameraMax = centerPosition + new Vector3(cameraWidth / 2 - (cameraWidth * marginX), cameraHeight / 2 - (cameraHeight * marginY), 0);

        // adjust if leader is close to the edge of the screen
        Vector3 adjustment = Vector3.zero;

        if (raceLeader.position.x < cameraMin.x) {
            adjustment.x = raceLeader.position.x - cameraMin.x; 
        }
        else if (raceLeader.position.x > cameraMax.x) {
            adjustment.x = raceLeader.position.x - cameraMax.x; 
        }

        if (raceLeader.position.y < cameraMin.y) {
            adjustment.y = raceLeader.position.y - cameraMin.y; 
        }
        else if (raceLeader.position.y > cameraMax.y) {
            adjustment.y = raceLeader.position.y - cameraMax.y; 
        }

        targetPosition = centerPosition + adjustment;

        cameraTarget.position = Vector3.SmoothDamp(cameraTarget.position, targetPosition, ref velocity, zoomSmoothing);
    }

    private Transform GetRaceLeader()
    {
        Transform leader = players[0];
        float maxPosition = players[0].position.x;

        foreach (Transform player in players) {
            if (player.position.x > maxPosition) { // na razie po x, potem to trzeba zmienic
                leader = player;
                maxPosition = player.position.x;
            }
        }

        return leader;
    }

    private Bounds CalculatePlayerBounds()
    {
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        bool hasActivePlayers = false;

        foreach (Transform player in players) {
            if (player.gameObject.activeInHierarchy)
            {
                if (!hasActivePlayers) {
                    bounds = new Bounds(player.position, Vector3.zero);
                    hasActivePlayers = true;
                }
                else {
                    bounds.Encapsulate(player.position);
                }
            }
        }
        return bounds;
    }

    private void UpdateEliminationCollider()
    {
        float cameraHeight = _camera.Lens.OrthographicSize * 2f;
        float cameraWidth = cameraHeight * Screen.width / Screen.height;

        eliminationCollider.size = new Vector2(cameraWidth + 2f, cameraHeight + 2f); 
        eliminationCollider.transform.position = transform.position; 
    }
}