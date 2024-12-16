using UnityEngine;
using Unity.Cinemachine;
using System.Collections.Generic;

[RequireComponent(typeof(CinemachineCamera))]
public class CameraResizer : MonoBehaviour
{
    [SerializeField] private List<GameObject> _players;
    
    private CinemachineCamera _camera;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _camera = GetComponent<CinemachineCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
