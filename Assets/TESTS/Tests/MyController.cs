using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyController : MonoBehaviour
{
    public float moveSpeed = 8.0f;
    //public float rotationSpeed = 0.1f;
    public float rotationTime = 6f;
    public Transform WheelFL;
    public Transform WheelFR;
    public FloatingJoystick variableJoystick;

    private float vehicle_speed;  // in meters per second
    private float wheel_radius;   // in meters

    private readonly float maxWheelAngle = 40;
    private Rigidbody m_Rigidbody;
    private Vector3 move;
    private Vector3 velocity = Vector3.zero;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        move = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;

#if UNITY_EDITOR
        move += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
#endif

        if (move != Vector3.zero)
        {
            //Move
            m_Rigidbody.velocity = transform.forward * moveSpeed;

            //Rotation
            Vector3 newDirection = Vector3.SmoothDamp(transform.forward, move, ref velocity, rotationTime);
            transform.rotation = Quaternion.LookRotation(newDirection);

            //Wheel Rotation
            Quaternion rot = Quaternion.LookRotation(move);
            float angle = Quaternion.Angle(rot, transform.rotation);
            if(angle < maxWheelAngle)
            {
                WheelFL.rotation = WheelFR.rotation = rot;
            }


            //Quaternion localSpaceRotation = WheelFL.rotation * transform.eulerAngles;
            //WheelFL.localRotation = WheelFR.localRotation = localSpaceRotation;
            //float smoothAngle = Mathf.LerpAngle(transform.eulerAngles.y, -turnAngle, rotationSpeed * Time.deltaTime);
            //WheelFL.rotation = WheelFR.rotation = Quaternion.eu(0, turnAngle, 0);


            //Vector3 r = Vector3.RotateTowards(transform.forward, move, rotationSpeed * Time.deltaTime, 0);
            //WheelFL.localEulerAngles = WheelFR.localEulerAngles = r;

            //Vector3 angle = WheelFL.eulerAngles;
            //a = angle.y;
            //angle.y = Mathf.Clamp(angle.y, -maxWheelAngle, maxWheelAngle);
            //WheelFL.localEulerAngles = WheelFR.localEulerAngles = angle;


            //WheelFL.localRotation = WheelFR.localRotation = Quaternion.LookRotation(move);
            //Debug.DrawRay(WheelFL.position, wheelsDirection, Color.green, 1);
        }

        //UpdateWheels();
    }


    float a;

    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 300, 20), a.ToString());
    }

    //private void UpdateWheels()
    //{
    //    var wheel_circumference = 2 * Mathf.PI * wheel_radius; // still in meters
    //    var rot_per_second = vehicle_speed / wheel_circumference;
    //}

    //    private void FixedUpdate()
    //    {
    //        //Vector3 move = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;

    //        //#if UNITY_EDITOR
    //        Vector3 move = new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
    ////#endif

    //        if (move != Vector3.zero)
    //        {
    //            Vector3 forward = Vector3.SmoothDamp(transform.forward, move, ref m_CurrentVelocity, rotationTime * Time.fixedDeltaTime);

    //            Quaternion toRotation = Quaternion.LookRotation(forward.normalized, Vector3.up);
    //            Quaternion rotation = Quaternion.RotateTowards(m_Rigidbody.rotation, toRotation, rotationSpeed * Time.fixedDeltaTime);
    //            m_Rigidbody.MoveRotation(rotation);

    //            m_Rigidbody.MovePosition(transform.position + forward.normalized * Time.fixedDeltaTime * moveSpeed);
    //        } 
    //    }
}
