using UnityEngine;
[RequireComponent(typeof(PlayerHandler))]
public class Bomb : MonoBehaviour
{
    public float countdown = 10f;
    [SerializeField] private float transferCooldown = 1f;
    private float timer;
    private float lastTransferTime = -Mathf.Infinity; // definitely not now

    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;

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

        Vector3 offset = new Vector3(offsetX, offsetY, 0f); // offset zby bomba nie byla na ryju
        transform.position = currentPlayer.transform.position + offset;
        transform.SetParent(currentPlayer.transform);

        lastTransferTime = Time.time;
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

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.CompareTag("Player"))
        {
            PlayerHandler otherPlayer = collision.GetComponentInParent<PlayerHandler>();
            if (otherPlayer != null && otherPlayer != currentPlayer && Time.time >= lastTransferTime + transferCooldown)
            {
                AttachToPlayer(otherPlayer);
            }
        }
    }
}
