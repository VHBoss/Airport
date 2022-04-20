using UnityEngine;

public class CarRaycaster : MonoBehaviour
{

    [SerializeField] private Transform m_Car;
    [SerializeField] private Transform m_RaycastU1;
    [SerializeField] private Transform m_RaycastU2;
    [SerializeField] private Transform m_RaycastD1;
    [SerializeField] private Transform m_RaycastD2;
    [SerializeField] private Transform m_RaycastL1;
    [SerializeField] private Transform m_RaycastL2;
    [SerializeField] private Transform m_RaycastR1;
    [SerializeField] private Transform m_RaycastR2;

    private Vector3 m_OverrideMove;

    public void DetectCollisions(ref Vector3 move)
    {
        bool Up = false, Down = false, Left = false, Right = false;

        if (Physics.Raycast(m_RaycastU1.position, m_RaycastU1.forward, out RaycastHit hitF, 2, 1 << 4) ||
            Physics.Raycast(m_RaycastU2.position, m_RaycastU2.forward, out hitF, 2, 1 << 4))
        {
            Up = true;
        }
        if (Physics.Raycast(m_RaycastD1.position, m_RaycastD1.forward, out RaycastHit hitD, 2, 1 << 4) ||
            Physics.Raycast(m_RaycastD2.position, m_RaycastD2.forward, out hitD, 2, 1 << 4))
        {
            Down = true;
        }

        if (Physics.Raycast(m_RaycastL1.position, m_RaycastL1.forward, out RaycastHit hitL, 2, 1 << 4) ||
            Physics.Raycast(m_RaycastL2.position, m_RaycastL2.forward, out hitL, 2, 1 << 4))
        {
            Left = true;
        }

        if (Physics.Raycast(m_RaycastR1.position, m_RaycastR1.forward, out RaycastHit hitR, 2, 1 << 4) ||
            Physics.Raycast(m_RaycastR2.position, m_RaycastR2.forward, out hitR, 2, 1 << 4))
        {
            Right = true;
        }

        //UP
        if (Up && !Left && !Right)
        {
            if (Vector3.SignedAngle(hitF.normal, m_Car.forward, Vector3.up) > 0)
            {
                //LEFT
                m_OverrideMove = Quaternion.Euler(0, 90, 0) * hitF.normal;
            }
            else
            {
                // RIGHT
                m_OverrideMove = Quaternion.Euler(0, -90, 0) * hitF.normal;
            }
            move = Vector3.Dot(hitF.normal, move) > 0 ? move : m_OverrideMove * move.magnitude;
        }

        //LEFT
        if (Left && !Up && !Down)
        {
            if (Vector3.SignedAngle(hitL.normal, m_Car.forward, Vector3.up) > 0)
            {
                //LEFT
                m_OverrideMove = Quaternion.Euler(0, 90, 0) * hitL.normal;
            }
            else
            {
                // RIGHT
                m_OverrideMove = Quaternion.Euler(0, -90, 0) * hitL.normal;
            }
            move = Vector3.Dot(hitL.normal, move) > 0 ? move : m_OverrideMove * move.magnitude;
        }

        //RIGHT
        if (Right && !Up && !Down)
        {
            if (Vector3.SignedAngle(hitR.normal, m_Car.forward, Vector3.up) > 0)
            {
                //LEFT
                m_OverrideMove = Quaternion.Euler(0, 90, 0) * hitR.normal;
            }
            else
            {
                // RIGHT
                m_OverrideMove = Quaternion.Euler(0, -90, 0) * hitR.normal;
            }
            move = Vector3.Dot(hitR.normal, move) > 0 ? move : m_OverrideMove * move.magnitude;
        }

        //DOWN
        if (Down && !Left && !Right)
        {
            if (Vector3.SignedAngle(hitD.normal, m_Car.forward, Vector3.up) > 0)
            {
                //LEFT
                m_OverrideMove = Quaternion.Euler(0, 90, 0) * hitD.normal;
            }
            else
            {
                // RIGHT
                m_OverrideMove = Quaternion.Euler(0, -90, 0) * hitD.normal;
            }
            move = Vector3.Dot(hitD.normal, move) > 0 ? move : m_OverrideMove * move.magnitude;
        }

        //UP LEFT CORNER
        if (Up && Left)
        {
            if (Vector3.Dot(hitL.normal, m_Car.forward) < -0.3f)
            {
                //Turn right
                m_OverrideMove = Quaternion.Euler(0, 90, 0) * hitL.normal;
                //move = Vector3.Dot(hitL.normal, move) > 0 ? move : overrideMove * move.magnitude;
                move = Vector3.SignedAngle(hitL.normal, move, Vector3.up) > 0 ? move : m_OverrideMove * move.magnitude;
            }
            else if (Vector3.Dot(hitF.normal, m_Car.forward) < -0.3f)
            {
                //Turn left
                m_OverrideMove = Quaternion.Euler(0, -90, 0) * hitF.normal;
                //move = Vector3.Dot(hitL.normal, move) > 0 ? move : overrideMove * move.magnitude;
                //print(Vector3.SignedAngle(hitF.normal, move, Vector3.up) + " | " + Vector3.SignedAngle(hitL.normal, move, Vector3.up));
                move = Vector3.SignedAngle(hitF.normal, move, Vector3.up) < 0 ? move : m_OverrideMove * move.magnitude;
            }
        }

        //DOWN LEFT CORNER
        if (Left && Down)
        {
            if (Vector3.Dot(hitD.normal, m_Car.forward) < -0.3f)
            {
                //Turn left
                m_OverrideMove = Quaternion.Euler(0, 90, 0) * hitD.normal;
                move = Vector3.Dot(hitL.normal, move) > 0 ? move : m_OverrideMove * move.magnitude;
            }
            else if (Vector3.Dot(hitL.normal, m_Car.forward) < -0.3f)
            {
                //Turn right
                m_OverrideMove = Quaternion.Euler(0, -90, 0) * hitL.normal;
                move = Vector3.Dot(hitL.normal, move) > 0 ? move : m_OverrideMove * move.magnitude;
            }
        }

        //UP RIGHT CORNER
        if (Up && Right)
        {
            if (Vector3.Dot(hitR.normal, m_Car.forward) < -0.3f)
            {
                //Turn right
                m_OverrideMove = Quaternion.Euler(0, -90, 0) * hitR.normal;
                move = Vector3.Dot(hitR.normal, move) > 0 ? move : m_OverrideMove * move.magnitude;
            }
            else if (Vector3.Dot(hitF.normal, m_Car.forward) < -0.3f)
            {
                //Turn left
                m_OverrideMove = Quaternion.Euler(0, 90, 0) * hitF.normal;
                move = Vector3.Dot(hitR.normal, move) > 0 ? move : m_OverrideMove * move.magnitude;
            }
        }

        //DOWN RIGHT CORNER
        if (Down && Right)
        {
            if (Vector3.Dot(hitR.normal, m_Car.forward) < -0.3f)
            {
                //Turn right
                m_OverrideMove = Quaternion.Euler(0, 90, 0) * hitR.normal;
                move = Vector3.Dot(hitR.normal, move) > 0 ? move : m_OverrideMove * move.magnitude;
            }
            else if (Vector3.Dot(hitD.normal, m_Car.forward) < -0.3f)
            {
                //Turn left
                m_OverrideMove = Quaternion.Euler(0, -90, 0) * hitD.normal;
                move = Vector3.Dot(hitR.normal, move) > 0 ? move : m_OverrideMove * move.magnitude;
            }
        }
    }
}