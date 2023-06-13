using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HomeWork0613
{
    public class PlayerAttacker : MonoBehaviour
    {
        [SerializeField] int damage;
        [SerializeField] float range;
        [SerializeField, Range(0, 360)] float angle;

        private Animator anim;
        private float cosResult;

        private void Awake()
        {
            anim = GetComponent<Animator>();
            cosResult = Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad);
        }

        private void Attack()
        {
            anim.SetTrigger("Attack");
        }

        private void OnAttack(InputValue value)
        {
            Attack();
        }

        public void AttackTiming()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, range);
            foreach (Collider collider in colliders)
            {
                Vector3 targetDir = (collider.transform.position - transform.forward).normalized;

                if (Vector3.Dot(transform.position, targetDir) < cosResult)
                    continue;

                IHittable hittable = collider.GetComponent<IHittable>();
                hittable?.TakeDamage(damage);
            }
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, range);

            Vector3 rightDir = AngleToDir(transform.eulerAngles.y + angle * 0.5f);
            Vector3 leftDir = AngleToDir(transform.eulerAngles.y - angle * 0.5f);
            Debug.DrawRay(transform.position, rightDir, Color.yellow);
            Debug.DrawRay(transform.position, leftDir, Color.yellow);

        }

        private Vector3 AngleToDir(float angle)
        {
            float radian = angle * Mathf.Deg2Rad;
            return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
        }
    }
}

