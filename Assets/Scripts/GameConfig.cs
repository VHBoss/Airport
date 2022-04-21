using System;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "GameConfig", order = 1)]
public class GameConfig : ScriptableObject
{
    public static Action<GameConfig> SettingsChanged;

    [Header("Camera settings")]
    public Vector3 CameraRotation = new Vector3(50, 0, 0);
    public float CameraZoom = 22;
    public float CameraFOV = 45f;

    [Header("Car settings")]
    public float CarMoveSpeed = 7;
    public float CarRotationTime = 0.15f;

    [Header("Trailers settings")]
    public float TrailerDistance = 1.6f;
    public int MaxTrailers = 2;
    public int StartTrailers = 2;
    public int MaxSuitcaseInTrail = 8;
    public float SuitcaseAnimationTime = 0.3f;

    [Header("Body tilt")]
    public float TiltSmoothTime = 0.05f;
    public float TiltMaxAngle = 15f;
    public float TiltAngleMultiplier = 10f;

    [Header("Suitcase terminal")]
    public float NewCaseEverySec = 1f;
    public float CaseMoveTime = 2f;
    public float CaseJumpTime = 0.5f;

    [Header("Airplane trap")]
    public float TrapUploadTime = 0.5f;
    //public float TrapMoveTime = 1f;

    public void UpdateSettings()
    {
        SettingsChanged?.Invoke(this);
    }

    private void OnValidate()
    {
        UpdateSettings();
    }
}
