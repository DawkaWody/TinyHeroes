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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(GLOBALS.playerTag))
        {
            Debug.Log("more yes");
            PlayerPowerupController playerPowerupController = other.GetComponentInParent<PlayerPowerupController>();
            UiManager.Instance.ShowPowerUp(other.GetComponentInParent<PlayerData>().index,
                playerPowerupController.GetFirstAvailableSlot() + 1, powerUpIcon);
            playerPowerupController.CollectPowerUp(gameObject.GetComponent<IPowerUp>());

            _spawnPointManager.SetSpawnPointAvailability(gameObject.transform, true);
            Destroy(gameObject);
        }
    }
}
