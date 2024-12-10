using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerAnimationController : MonoBehaviour
{
    [Header("Animator Parameters")]
    [SerializeField] private string _movingBoolName = "isMoving";
    [SerializeField] private string _runningBoolName = "isRunning";
    [SerializeField] private string _landTriggerName = "land";
    [SerializeField] private string _jumpTriggerName = "jump";
    [SerializeField] private string _attack1TriggerName = "attack1";
    [SerializeField] private string _attack2TriggerName = "attack2";

    [Header("Animator States")]
    [SerializeField] private string _jumpStateName = "Jump";
    [SerializeField] private string _inAirStateName = "InAir";

    private int _movingBoolId;
    private int _runningBoolId;
    private int _landTriggerId;
    private int _jumpTriggerId;
    private int _attack1TriggerId;
    private int _attack2TriggerId;

    private Animator _animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _animator = GetComponent<Animator>();

        _movingBoolId = Animator.StringToHash(_movingBoolName);
        _runningBoolId = Animator.StringToHash(_runningBoolName);
        _landTriggerId = Animator.StringToHash(_landTriggerName);
        _jumpTriggerId = Animator.StringToHash(_jumpTriggerName);
        _attack1TriggerId = Animator.StringToHash(_attack1TriggerName);
        _attack2TriggerId = Animator.StringToHash(_attack2TriggerName);
    }

    public void AnimateMovement(bool isMoving, bool isRunning)
    {
        _animator.SetBool(_movingBoolId, isMoving);
        _animator.SetBool(_runningBoolId, isRunning);
    }

    public void AnimateAttack(int variant)
    {
        if (variant == 1)
        {
            _animator.SetTrigger(_attack1TriggerId);
        }
        else if (variant == 2)
        {
            _animator.SetTrigger(_attack2TriggerId);
        }
    }

    public void InitiateJump()
    {
        _animator.SetTrigger(_jumpTriggerId);
    }

    public void Land()
    {
        _animator.SetTrigger(_landTriggerId);
    }

    public bool IsInAirOrJumping()
    {
        AnimatorStateInfo state = _animator.GetCurrentAnimatorStateInfo(0);
        return state.IsName(_jumpStateName) || state.IsName(_inAirStateName);
    }
}
