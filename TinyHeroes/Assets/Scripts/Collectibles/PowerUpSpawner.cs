using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnRate;
    [SerializeField] private GameObject _powerUpContainer;
    [SerializeField] private List<Transform> _players;
    [SerializeField] private float _minimumSpawnDistance;
    [SerializeField] private LayerMask diamondLayer;
    [SerializeField] private GameObject[] _powerUpPrefabs;

    private SpawnPointManager _spawnPointManager;

    private List<Transform> _spawnPoints = new List<Transform>();

    private float _spawnTimer;

    void Start()
    {
        _spawnPointManager = SpawnPointManager.Instance;

        foreach (Transform spawnPoint in _powerUpContainer.transform) {
            _spawnPoints.Add(spawnPoint);
            _spawnPointManager.RegisterSpawnPoint(spawnPoint); 
        }
    }

    // Update is called once per frame
    void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0) {
            SpawnPowerUp();
            _spawnTimer = _spawnRate;
        }
    }

    private List<Transform> GetValidSpawnPoints()
    {
        return _spawnPoints.FindAll(p =>
        {
            bool isFarFromPlayers = true;

            foreach (Transform player in _players) {
                float distance = Vector3.Distance(p.transform.position, player.position);
                if (distance < _minimumSpawnDistance) {
                    isFarFromPlayers = false;
                    break;
                }
            }
            bool hasDiamond = Physics2D.OverlapCircle(p.position, 0.1f, diamondLayer) != null;

            return isFarFromPlayers && _spawnPointManager.IsSpawnPointAvailable(p) && !hasDiamond;
        });
    }

    private void SpawnPowerUp()
    {
        List<Transform> validSpawnPoints = GetValidSpawnPoints();
        if (validSpawnPoints.Count > 0) {
            Transform chosenSpawnPoint = validSpawnPoints[Random.Range(0, validSpawnPoints.Count)];
            GameObject powerUpToSpawn = _powerUpPrefabs[Random.Range(0, _powerUpPrefabs.Length)];
            Instantiate(powerUpToSpawn, chosenSpawnPoint.position, Quaternion.identity);

            _spawnPointManager.SetSpawnPointAvailability(chosenSpawnPoint, false);
        }
    } 
}
