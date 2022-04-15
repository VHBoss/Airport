using UnityEngine;

public class CarController : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float rotationTime = 0.1f;
    public Transform WheelFL;
    public Transform WheelFR;
    public Transform Body;
    public FloatingJoystick variableJoystick;

    public float Velocity => m_Rigidbody.velocity.magnitude;

    private readonly float maxWheelAngle = 40;
    private readonly float maxBodyAngleZ = 15;
    //private readonly float maxBodyAngleX = 5;
    private Rigidbody m_Rigidbody;
    private Vector3 move;
    private float angle;
    //private float prevSpeed;
    //private float bodyVelocity;
    private Vector3 rotationVelocity = Vector3.zero;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        UpdateMoving();
        //UpdateBody();
    }

    private void UpdateMoving()
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
            Vector3 newDirection = Vector3.SmoothDamp(transform.forward, move, ref rotationVelocity, rotationTime);
            transform.rotation = Quaternion.LookRotation(newDirection);

            //Wheel Rotation
            Quaternion rot = Quaternion.LookRotation(move);
            angle = Quaternion.Angle(rot, transform.rotation);
            if (angle < maxWheelAngle)
            {
                WheelFL.rotation = WheelFR.rotation = rot;
            }
            //UpdateBody();
        }
        else
        {
            //m_Rigidbody.velocity = Vector3.zero;
            WheelFL.localRotation = WheelFR.localRotation = Quaternion.identity;
        }
        UpdateBody();
    }

    private void UpdateBody()
    {
        if (move != Vector3.zero)
        {
            float ang = Vector3.SignedAngle(transform.forward, move, Vector3.up);
            ang = ang < -maxBodyAngleZ ? -maxBodyAngleZ : ang;
            ang = ang > maxBodyAngleZ ? maxBodyAngleZ : ang;
            Body.localEulerAngles = new Vector3(0, 0, ang);
        }
    }

    //private float x, y;
    //private Vector3 a, b;
    //private void OnGUI()
    //{
    //    GUI.Label(new Rect(10, 10, 100, 20), x.ToString());
    //    GUI.Label(new Rect(10, 30, 100, 20), y.ToString());
    //    //GUI.Label(new Rect(10, 10, 100, 20), a.ToString());
    //    //GUI.Label(new Rect(10, 30, 100, 20), b.ToString());
    //}
}
