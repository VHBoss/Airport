using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceArea : MonoBehaviour
{
    public Transform GetEmptySlot()
    {
        foreach (Transform item in transform)
        {
            if (item.gameObject.activeInHierarchy)
            {
                return item;
            }
        }
        return null;
    }
}
