using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpherizeTransform : MonoBehaviour
{
    private Vector3 m_StartPosition;

    private void Awake()
    {
        SpherizeWorld.OnSpherize += Spherize;

        m_StartPosition = transform.position;
    }

    private void OnDestroy()
    {
        SpherizeWorld.OnSpherize -= Spherize;
    }

    void Spherize()
    {
        transform.position = SpherizeWorld.Coords(m_StartPosition);
        Vector3 view = transform.position - new Vector3(0, -SpherizeWorld.Radius, 0);
        transform.rotation = Quaternion.LookRotation(view);
    }
}
