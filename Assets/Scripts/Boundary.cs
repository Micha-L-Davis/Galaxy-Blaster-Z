using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boundary : MonoBehaviour
{


    private void OnTriggerEnter(Collider other)
    {
        IConcealable entered = other.GetComponent<IConcealable>();
        if (entered != null)
            entered.ToggleConcealment();

    }

    private void OnTriggerExit(Collider other)
    {
        IConcealable exited = other.GetComponent<IConcealable>();
        if (exited != null)
            exited.ToggleConcealment();
    }
}
