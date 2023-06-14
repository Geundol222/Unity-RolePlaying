using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundReactor : MonoBehaviour, IListenable
{
    public void Listen(Transform trans)
    {
        StartCoroutine(LookRoutine(trans));
    }

    IEnumerator LookRoutine(Transform trans)
    {
        while (true)
        {
            Vector3 lookDir = (trans.position - transform.position).normalized;
            Quaternion lookRot = Quaternion.LookRotation(new Vector3(lookDir.x, 0, lookDir.z));
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRot, 0.1f);
            yield return null;
        }
    }
}
