using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HitAdaptor : MonoBehaviour, IHittable
{
    public UnityEvent<int> OnDamaged;

    public void TakeDamage(int damage)
    {
        OnDamaged?.Invoke(damage);
    }
}
