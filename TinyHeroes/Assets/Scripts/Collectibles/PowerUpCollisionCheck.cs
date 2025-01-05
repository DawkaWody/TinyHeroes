using UnityEngine;

public class PowerUpCollisionCheck : MonoBehaviour
{
    public string powerUpName;
    public Sprite powerUpIcon;
    private SpawnPointManager _spawnPointManager;

    void Start()
    {
        _spawnPointManager = SpawnPointManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("yes");
        if (collision.gameObject.CompareTag(GLOBALS.playerTag)) {
            Debug.Log("more yes");
            _spawnPointManager.SetSpawnPointAvailability(gameObject.transform, true);
            Destroy(gameObject);
        }
    }
}
