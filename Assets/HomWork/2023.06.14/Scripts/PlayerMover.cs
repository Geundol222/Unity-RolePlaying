using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;

namespace HomeWork0614
{
    public class PlayerMover : MonoBehaviour
    {
        [SerializeField] bool debug;

        [SerializeField] float walkSpeed;
        [SerializeField] float runSpeed;
        [SerializeField] float jumpSpeed;
        [SerializeField] float walkStepRange;
        [SerializeField] float runStepRange;

        private Animator anim;
        private CharacterController controller;
        private Vector3 moveDir;
        private float curSpeed;
        private float ySpeed;
        private bool isGrounded;
        private bool isWalk;

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
            anim = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            StartCoroutine(MoveRoutine());
        }

        IEnumerator MoveRoutine()
        {
            while (true) 
            {
                Move();
                Fall();
                yield return null;
            }
        }

        float lastStepTime = 0.5f;

        private void Move()
        {
            if (moveDir.magnitude == 0)
            {
                curSpeed = Mathf.Lerp(curSpeed, 0, 0.05f);
                anim.SetFloat("MoveSpeed", curSpeed);
                return;
            }

            if (isWalk)
            {
                curSpeed = Mathf.Lerp(curSpeed, walkSpeed, 0.05f);
                anim.SetFloat("MoveSpeed", curSpeed);
            }
            else
            {
                curSpeed = Mathf.Lerp(curSpeed, runSpeed, 0.05f);
                anim.SetFloat("MoveSpeed", curSpeed);
            }

            Transform camera = Camera.main.transform;
            Vector3 forwardVector = new Vector3(camera.forward.x, 0, camera.forward.z).normalized;
            Vector3 rightVector = new Vector3(camera.right.x, 0, camera.right.z).normalized;

            controller.Move(forwardVector * moveDir.z * curSpeed * Time.deltaTime);
            controller.Move(rightVector * moveDir.x * curSpeed * Time.deltaTime);

            Quaternion lookRotation = Quaternion.LookRotation(forwardVector * moveDir.z + rightVector * moveDir.x);
            transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.05f);

            lastStepTime -= Time.deltaTime;
            if (lastStepTime < 0)
            {
                lastStepTime = 0.5f;
                GenerateFootStepSound();
            }
        }

        private void GenerateFootStepSound()
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position, isWalk ? walkStepRange : runStepRange);
            foreach (Collider collider in colliders)
            {
                IListenable listenable = collider.GetComponent<IListenable>();
                listenable?.Listen(transform);
            }
        }

        private void OnMove(InputValue value)
        {
            moveDir.x = value.Get<Vector2>().x;
            moveDir.z = value.Get<Vector2>().y;
        }

        private void Fall()
        {
            ySpeed += Physics.gravity.y * Time.deltaTime;

            if (isGrounded && ySpeed < 0)
                ySpeed = 0;
            
            controller.Move(Vector3.up * ySpeed * Time.deltaTime);
        }

        private void Jump()
        {
            ySpeed = jumpSpeed;
        }

        private void OnJump(InputValue value)
        {
            Jump();
        }

        private void OnWalk(InputValue value)
        {
            isWalk = value.isPressed;
        }

        private void GroundChecker()
        {
            RaycastHit hit;

            isGrounded = Physics.SphereCast(transform.position + Vector3.up * 1f, 0.4f, Vector3.down, out hit, 0.6f);
        }

        private void OnDisable()
        {
            StopCoroutine(MoveRoutine());
        }

        private void OnDrawGizmosSelected()
        {
            if (!debug)
                return;

            Gizmos.color = Color.cyan;
            Gizmos.DrawWireSphere(transform.position, walkStepRange);
            Gizmos.DrawWireSphere(transform.position, runStepRange);
        }
    }
}