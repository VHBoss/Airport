using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform m_Rotation;
    [SerializeField] private Transform m_Zoom;

    private Transform m_Target;
    private Camera m_Camera;

    //public Vector3 m_TargetOffset;

    private void Awake()
    {
        GameConfig.SettingsChanged += OnSettingsChanged;
        m_Camera = GetComponentInChildren<Camera>();
    }

    private void OnDestroy()
    {
        GameConfig.SettingsChanged -= OnSettingsChanged;
    }

    private void OnSettingsChanged(GameConfig config)
    {
        m_Rotation.localEulerAngles = config.CameraRotation;
        m_Zoom.localPosition = new Vector3(0, 0, -config.CameraZoom);
        m_Camera.fieldOfView = config.CameraFOV;
    }

    private void Start()
    {
        m_Target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        transform.position = m_Target.position;
    }
}
