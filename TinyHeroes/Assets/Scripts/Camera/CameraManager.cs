using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [SerializeField] private CinemachineCamera[] _cameras;

    [Header("Player jump/fall camera controls")]
    [SerializeField] private float _fallPanAmount = 0.25f;
    [SerializeField] private float _fallPanYTime = 0.35f;
    public float fallSpeedYDampingChangeThreshold = -15f;

    [HideInInspector] public bool isLerpingYDamping;
    [HideInInspector] public bool lerpedFromPlayerFalling;

    private float _normYPanAmount;
    private Vector2 _startingTrackedObjectOffest;

    private Coroutine _lerpYPanCoroutine;

    private CinemachineCamera _activeCamera;
    private CinemachinePositionComposer _positionComposer;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.LogWarning("You are using multiple camera managers across the scene. Destroying this gameobject");
            Destroy(this);
            return;
        }
        
        Instance = this;

        for (int i = 0; i < _cameras.Length; i++)
        {
            if (_cameras[i].enabled)
            {
                _activeCamera = _cameras[i];
                _positionComposer = _activeCamera.GetComponent<CinemachinePositionComposer>();
            }
        }

        _normYPanAmount = _positionComposer.Damping.y;

        _startingTrackedObjectOffest = _positionComposer.TargetOffset;
    }

    #region Lerp Y Damping

    public  void LerpYDamping(bool playerFalling)
    {
        _lerpYPanCoroutine = StartCoroutine(LerpYCo(playerFalling));
    }

    private IEnumerator LerpYCo(bool playerFalling)
    {
        isLerpingYDamping = true;

        float startDampAmount = _positionComposer.Damping.y;
        float endDampAmount = 0f;

        if (playerFalling)
        {
            endDampAmount = _fallPanAmount;
            lerpedFromPlayerFalling = true;
        }
        else
        {
            endDampAmount = _normYPanAmount;
        }

        float elapsedTime = 0f;
        while (elapsedTime < _fallPanYTime)
        {
            elapsedTime += Time.deltaTime;

            float lerpedPanAmount = Mathf.Lerp(startDampAmount, endDampAmount, elapsedTime / _fallPanYTime);
            _positionComposer.Damping = new Vector3(_positionComposer.Damping.x, lerpedPanAmount, _positionComposer.Damping.y);

            yield return null;
        }

        isLerpingYDamping = false;
    }

    #endregion
}
