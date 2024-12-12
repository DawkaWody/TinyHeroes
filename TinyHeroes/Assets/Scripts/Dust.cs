using UnityEngine;

[RequireComponent(typeof(Animator))]
public class Dust : MonoBehaviour
{
    [SerializeField] private string _typeIntName = "Type";
    [SerializeField] private string _playTriggerName = "play";

    private int _typeIntId;
    private int _playTriggerId;
    private bool _playedAnimation;

    private Animator _animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        _animator = GetComponent<Animator>();

        _typeIntId = Animator.StringToHash(_typeIntName);
        _playTriggerId = Animator.StringToHash(_playTriggerName);
    }

    private void Update()
    {
        if (_playedAnimation)
        {
            if (_animator.GetCurrentAnimatorStateInfo(0).IsName("None"))
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetType(int type)
    {
        // 0 - Double jump dust, 1 - Run/Push dust
        _animator.SetInteger(_typeIntId, type);
    }

    public void Play()
    {
        _animator.SetTrigger(_playTriggerId);
        _playedAnimation = true;
    }
}
