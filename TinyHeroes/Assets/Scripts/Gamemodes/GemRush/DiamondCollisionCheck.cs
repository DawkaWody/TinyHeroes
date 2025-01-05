using UnityEngine;

public class DiamondCollisionCheck : MonoBehaviour
{
    private SpawnPointManager _spawnPointManager;

    void Start()
    {
        _spawnPointManager = SpawnPointManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag(GLOBALS.playerTag)) {
            _spawnPointManager.SetSpawnPointAvailability(gameObject.transform, true);
            other.GetComponentInParent<PlayerHandler>().diamonds++;
            gameObject.SetActive(false);
        }
    }
}
