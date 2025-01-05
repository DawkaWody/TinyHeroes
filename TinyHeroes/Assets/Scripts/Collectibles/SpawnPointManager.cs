using UnityEngine;
using System.Collections.Generic;

public class SpawnPointManager : MonoBehaviour
{
    public static SpawnPointManager Instance { get; private set; }

    private Dictionary<Transform, bool> _spawnPoints = new Dictionary<Transform, bool>();
    
    private void Awake()
    {
        if (Instance != null && Instance != this) {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void RegisterSpawnPoint(Transform spawnPoint)
    {
        if (!_spawnPoints.ContainsKey(spawnPoint)) {
            _spawnPoints[spawnPoint] = true;
        }
    }

    public bool IsSpawnPointAvailable(Transform spawnPoint)
    {
        return _spawnPoints.ContainsKey(spawnPoint) && _spawnPoints[spawnPoint];
    }

    public void SetSpawnPointAvailability(Transform spawnPoint, bool isAvailable)
    {
        if (_spawnPoints.ContainsKey(spawnPoint)) {
            _spawnPoints[spawnPoint] = isAvailable;
        }
    }

    public List<Transform> GetAvailableSpawnPoints()
    {
        List<Transform> availablePoints = new List<Transform> ();  
        foreach (var pair in _spawnPoints) {
            if (pair.Value) availablePoints.Add(pair.Key);
        }
        return availablePoints;
    }
}
