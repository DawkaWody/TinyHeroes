using UnityEngine;

public class DiamondSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnRate;
    [SerializeField] private GameObject[] _diamonds;
    [SerializeField] private Transform _player;
    [SerializeField] private float _minimumSpawnDistance;

    private float _spawnTimer;

    void Start() {
        foreach (GameObject d in _diamonds) {
            d.SetActive(false);
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
        GameObject[] validDiamonds = GetDiamondsFarFromPlayer();
        if (validDiamonds.Length > 0) {
            GameObject diamondToSpawn = validDiamonds[Random.Range(0, validDiamonds.Length)];
            diamondToSpawn.SetActive(true);
        }
    }

    private GameObject[] GetDiamondsFarFromPlayer() {
        return System.Array.FindAll(_diamonds, d => { // lambda ;)
            float distance = Vector3.Distance(d.transform.position, _player.position);
            return !d.activeSelf && distance >= _minimumSpawnDistance; // if inactive and far enough
        });
    }
}