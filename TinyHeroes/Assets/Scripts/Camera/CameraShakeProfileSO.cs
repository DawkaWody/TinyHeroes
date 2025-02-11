using UnityEngine;

[CreateAssetMenu(fileName = "CameraShakeProfile", menuName = "Scriptable Objects/CameraShakeProfile")]
public class CameraShakeProfileSO : ScriptableObject
{
    [Header("Impulse Source Settings")] 
    public float impulseTime = 0.2f;
    public float impulseForce = 1f;
    public Vector3 defaultVelocity = new Vector3(0f, -1f);
    public AnimationCurve impulseCurve;

    [Header("Impulse Listener Settings")] 
    public float amplitude = 1f;
    public float frequency = 1f;
    public float duration = 1f;
}
