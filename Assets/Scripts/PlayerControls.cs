using UnityEngine;

public class PlayerControls : MonoBehaviour
{
    public float playerSpeed = 5.0f;
    public float rotationTime = 0.1f;
    public float breakSpeed = 0.96f;
    public Transform Car;
    public Transform WheelFL;
    public Transform WheelFR;
    public CarRaycaster Raycaster;
    public FloatingJoystick variableJoystick;

    private readonly float maxWheelAngle = 40;

    private CharacterController controller;
    private Vector3 playerVelocity;
    private Vector3 move;
    private bool groundedPlayer;
    private Vector3 rotationVelocity = Vector3.zero;

    private void Awake()
    {
        controller = gameObject.GetComponent<CharacterController>();
        GameConfig.SettingsChanged += OnSettingsChanged;
    }

    private void OnDestroy()
    {
        GameConfig.SettingsChanged -= OnSettingsChanged;
    }

    private void OnSettingsChanged(GameConfig config)
    {
        playerSpeed = config.CarMoveSpeed;
        rotationTime = config.CarRotationTime;
        breakSpeed = config.CarBreakSpeed;
    }

    void Update()
    {
        move = Vector3.forward * variableJoystick.Vertical + Vector3.right * variableJoystick.Horizontal;

#if UNITY_EDITOR
        move += new Vector3(Input.GetAxis("Horizontal"), 0, Input.GetAxis("Vertical"));
#endif

        Raycaster.DetectCollisions(ref move);

        if (move != Vector3.zero)
        {
            Vector3 newDirection = Vector3.SmoothDamp(Car.transform.forward, move, ref rotationVelocity, rotationTime);
            playerVelocity = newDirection * playerSpeed;
            Car.transform.forward = newDirection;

            Quaternion rot = Quaternion.LookRotation(move);
            float angle = Quaternion.Angle(rot, Car.transform.rotation);
            if (angle < maxWheelAngle)
            {
                WheelFL.rotation = WheelFR.rotation = rot;
            }
        }
        else
        {
            playerVelocity *= breakSpeed;
            WheelFL.localRotation = WheelFR.localRotation = Quaternion.identity;
        }

        groundedPlayer = controller.isGrounded;
        if (groundedPlayer && playerVelocity.y < 0)
        {
            playerVelocity.y = 0f;
        }

        playerVelocity.y -= 10 * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }
}
