using UnityEngine;

public class PowerUpCollisionCheck : MonoBehaviour
{
    public string powerUpName;
    public Sprite powerUpIcon;
    private SpawnPointManager _spawnPointManager;
    private PowerUpManager _powerUpManager;

    void Start()
    {
        _spawnPointManager = SpawnPointManager.Instance;
        _powerUpManager = FindObjectOfType<PowerUpManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(GLOBALS.playerTag)) 
        {
            Debug.Log("more yes");
            _powerUpManager.CollectPowerUp(this, powerUpIcon);

            _spawnPointManager.SetSpawnPointAvailability(gameObject.transform, true);
            Destroy(gameObject);
        }
    }
}
