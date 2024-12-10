using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputHandler : MonoBehaviour
{
    [SerializeField] private string _moveActionName;
    [SerializeField] private string _jumpActionName;
    [SerializeField] private string _runActionName;
    [SerializeField] private string _attack1ActionName;

    [HideInInspector] public Vector2 Movement;
    [HideInInspector] public bool JumpWasPressed;
    [HideInInspector] public bool JumpIsHeld;
    [HideInInspector] public bool JumpWasReleased;
    [HideInInspector] public bool RunIsHeld;
    [HideInInspector] public bool Attack1WasPressed;

    private InputAction _moveAction;
    private InputAction _jumpAction;
    private InputAction _runAction;
    private InputAction _attack1Action;

    private PlayerInput _playerInput;

    private void Awake()
    {
        _playerInput = GetComponent<PlayerInput>();

        _moveAction = _playerInput.actions[_moveActionName];
        _jumpAction = _playerInput.actions[_jumpActionName];
        _runAction = _playerInput.actions[_runActionName];
        _attack1Action = _playerInput.actions[_attack1ActionName];
    }

    private void Update()
    {
        Movement = _moveAction.ReadValue<Vector2>();

        JumpWasPressed = _jumpAction.WasPressedThisFrame();
        JumpIsHeld = _jumpAction.IsPressed();
        JumpWasReleased = _jumpAction.WasReleasedThisFrame();

        Attack1WasPressed = _attack1Action.WasPressedThisFrame();

        RunIsHeld = _runAction.IsPressed();
    }
}
