using UnityEngine;

[RequireComponent(typeof(PlayerAnimationController))]
[RequireComponent(typeof(PlayerInputHandler))]
public class PlayerAttackController : MonoBehaviour
{
    private PlayerAnimationController _animationController;
    private PlayerInputHandler _inputHandler;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animationController = GetComponent<PlayerAnimationController>();
        _inputHandler = GetComponent<PlayerInputHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_inputHandler.Attack1WasPressed)
        {
            Debug.Log("attack");
        }
    }
}
