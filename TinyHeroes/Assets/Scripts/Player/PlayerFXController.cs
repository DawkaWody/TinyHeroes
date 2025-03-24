using UnityEngine;
using Unity.Cinemachine;

[RequireComponent(typeof(CinemachineImpulseSource))]
public class PlayerFXController : MonoBehaviour
{
    [SerializeField] private GameObject _dustFxPrefab;
    [SerializeField] private GameObject _attackEffect;
    [SerializeField] private CameraShakeProfileSO _hitShakeEffect;
    [SerializeField] private GameObject _speedBoostEffect;

    private CinemachineImpulseSource _impulseSource;

    void Start()
    {
        _impulseSource = GetComponent<CinemachineImpulseSource>();
    }

    public void PlayDoubleJumpDust(Vector2 position)
    {
        GameObject dustFx = Instantiate(_dustFxPrefab, position, Quaternion.identity);
        Dust dust = dustFx.GetComponent<Dust>();
        dust.SetType(0);
        dust.Play();
    }

    public void PlayRunDust(Vector2 position)
    {
        GameObject dustFx = Instantiate(_dustFxPrefab, position, Quaternion.identity);
        Dust dust = dustFx.GetComponent<Dust>();
        dust.SetType(1);
        dust.Play();
    }

    public void PlayAttackEffect(Vector2 position) 
    {
        Instantiate(_attackEffect, position, Quaternion.identity);
    }

    public void PlayHitEffect(int comboStage)
    {
        CameraManager.Instance.CameraShake(_impulseSource, _hitShakeEffect, GLOBALS.hitShakeForceMultipliers[comboStage - 1]);
    }

    public GameObject PlaySpeedBoostEffect(Vector2 position)
    {
        GameObject particleEffect = Instantiate(_speedBoostEffect, position, Quaternion.identity);

        return particleEffect;
    }
}
