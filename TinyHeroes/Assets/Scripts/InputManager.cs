using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (InputSystem.devices.OfType<Gamepad>().Count() > 1)
        {
            Debug.Log("Multiple gamepads connected");
        }
    }
}
