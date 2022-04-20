using UnityEngine;

public class CarController : MonoBehaviour
{
    public float moveSpeed = 10.0f;
    public float rotationTime = 0.1f;
    public Transform WheelFL;
    public Transform WheelFR;
    public Transform Body;
    public Transform RaycastPoint;
    public Transform RaycastPointL;
    public Transform RaycastPointR;
    public FloatingJoystick variableJoystick;

    //public float Velocity => m_Rigidbody.velocity.magnitude;
    public float Velocity => 0;

    private readonly float maxWheelAngle = 40;
    private readonly float maxBodyAngleZ = 15;
    private Rigidbody m_Rigidbody;
    private Vector3 move;
    private Vector3 overrideMove;
    private Vector3 rotationVelocity = Vector3.zero;

    void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        UpdateMoving();
        UpdateBody();
    }

    private void UpdateMoving()
    {
        move = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;

#if UNITY_EDITOR
        move += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
#endif
        DetectCollisions();

        if (move != Vector3.zero)
        {
            //Move
            m_Rigidbody.velocity = transform.forward * moveSpeed;
            //transform.position += transform.forward * moveSpeed * Time.deltaTime;
            //transform.position += move * moveSpeed * Time.deltaTime;

            //Rotation
            Vector3 newDirection = Vector3.SmoothDamp(transform.forward, move, ref rotationVelocity, rotationTime);
            transform.rotation = Quaternion.LookRotation(newDirection);
            //transform.rotation = Quaternion.LookRotation(move);

            //Wheel Rotation
            Quaternion rot = Quaternion.LookRotation(move);
            float angle = Quaternion.Angle(rot, transform.rotation);
            if (angle < maxWheelAngle)
            {
                WheelFL.rotation = WheelFR.rotation = rot;
            }
        }
        else
        {
            //m_Rigidbody.velocity = Vector3.zero;
            WheelFL.localRotation = WheelFR.localRotation = Quaternion.identity;
        }
        Debug.DrawRay(transform.position, move * 2, Color.magenta, 1f);
    }

    private void UpdateBody()
    {
        //if (move != Vector3.zero)
        //{
            float ang = Vector3.SignedAngle(RaycastPoint.forward, move, Vector3.up);
            ang = ang < -maxBodyAngleZ ? -maxBodyAngleZ : ang;
            ang = ang > maxBodyAngleZ ? maxBodyAngleZ : ang;
            Body.localEulerAngles = new Vector3(0, 0, ang);
        //}
    }

    private void DetectCollisions()
    {
        if (Physics.Raycast(RaycastPointL.position, RaycastPointL.forward, out RaycastHit hit, 2, 1 << 4))
        {
            //RIGHT
            overrideMove = Quaternion.Euler(0, -90, 0) * hit.normal;
            Debug.DrawRay(RaycastPointL.position, RaycastPointL.forward * 2, Color.yellow, 1f);
            move = Vector3.Dot(hit.normal, move) > 0 ? move : overrideMove * move.magnitude;
        }
        else if (Physics.Raycast(RaycastPointR.position, RaycastPointR.forward, out hit, 2, 1 << 4))
        {
            //LEFT
            overrideMove = Quaternion.Euler(0, 90, 0) * hit.normal;
            Debug.DrawRay(RaycastPointR.position, RaycastPointR.forward * 2, Color.green, 1f);
            move = Vector3.Dot(hit.normal, move) > 0 ? move : overrideMove * move.magnitude;
        }
        else
        {
            Debug.DrawRay(RaycastPoint.position, RaycastPoint.forward * 2, Color.white, 1f);
        }
    }

    private void DetectCollisions3()
    {
        //if (Physics.Raycast(RaycastPoint.position, transform.forward * 2, out RaycastHit centerHit, 2, 1 << 4))
        //{
        //    //Debug.DrawRay(RaycastPoint.position, transform.forward * 2, Color.yellow, 1f);
        //    if (Physics.Raycast(RaycastPoint.position, -transform.right * 2, out RaycastHit sideHit, 2, 1 << 4))
        //    {
        //        //print("Right");                
        //        overrideMove = Quaternion.Euler(0, -90, 0) * centerHit.normal;
        //    }
        //    else
        //    {
        //        //print("Left");
        //        overrideMove = Quaternion.Euler(0, 90, 0) * centerHit.normal;
        //    }
        //    //Debug.DrawRay(centerHit.point, centerHit.normal, Color.red, 1f);
        //    //Debug.DrawRay(centerHit.point, move, Color.green, 1f);
        //    move = Vector3.Dot(centerHit.normal, move) > 0 ? move : overrideMove * move.magnitude;
        //}

        if (Physics.Raycast(RaycastPoint.position, -transform.right, out RaycastHit hit, 2, 1 << 4))
        {
            //RIGHT
            overrideMove = Quaternion.Euler(0, -90, 0) * hit.normal;
            Debug.DrawRay(RaycastPoint.position, overrideMove * 2, Color.red, 1f);
            move = Vector3.Dot(hit.normal, move) > 0 ? move : overrideMove * move.magnitude;
        }
        else if (Physics.Raycast(RaycastPoint.position, transform.right, out hit, 2, 1 << 4))
        {
            //LEFT
            overrideMove = Quaternion.Euler(0, 90, 0) * hit.normal;
            Debug.DrawRay(RaycastPoint.position, overrideMove * 2, Color.green, 1f);
            move = Vector3.Dot(hit.normal, move) > 0 ? move : overrideMove * move.magnitude;
        }


    }

    private void DetectCollisions2()
    {
        if (Physics.Raycast(RaycastPoint.position, -transform.right, out RaycastHit hit, 2, 1 << 4))
        {
            //Debug.DrawRay(RaycastPoint.position, -transform.right*2, Color.yellow, 1f);
            //vector = Quaternion.Euler(0, -90, 0) * hit.normal;
            move = Quaternion.Euler(0, -90, 0) * hit.normal;
        }
        else if (Physics.Raycast(RaycastPoint.position, transform.right, out hit, 2, 1 << 4))
        {
            //Debug.DrawRay(RaycastPoint.position, transform.right *2, Color.cyan, 1f);
            //transform.rotation = Quaternion.FromToRotation(-Vector3.right, hit.normal);
            //move = hit.normal;
            move = Quaternion.Euler(0, 90, 0) * hit.normal;
        }

        if (Physics.Raycast(RaycastPoint.position, transform.forward, out hit, 2, 1 << 4))
        {
            //Debug.DrawRay(RaycastPoint.position, transform.forward*2, Color.green, 1f);
            move = transform.right;
        }
        else
        {
            //Debug.DrawRay(RaycastPoint.position, transform.forward * 2, Color.red, 1f);
        }
    }
}
