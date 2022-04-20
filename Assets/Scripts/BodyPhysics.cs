using System;
using UnityEngine;

public class BodyPhysics : MonoBehaviour
{
    private float smoothTime = 0.05f;
    private float yVelocity = 0.0f;
    private float angleMultiplier = 10f;

    private float maxBodyAngleZ = 15;

    private Vector3 m_PrevPosition;
    private Vector3 m_PrevDirection;
    private float m_Angle;

    private void Awake()
    {
        m_PrevPosition = transform.position;
        GameConfig.SettingsChanged += OnSettingsChanged;
    }

    private void OnDestroy()
    {
        GameConfig.SettingsChanged -= OnSettingsChanged;
    }

    private void OnSettingsChanged(GameConfig config)
    {
        smoothTime = config.TiltSmoothTime;
        maxBodyAngleZ = config.TiltMaxAngle;
        angleMultiplier = config.TiltAngleMultiplier;
    }

    void LateUpdate()
    {
        if (m_PrevPosition != transform.position)
        {
            m_Angle = -Vector3.SignedAngle(transform.forward, m_PrevDirection, Vector3.up) * angleMultiplier;
            m_Angle = m_Angle < -maxBodyAngleZ ? -maxBodyAngleZ : m_Angle;
            m_Angle = m_Angle > maxBodyAngleZ ? maxBodyAngleZ : m_Angle;

            m_PrevPosition = transform.position;
            m_PrevDirection = transform.forward;            
        }
        else if (m_Angle != 0)
        {
            m_Angle = 0;
        }

        float newAngle = Mathf.SmoothDampAngle(transform.localEulerAngles.z, m_Angle, ref yVelocity, smoothTime);
        transform.localEulerAngles = new Vector3(0, 0, newAngle);
    }
}
