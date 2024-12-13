using UnityEngine;

public class DiamondSpawner : MonoBehaviour
{
    [SerializeField] private float _spawnRate;
    [SerializeField] private GameObject[] _diamonds;

    private float _spawnTimer;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach (GameObject d in _diamonds)
        {
            d.SetActive(false);
        }

        _spawnTimer = _spawnRate;
    }

    // Update is called once per frame
    void Update()
    {
        _spawnTimer -= Time.deltaTime;
        if (_spawnTimer <= 0)
        {
            _diamonds[Random.Range(0, _diamonds.Length)].SetActive(true);
            _spawnTimer = _spawnRate;
        }
    }
}
