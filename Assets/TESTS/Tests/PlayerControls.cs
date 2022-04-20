using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float playerSpeed = 5.0f;
    public float rotationTime = 0.1f;
    public Transform Car;
    public Transform CarBody;
    public Transform WheelFL;
    public Transform WheelFR;
    public CarRaycaster Raycaster;
    public FloatingJoystick variableJoystick;

    private readonly float maxWheelAngle = 40;
    private readonly float maxBodyAngleZ = 15;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private Vector3 move;
    private bool groundedPlayer;
    private Vector3 rotationVelocity = Vector3.zero;

    private void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();
    }

    void Update()
    {
        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        move = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;

#if UNITY_EDITOR
        move += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
#endif

        Raycaster.DetectCollisions(ref move);

        if (move != Vector3.zero)
        {
            Vector3 newDirection = Vector3.SmoothDamp(Car.transform.forward, move, ref rotationVelocity, rotationTime);
            controller.Move(newDirection * Time.deltaTime * playerSpeed);
            Car.transform.forward = newDirection;
        }

        if (move != Vector3.zero)
        {
            Quaternion rot = Quaternion.LookRotation(move);
            float angle = Quaternion.Angle(rot, Car.transform.rotation);
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

        playerVelocity.y -= 10 * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);

        UpdateBody();
    }

    private void UpdateBody()
    {
        //if (move != Vector3.zero)
        //{
            float ang = Vector3.SignedAngle(Car.transform.forward, move, Vector3.up);
            ang = ang < -maxBodyAngleZ ? -maxBodyAngleZ : ang;
            ang = ang > maxBodyAngleZ ? maxBodyAngleZ : ang;
            CarBody.localEulerAngles = new Vector3(0, 0, ang);
        //}
    }
}
