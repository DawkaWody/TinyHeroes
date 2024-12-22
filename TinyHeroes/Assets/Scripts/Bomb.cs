using UnityEngine;
[RequireComponent(typeof(PlayerHandler))]
public class Bomb : MonoBehaviour
{
    public float countdown = 10f;
    private float timer;
    private PlayerHandler currentPlayer;
    private BombTagManager bombTagManager;

    void Start() {
        timer = countdown;
        bombTagManager = FindFirstObjectByType<BombTagManager>();
    }

    void Update() {
        timer -= Time.deltaTime;

        if (timer <= 0) {
            Explode();
        }
    }

    public void AttachToPlayer(PlayerHandler player) {
        if (currentPlayer != null) {
            currentPlayer.HasBomb = false;
            transform.SetParent(null);
        }

        currentPlayer = player;
        currentPlayer.HasBomb = true;

        transform.position = currentPlayer.transform.position;
        transform.SetParent(currentPlayer.transform);
        timer = countdown;
    }

    private void Explode() 
    {
        if (currentPlayer != null) {
            currentPlayer.HandleExplosion(); 
        }

        if (bombTagManager != null && bombTagManager.GetRemainingPlayersCount() > 1) {
            timer = countdown;
            AttachToPlayer(bombTagManager.GetRandomPlayer());
        }
        else {
            Debug.Log("Game Over! Only one player left.");
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision) 
    {
        if (collision.CompareTag("Player")) {
            PlayerHandler otherPlayer = collision.GetComponent<PlayerHandler>();
            if (otherPlayer != null && otherPlayer != currentPlayer) { // npc nie spe³nia warunku otherPlayer != null, pozniej to naprawie
                Debug.Log("Yes!");
                AttachToPlayer(otherPlayer);
            } 
        }
    }
}
