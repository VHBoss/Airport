using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirTrap : MonoBehaviour
{
    [SerializeField] private float m_LoadTime = 0.1f;
    [SerializeField] private float m_TrapSpeed = 0.5f;
    [SerializeField] private Transform m_StartPoint;
    [SerializeField] private Transform m_EndPoint;

    private TrailerController m_TtrailerController;

    private void Awake()
    {
        GameConfig.SettingsChanged += OnSettingsChanged;
    }

    private void OnDestroy()
    {
        GameConfig.SettingsChanged -= OnSettingsChanged;
    }

    private void OnSettingsChanged(GameConfig config)
    {
        m_LoadTime = config.TrapUploadTime;
        m_TrapSpeed = config.TrapMoveTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            m_TtrailerController = other.GetComponent<TrailerController>();
            InvokeRepeating("StartProcess", 0, m_LoadTime);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        CancelInvoke();
    }

    private void StartProcess()
    {
        Case item = m_TtrailerController.GetItem();

        if (item != null)
        {
            item.Unload(m_StartPoint, m_EndPoint, m_TrapSpeed);
        }
        else
        {
            CancelInvoke();
        }
    }
}
