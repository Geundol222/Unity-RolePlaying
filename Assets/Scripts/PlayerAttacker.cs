using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerAttacker : MonoBehaviour
{
    [SerializeField] int damage;
    [SerializeField] float range;
    [SerializeField, Range(0, 360)] float attackAngle;

    private Animator anim;
    private float cosResult;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        cosResult = Mathf.Cos(attackAngle * 0.5f * Mathf.Deg2Rad);
    }

    public void Attack()
    {
        anim.SetTrigger("Attack");
    }

    private void OnAttack(InputValue value)
    {
        Attack();
    }

    public void AttackTiming()
    {
        // 1. 범위 안에 있는지 확인
        Collider[] colliders = Physics.OverlapSphere(transform.position, range);
        foreach (Collider collider in colliders)
        {
            // 2. 각도 안에 있는지 확인
            Vector3 dirTarget = (collider.transform.position - transform.position).normalized;
            if (Vector3.Dot(transform.forward, dirTarget) < cosResult)
                continue;
            
            IHittable hittiable = collider.GetComponent<IHittable>();
            hittiable?.TakeDamage(damage);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);

        Vector3 rightDir = AngleToDir(transform.eulerAngles.y + attackAngle * 0.5f);
        Vector3 leftDir = AngleToDir(transform.eulerAngles.y - attackAngle * 0.5f);
        Debug.DrawRay(transform.position, rightDir * range, Color.green);
        Debug.DrawRay(transform.position, leftDir * range, Color.green);
    }

    private Vector3 AngleToDir(float angle)
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0, Mathf.Cos(radian));
    }
}
