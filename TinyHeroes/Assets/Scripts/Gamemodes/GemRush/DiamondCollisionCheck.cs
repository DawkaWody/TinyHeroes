using UnityEngine;

public class DiamondCollisionCheck : MonoBehaviour
{ 
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag(GLOBALS.playerTag)) {
            SpawnPointManager.Instance.SetSpawnPointAvailability(gameObject.transform, true);
            other.GetComponentInParent<PlayerHandler>().diamonds++;
            gameObject.SetActive(false);
        }
    }
}
