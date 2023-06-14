using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HomeWork0614
{
    public class Weapon : MonoBehaviour
    {
        [SerializeField] int damage;

        Collider collider;

        private void Awake()
        {
            collider = GetComponent<Collider>();
        }

        public void EnableWeapon()
        {
            collider.enabled = true;
        }

        public void DisableWeapon()
        {
            collider.enabled = false;
        }

        private void OnTriggerEnter(Collider other)
        {
            IHittable hittable = other.GetComponent<IHittable>();
            hittable?.TakeDamage(damage);
        }
    }
}
