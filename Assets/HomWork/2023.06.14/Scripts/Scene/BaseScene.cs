using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HomeWork0614
{
    public abstract class BaseScene : MonoBehaviour
    {
        public float progress { get; protected set; }
        protected abstract IEnumerator LoadingRoutine();

        public void LoadAsync()
        {
            StartCoroutine(LoadingRoutine());
        }
    }
}