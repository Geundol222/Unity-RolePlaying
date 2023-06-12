using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMover : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    [SerializeField] float jumpSpeed;

    private CharacterController controller;
    private Vector3 moveDir;
    private float ySpeed;

    private void Awake()
    {
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
            return;

        Vector3 forwardVector = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
        Vector3 rightVector = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;

        controller.Move(forwardVector * moveDir.z * moveSpeed * Time.deltaTime);
        controller.Move(rightVector * moveDir.x * moveSpeed * Time.deltaTime);

        Quaternion lookRotation = Quaternion.LookRotation(forwardVector * moveDir.z + rightVector * moveDir.x);
        transform.rotation = Quaternion.Lerp(transform.rotation, lookRotation, 0.05f);
    }

    private void OnMove(InputValue value)
    {
        moveDir.x = value.Get<Vector2>().x;
        moveDir.z = value.Get<Vector2>().y;
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
