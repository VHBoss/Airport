using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private Transform m_Target;

    public Vector3 m_TargetOffset;

    private void Start()
    {
        m_Target = GameObject.FindGameObjectWithTag("Player").transform;
    }

    void LateUpdate()
    {
        transform.position = m_Target.position + m_TargetOffset;
    }
}
