using UnityEngine;

public class DiamondCollisionCheck : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.CompareTag(GLOBALS.playerTag)) {
            Destroy(gameObject);
        }
    }
}
