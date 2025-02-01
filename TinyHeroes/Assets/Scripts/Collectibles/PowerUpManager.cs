using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpManager : MonoBehaviour
{
    public Image powerUpSlot1;
    public Image powerUpSlot2;

    private PlayerInputHandler _inputHandler;

    void Start() 
    {
        _inputHandler = FindObjectOfType<PlayerInputHandler>();

        powerUpSlot1.gameObject.SetActive(false);
        powerUpSlot2.gameObject.SetActive(false);
    }

    void Update() 
    {
    }

    public void CollectPowerUp(PlayerPowerupController player, PowerUpCollisionCheck powerup, Sprite powerupIcon) 
    {
        player.CollectPowerUp(powerup.GetComponent<IPowerUp>());
        Debug.Log("Powerup collected");
        // UI póŸniej (trzeba zrobiæ nowy skrypt)
    }
}