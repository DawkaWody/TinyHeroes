using UnityEngine;
public class BombTagManager : MonoBehaviour
{
    public GameObject bombPrefab;
    private PlayerHandler[] players;

    void Start() {
        players = GameObject.FindObjectsByType<PlayerHandler>(FindObjectsSortMode.None);
        if (players.Length > 0) {
            AssignBombToRandomPlayer();
        }
    }

    public int GetRemainingPlayersCount() {
        int count = 0;
        foreach (PlayerHandler player in players) {
            if (player.gameObject.activeSelf) // check if player stiill active
            {
                count++;
            }
        }
        return count;
    }

    public PlayerHandler GetRandomPlayer() 
    {
        PlayerHandler[] activePlayers = System.Array.FindAll(players, p => p.gameObject.activeSelf);

        if (activePlayers.Length > 0) {
            int randomIndex = Random.Range(0, activePlayers.Length);
            return activePlayers[randomIndex];
        }
        return null;
    }

    private void AssignBombToRandomPlayer() 
    {
        int randomIndex = Random.Range(0, players.Length);
        PlayerHandler randomPlayer = players[randomIndex];

        GameObject bomb = Instantiate(bombPrefab, randomPlayer.transform.position, Quaternion.identity);
        Bomb bombComponent = bomb.GetComponent<Bomb>();

        bombComponent.AttachToPlayer(randomPlayer);
    }
}
