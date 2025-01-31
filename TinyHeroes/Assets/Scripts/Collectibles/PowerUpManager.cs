using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PowerUpManager : MonoBehaviour
{
    public Image powerUpSlot1;
    public Image powerUpSlot2;

    private IPowerUp[] powerupSlots = new IPowerUp[2]; 
    private int currentSlot = 0;

    private PlayerInputHandler _inputHandler;

    void Start() 
    {
        _inputHandler = FindObjectOfType<PlayerInputHandler>();

        powerUpSlot1.gameObject.SetActive(false);
        powerUpSlot2.gameObject.SetActive(false);
    }

    void Update() 
    {
        if (_inputHandler.PowerUp1WasPressed) 
        {
            UsePowerUp(0);
        }
        else if (_inputHandler.PowerUp2WasPressed) 
        {
            UsePowerUp(1);
        }    
    }

    public void CollectPowerUp(PowerUpCollisionCheck powerup, Sprite powerupIcon) 
    {
        if (currentSlot >= 2) return;

        if(powerUpSlot1.gameObject.activeSelf == false)
        {
            powerUpSlot1.sprite = powerupIcon;
            powerUpSlot1.gameObject.SetActive(true);
            powerupSlots[0] = powerup.GetComponent<IPowerUp>();
        }
        else if (powerUpSlot2.gameObject.activeSelf == false) 
        {
            powerUpSlot2.sprite = powerupIcon;
            powerUpSlot2.gameObject.SetActive(true);
            powerupSlots[1] = powerup.GetComponent<IPowerUp>();
        }
        currentSlot++;
    }

    public void UsePowerUp(int slot) 
    {
        if (slot == 0 && powerupSlots[0] != null) 
        {
            powerupSlots[0].Use();
            powerUpSlot1.gameObject.SetActive(false);
            powerupSlots[0] = null;
            currentSlot--;
        }
        else if (slot == 1 && powerupSlots[1] != null) 
        {
            powerupSlots[1].Use();
            powerUpSlot2.gameObject.SetActive(false);
            powerupSlots[1] = null;
            currentSlot--;
        }
    }
}