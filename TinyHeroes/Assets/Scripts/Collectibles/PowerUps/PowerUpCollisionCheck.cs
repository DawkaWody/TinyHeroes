using UnityEngine;

public class PowerUpCollisionCheck : MonoBehaviour
{
    public string powerUpName;

    private Sprite _powerUpIcon;
    private SpawnPointManager _spawnPointManager;

    private bool _collected;

    void Start()
    {
        _powerUpIcon = GetComponentInChildren<SpriteRenderer>().sprite;
        _spawnPointManager = SpawnPointManager.Instance;
        _collected = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag(GLOBALS.playerTag) && !_collected)
        {
            _collected = true;
            PlayerPowerupController playerPowerupController = other.GetComponentInParent<PlayerPowerupController>();
            UiManager.Instance.ShowPowerUp(other.GetComponentInParent<PlayerData>().index,
                playerPowerupController.GetFirstAvailableSlot() + 1, _powerUpIcon);
            playerPowerupController.CollectPowerUp(gameObject.GetComponent<IPowerUp>());

            _spawnPointManager.SetSpawnPointAvailability(gameObject.transform, true);
            transform.GetComponentInChildren<SpriteRenderer>().enabled = false;
        }
    }
}
