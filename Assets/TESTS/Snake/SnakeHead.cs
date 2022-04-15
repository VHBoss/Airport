using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SnakeHead : MonoBehaviour
{
    public float fTurnRate = 90.0f;  // 90 degrees of turning per second
    public float fSpeed = 1.0f;  // Units per second of movement;

    void Update()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
            transform.Rotate(Vector3.down * fTurnRate * Time.deltaTime);
        if (Input.GetKey(KeyCode.RightArrow))
            transform.Rotate(Vector3.up * fTurnRate * Time.deltaTime);
        transform.localPosition = transform.localPosition + transform.forward * fSpeed * Time.deltaTime;
    }
}