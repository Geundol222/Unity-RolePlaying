using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapDoor : MonoBehaviour
{
    public void Open()
    {
        Debug.Log("π‚¿Ω");
        transform.position = new Vector3(transform.position.x, transform.position.y + 1f, transform.position.z);
        transform.Rotate(90, 0, 0);
    }
}
