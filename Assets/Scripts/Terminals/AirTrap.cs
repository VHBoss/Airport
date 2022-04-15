using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirTrap : MonoBehaviour
{
    [SerializeField] private float m_LoadTime = 0.05f;

    private TrailerController m_TtrailerController;

    void Start()
    {
        
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
            Destroy(item.gameObject);
        }
        else
        {
            CancelInvoke();
        }
    }
}
