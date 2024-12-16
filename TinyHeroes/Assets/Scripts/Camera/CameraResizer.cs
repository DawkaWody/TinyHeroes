using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;

[RequireComponent(typeof(CinemachineCamera))]
public class CameraResizer : MonoBehaviour
{
    [SerializeField] private List<Transform> _players;
    [SerializeField] private Collider2D _levelBounds;
    [SerializeField] private float _minZoom;
    [SerializeField] private float _maxZoom;

    private float _worldX;
    private Bounds _playerRect;
    private CinemachineCamera _camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = GetComponent<CinemachineCamera>();
        _worldX = _levelBounds.bounds.size.x;
    }

    void LateUpdate()
    {
        UpdateBounds();
        Debug.Log(_levelBounds);
        _camera.Lens.OrthographicSize = Mathf.Lerp(_minZoom, _maxZoom, _playerRect.size.x / _worldX);
    }

    void UpdateBounds()
    {
        _playerRect = new Bounds(_players[0].position, Vector3.zero);
        for (int i = 1; i < _players.Count; i++)
        {
            _playerRect.Encapsulate(_players[i].position);
        }
    }
}
