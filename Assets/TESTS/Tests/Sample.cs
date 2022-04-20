using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    void Start()
    {
        uint a = 0b_0001;
        print(a);
        a |= 0b_1000;
        print(a);
        a |= 0b_0010;
        print(a);
        print(a);
    }
}
