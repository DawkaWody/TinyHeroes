using System.Collections.Generic;
using UnityEngine;

public class PlayerSpawningManager : MonoBehaviour
{
    public static PlayerSpawningManager Instance { get; private set; }

    [SerializeField] private GameObject _spawnPointContainer;
    [SerializeField] private List<Transform> _players;
    [SerializeField] private float _minimumSpawnDistance;

    private List<Transform> _spawnPoints = new();
    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    void Start()
    {
        foreach (Transform spawnPoint in _spawnPointContainer.transform)
        {
            _spawnPoints.Add(spawnPoint);
        }
    }

    private List<Transform> GetValidSpawnPoints()
    {
        return _spawnPoints.FindAll(p =>
        {
            bool isFarFromPlayers = true;

            foreach (Transform player in _players)
            {
                float distance = Vector3.Distance(p.transform.position, player.position);
                if (distance < _minimumSpawnDistance)
                {
                    isFarFromPlayers = false;
                    break;
                }
            }

            return isFarFromPlayers;
        });
    }

    public Transform GetRandomSpawnpoint()
    {
        List<Transform> validSpawnPoints = GetValidSpawnPoints();
        return validSpawnPoints.Count > 0 ? validSpawnPoints[Random.Range(0, validSpawnPoints.Count)] : null;
    }
}
