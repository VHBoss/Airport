using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupTrailer : MonoBehaviour
{
    public PickupHead Head;
    public float Distance = 2;

    private Transform prevPoint;
    private Transform nextPoint;
    private int currentIndex;
    private float counter;
    private float speed;

    void Start()
    {
        NextPoint();
    }

    private void Update()
    {
        if (Head.Speed == 0)
        {
            return;
        }

        counter += Head.Speed * speed;
        transform.position = Vector3.Lerp(prevPoint.position, nextPoint.position, counter);

        Vector3 viewDirection = transform.position - nextPoint.position;
        if (viewDirection != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(viewDirection);
        }

        if (counter >= 1)
        {
            NextPoint();
        }
    }

    void NextPoint()
    {
        counter = 0;
        currentIndex++;
        if (currentIndex == Head.Path.Count)
        {
            return;
        }
        prevPoint = nextPoint;
        //nextPoint = Head.Path. (currentIndex);
        float dist = Vector3.Distance(prevPoint.position, nextPoint.position);
        speed = 1f / dist;
    }
}
