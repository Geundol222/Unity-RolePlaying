using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HomeWork0614
{
    public class SoundReactor : MonoBehaviour, IListenable
    {
        private Animator anim;

        private void Awake()
        {
            anim = GetComponent<Animator>();
        }

        public void Listen(Transform trans)
        {
            anim.SetTrigger("Listen");
            StartCoroutine(LookRoutine(trans));
        }

        IEnumerator LookRoutine(Transform trans)
        {
            while (true)
            {
                Vector3 lookDir = (trans.position - transform.position).normalized;
                Quaternion lookRotation = Quaternion.LookRotation(lookDir);
                transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.1f);
                yield return null;
            }
        }
    }
}

