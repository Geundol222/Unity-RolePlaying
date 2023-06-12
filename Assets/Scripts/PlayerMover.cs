using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] float runSpeed;
    [SerializeField] float walkSpeed;
    [SerializeField] float jumpSpeed;

    private Animator anim;
    private CharacterController controller;
    private Vector3 moveDir;
    private float curSpeed;
    private float ySpeed;
    private bool isWalk;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
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

        Vector3 forwardVector = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
        Vector3 rightVector = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;

        controller.Move(forwardVector * moveDir.z * curSpeed * Time.deltaTime);
        controller.Move(rightVector * moveDir.x * curSpeed * Time.deltaTime);

        Quaternion lookRotation = Quaternion.LookRotation(forwardVector * moveDir.z + rightVector * moveDir.x);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.05f);
    }

    private void OnMove(InputValue value)
    {
        moveDir.x = value.Get<Vector2>().x;
        moveDir.z = value.Get<Vector2>().y;
    }

    private void OnWalk(InputValue value)
    {
        isWalk = value.isPressed;
    }

    private void Fall()
    {
        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (controller.isGrounded && ySpeed < 0)
            ySpeed = 0f;

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

    private void OnDisable()
    {
        StopCoroutine(MoveRoutine());
    }
}
