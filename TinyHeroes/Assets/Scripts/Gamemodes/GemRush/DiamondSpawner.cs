using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiamondSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnRate;
    [SerializeField] private GameObject _diamondContainer;
    [SerializeField] private Transform _player;
    [SerializeField] private float _minimumSpawnDistance;
    [SerializeField] private LayerMask powerUpLayer;

    private SpawnPointManager _spawnPointManager;

    private List<GameObject> _diamonds = new();

    private float _spawnTimer;

    void Start()
    {
        _spawnPointManager = SpawnPointManager.Instance;

        foreach (Transform d in _diamondContainer.transform) {
            _diamonds.Add(d.gameObject);
            d.gameObject.SetActive(false);
            _spawnPointManager.RegisterSpawnPoint(d);
        }
    }

    // Update is called once per frame
    void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0) {
            SpawnDiamond();
            _spawnTimer = _spawnRate;
        }
    }

    private List<GameObject> GetDiamondsFarFromPlayer()
    {
        return _diamonds.FindAll(d =>
        {
            float distance = Vector3.Distance(d.transform.position, _player.position);
            bool hasPowerUp = Physics2D.OverlapCircle(d.transform.position, 0.1f, powerUpLayer) != null;

            return !d.activeSelf && distance >= _minimumSpawnDistance && _spawnPointManager.IsSpawnPointAvailable(d.transform) && !hasPowerUp;
        });
    }

    private void SpawnDiamond()
    {
        List<GameObject> validDiamonds = GetDiamondsFarFromPlayer();
        if (validDiamonds.Count > 0) {
            GameObject diamondToSpawn = validDiamonds[Random.Range(0, validDiamonds.Count)];
            diamondToSpawn.SetActive(true);

            _spawnPointManager.SetSpawnPointAvailability(diamondToSpawn.transform, false);
        }
    }
}
