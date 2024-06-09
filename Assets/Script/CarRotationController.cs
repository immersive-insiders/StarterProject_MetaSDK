using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarRotationController : MonoBehaviour
{
    public void Rotate(int value)
    {
        transform.rotation = Quaternion.Euler(0, value, 0);
    }
}
