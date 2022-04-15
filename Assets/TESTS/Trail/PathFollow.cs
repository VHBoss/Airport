using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathFollow : MonoBehaviour
{
    public Transform Path;
    [Range(0,1)]
    public float Speed;

    public Transform[] m_Path;
    private Transform prevPoint;
    private Transform nextPoint;
    private int currentIndex;
    private float counter;
    private float speed;

    void Start()
    {
        m_Path = Path.GetComponentsInChildren<Transform>();
        nextPoint = m_Path[currentIndex];
        NextPoint();
    }

    private void Update()
    {
        if(Speed == 0)
        {
            return;
        }

        counter += Speed * speed;
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
        if (currentIndex == m_Path.Length)
        {
            currentIndex = 0;
        }
        prevPoint = nextPoint;
        nextPoint = m_Path[currentIndex];
        float dist = Vector3.Distance(prevPoint.position, nextPoint.position);
        speed = 1f / dist;
    }
}
