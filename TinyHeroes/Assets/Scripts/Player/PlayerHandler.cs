using UnityEngine;

public class PlayerHandler : MonoBehaviour
{
    public bool HasBomb = false;

    public void HandleExplosion() {
        Debug.Log(gameObject.name + " exploded!");

        TriggerExplosionEffect();

        gameObject.SetActive(false);
    }

    // for visuals later
    private void TriggerExplosionEffect() {
        Debug.Log("Explosion effect triggered for " + gameObject.name);
    }
}
