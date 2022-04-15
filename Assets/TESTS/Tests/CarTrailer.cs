using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarTrailer : MonoBehaviour
{
    public CarController Car;
    public List<Transform> bodyParts = new List<Transform>();
    public float minDistance = 1.5f;

    private Vector3 movementVelocity;
    void FixedUpdate()
    {
        float speed = Car.Velocity;
        if (speed > 0)
        {
            for (int i = 1; i < bodyParts.Count; i++)
            {
                Transform curBodyPart = bodyParts[i];
                Transform PrevBodyPart = bodyParts[i - 1];

                float dis = Vector3.Distance(PrevBodyPart.position, curBodyPart.position);
                float T = Time.deltaTime * dis / minDistance * speed;

                if (T > 0.5f)
                    T = 0.5f;

                curBodyPart.position = Vector3.Slerp(curBodyPart.position, PrevBodyPart.position, T);
                curBodyPart.rotation = Quaternion.Slerp(curBodyPart.rotation, PrevBodyPart.rotation, T);
                //curBodyPart.position = Vector3.SmoothDamp(curBodyPart.position, PrevBodyPart.position, ref movementVelocity, minDistance * Time.fixedDeltaTime, Mathf.Infinity, Time.fixedDeltaTime);

            }
        }
    }
}
