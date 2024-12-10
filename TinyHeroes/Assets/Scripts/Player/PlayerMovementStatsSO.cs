using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementStats", menuName = "Scriptable Objects/PlayerMovementStats")]
public class PlayerMovementStatsSO : ScriptableObject
{
    [Header("Movement Properties")]
    [Range(1f, 100f)] public float MaxWalkSpeed = 12.5f;
    [Range(1f, 100f)] public float MaxRunSpeed = 12.5f;
    [Range(0.25f, 50f)] public float GroundAcceleration = 5f;
    [Range(0.25f, 50f)] public float GroundDecceleration = 20f;
    [Range(0.25f, 50f)] public float AirAcceleration = 5f;
    [Range(0.25f, 50f)] public float AirDecceleration = 5f;

    [Header("Jump Properties")]
    public float JumpHeight = 6.5f;
    public float MaxFallSpeed = 26f;
    [Range(1f, 1.1f)] public float JumpHeightCompensationFactor = 1.054f;
    [Range(0.01f, 5f)] public float GravityOnReleaseMultiplier;
    [Range(1, 5)] public int NumberOfJumpsAllowed = 2;

    [Header("Jump Cut")]
    [Range(0.02f, 0.3f)] public float TimeForUpwardsCancel = 0.027f;

    [Header("Jump Apex")]
    public float TimeTillJumpApex = 0.35f;
    [Range(0.5f, 1f)] public float ApexThreshold = 0.97f;
    [Range(0.01f, 1f)] public float ApexHangTime = 0.075f;

    [Header("Jump Buffer")]
    [Range(0f, 1f)] public float JumpBufferTime = 0.125f;

    [Header("Jump Coyote Time")]
    [Range(0f, 1f)] public float JumpCoyoteTime = 0.1f; 

    [Header("Collision Checks")]
    public LayerMask GroundLayer;
    public float GroundDetectionRayLength = 0.02f;
    public float HeadDetectionRayLength = 0.02f;
    [Range(0f, 1f)] public float HeadWidth = 0.75f;

    [HideInInspector] public float Gravity { get; private set; }
    [HideInInspector] public float InitialJumpVelocity { get; private set; }
    [HideInInspector] public float AdjustedJumpHeight { get; private set; }

    private void OnValidate()
    {
        CalculateValues();
    }

    private void OnEnable()
    {
        CalculateValues();
    }

    private void CalculateValues()
    {
        AdjustedJumpHeight = JumpHeight * JumpHeightCompensationFactor;
        Gravity = -(2f * AdjustedJumpHeight) / Mathf.Pow(TimeTillJumpApex, 2f);
        InitialJumpVelocity = Mathf.Abs(Gravity) * TimeTillJumpApex;
    }

}
