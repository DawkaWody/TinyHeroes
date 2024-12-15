using System.Collections.Generic;
using UnityEngine;

public class DiamondSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnRate;
    [SerializeField] private GameObject _diamondContainer;
    [SerializeField] private Transform _player;
    [SerializeField] private float _minimumSpawnDistance;

    private List<GameObject> _diamonds;

    private float _spawnTimer;

    void Start() 
    {
        _diamonds = new List<GameObject>();
        foreach (Transform d in _diamondContainer.transform) 
        {
            _diamonds.Add(d.gameObject);
            d.gameObject.SetActive(false);
        }

        _spawnTimer = _spawnRate;
    }

    // Update is called once per frame
    void Update() {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0) {
            SpawnDiamond();
            _spawnTimer = _spawnRate;
        }
    }

    private void SpawnDiamond() {
        List<GameObject> validDiamonds = GetDiamondsFarFromPlayer();
        if (validDiamonds.Count > 0) {
            GameObject diamondToSpawn = validDiamonds[Random.Range(0, validDiamonds.Count)];
            diamondToSpawn.SetActive(true);
        }
    }

    private List<GameObject> GetDiamondsFarFromPlayer() {
        return _diamonds.FindAll(d => { // lambda ;)
            float distance = Vector3.Distance(d.transform.position, _player.position);
            return !d.activeSelf && distance >= _minimumSpawnDistance; // if inactive and far enough
        });
    }
}