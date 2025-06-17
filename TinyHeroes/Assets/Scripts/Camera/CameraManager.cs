using System.Collections;
using Unity.Cinemachine;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    public static CameraManager Instance { get; private set; }

    [SerializeField] private CinemachineCamera _singleplayerCamera;
    [SerializeField] private CinemachineCamera _multiplayerCamera;

    [Header("Player jump/fall camera controls")]
    [SerializeField] private float _fallPanAmount = 0.25f;
    [SerializeField] private float _fallPanYTime = 0.35f;
    public float fallSpeedYDampingChangeThreshold = -15f;

    [Header("Camera Shake")]
    [SerializeField] private CinemachineImpulseListener _impulseListener;
    [SerializeField] private CameraShakeProfileSO _globalCameraShakeProfile;

    [HideInInspector] public bool isLerpingYDamping;
    [HideInInspector] public bool lerpedFromPlayerFalling;

    private float _normYPanAmount;

    private CinemachineCamera _activeCamera;
    private CinemachinePositionComposer _positionComposer;
    private CinemachineTargetGroup _targetGroup;

    private void Awake()
    {
        if (Instance)
        {
            Debug.LogWarning("You are using multiple camera managers across the scene. Destroying this gameobject");
            Destroy(this);
            return;
        }
        
        Instance = this;

        _activeCamera = _singleplayerCamera;
        _positionComposer = _activeCamera.GetComponent<CinemachinePositionComposer>();
        _targetGroup = _multiplayerCamera.GetComponent<CinemachineCamera>().Target.TrackingTarget.GetComponent<CinemachineTargetGroup>();

        _normYPanAmount = _positionComposer.Damping.y;

        _multiplayerCamera.gameObject.SetActive(false);
    }

    #region Lerp Y Damping

    public  void LerpYDamping(bool playerFalling)
    {
        StartCoroutine(LerpYCo(playerFalling));
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

    #region Camera Shake

    public void CameraShake(CinemachineImpulseSource impulseSource)
    {
        CameraShake(impulseSource, _globalCameraShakeProfile);
    }

    public void CameraShake(CinemachineImpulseSource impulseSource, CameraShakeProfileSO profile)
    {
        CameraShake(impulseSource, profile, 1f);
    }

    public void CameraShake(CinemachineImpulseSource impulseSource, CameraShakeProfileSO profile, float forceMultiplier)
    {
        CinemachineImpulseDefinition impulseDefinition = impulseSource.ImpulseDefinition;
        impulseDefinition.ImpulseDuration = profile.impulseTime;
        impulseDefinition.CustomImpulseShape = profile.impulseCurve;
        impulseSource.DefaultVelocity = profile.defaultVelocity;
        _impulseListener.Gain = profile.gain;
        _impulseListener.ReactionSettings.AmplitudeGain = profile.amplitude;
        _impulseListener.ReactionSettings.FrequencyGain = profile.frequency;
        _impulseListener.ReactionSettings.Duration = profile.duration;

        impulseSource.GenerateImpulseWithForce(profile.impulseForce * forceMultiplier);
    }

    #endregion

    #region Camera switching

    public void ChangeCameraToSingleplayer()
    {
        if (_activeCamera == _singleplayerCamera) return;
        _activeCamera.gameObject.SetActive(false);
        _singleplayerCamera.gameObject.SetActive(true);
        _activeCamera = _singleplayerCamera;
        _positionComposer = _activeCamera.GetComponent<CinemachinePositionComposer>();
    }

    public void ChangeCameraToMultiplayer()
    {
        if (_activeCamera == _multiplayerCamera) return;
        _activeCamera.gameObject.SetActive(false);
        _multiplayerCamera.gameObject.SetActive(true);
        _activeCamera = _multiplayerCamera;
        _positionComposer = _activeCamera.GetComponent<CinemachinePositionComposer>();
    }

    public void SetPlayerTarget(Transform player)
    {
        _singleplayerCamera.Target.TrackingTarget.GetComponent<CameraPlayerFollow>().SetTarget(player);
    }

    public void AddPlayerTarget(Transform player)
    {
        _targetGroup.AddMember(player, 1f, 1f);
    }

    #endregion
}
