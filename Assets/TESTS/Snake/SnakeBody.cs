using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeBody : MonoBehaviour
{
    public GameObject goLeader = null;
    public float offset = 5;

    void Update()
    {
        if (goLeader == null) return;
        Vector3 v3FromLeader = transform.position - goLeader.transform.position;
        v3FromLeader = v3FromLeader.normalized;
        transform.position = v3FromLeader + goLeader.transform.position + goLeader.transform.forward * offset;
    }
}