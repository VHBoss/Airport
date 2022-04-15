using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupHead : MonoBehaviour
{
    public int Capacity = 8;
    public float UpdatePeriod = 0.5f;
    public Queue<Vector4> Path;
    public Queue<Vector3> path = new Queue<Vector3>();
    private Queue<float> distancesAbsolute = new Queue<float>();
    private Queue<float> distances = new Queue<float>();
    public float Speed => m_Speed;

    public float m_Speed;
    private float nextTimeUpdate = 0.0f;
    private Vector3 prevPos;
    private Vector3 prevPointPos;
    public float totalDistance;

    void Start()
    {
        Path = new Queue<Vector4>(Capacity);
        prevPos = transform.position;
        prevPointPos = prevPos;
    }

    private void Update()
    {
        if (Time.time > nextTimeUpdate)
        {
            nextTimeUpdate += UpdatePeriod;
            UpdateTrail();
        }

        CalculateSpeed();
    }

    void CalculateSpeed()
    {
        float delta = Vector3.Distance(prevPos, transform.position);
        m_Speed = delta / Time.deltaTime;
        prevPos = transform.position;
    }

    private void UpdateTrail2()
    {
        float distance = Vector3.Distance(prevPointPos, transform.position);
        totalDistance += distance;

        prevPointPos = transform.position;
        Vector4 item = transform.position;
        path.Enqueue(item);
        distancesAbsolute.Enqueue(totalDistance);
        distances.Enqueue(distance);

        if (path.Count > Capacity)
        {
            path.Dequeue();
            distancesAbsolute.Dequeue();
            distances.Dequeue();
        }
    }

    public void OnSliderChanged(float value)
    {
        //float absSlider = value * totalDistance;
        //for (int i = 0; i < distances.Count - 1; i++)
        //{
        //    if (absSlider > distancesAbsolute[i] && absSlider < distancesAbsolute[i + 1])
        //    {
        //        curPos = points[i];
        //        nextPos = points[i + 1];
        //        m_Distance = distances[i + 1];
        //        m_DistanceAbsolute = distancesAbsolute[i];
        //        break;
        //    }
        //}
        //float val = (absSlider - m_DistanceAbsolute) / m_Distance;
        //m_Arrow.position = Vector3.Lerp(curPos, nextPos, val);
    }

    private void UpdateTrail()
    {
        totalDistance += Vector3.Distance(prevPointPos, transform.position);
        prevPointPos = transform.position;

        Vector4 item = transform.position;
        item.w = totalDistance;
        Path.Enqueue(item);

        if(Path.Count > Capacity)
        {
            Vector4 tail = Path.Dequeue();
            print(tail.w);
            totalDistance -= tail.w;
        }
    }

    private void OnDrawGizmos()
    {
        if(Path == null)
        {
            return;
        }
        foreach (var item in Path)
        {
            Gizmos.DrawSphere(item, 0.5f);
        }
    }
}
