using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailTest : MonoBehaviour
{
    public TrailRenderer trail;
    void Start()
    {
        //trail.getpos
    }

    private void Update()
    {
        if(trail.positionCount > 10)
        {
            transform.position = trail.GetPosition(0);
        }
    }
    private void OnGUI()
    {
        GUI.Label(new Rect(10, 10, 50, 20), trail.positionCount.ToString());
    }
}
