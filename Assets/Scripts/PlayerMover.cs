using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

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
        anim = GetComponent<Animator>();
        controller = GetComponent<CharacterController>();
    }

    private void OnEnable()
    {
        Cursor.lockState = CursorLockMode.Locked;
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

        Vector3 forwardVector = new Vector3(Camera.main.transform.forward.x, 0, Camera.main.transform.forward.z).normalized;
        Vector3 rightVector = new Vector3(Camera.main.transform.right.x, 0, Camera.main.transform.right.z).normalized;


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
        foreach(Collider collider in colliders)
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

    private void OnWalk(InputValue value)
    {
        isWalk = value.isPressed;
    }

    private void Fall()
    {
        ySpeed += Physics.gravity.y * Time.deltaTime;

        if (isGrounded && ySpeed < 0)
        {
            ySpeed = 0f;
            anim.SetBool("IsGrounded", true);
        }

        controller.Move(Vector3.up * ySpeed * Time.deltaTime);
    }

    private void Jump()
    {
        ySpeed = jumpSpeed;
    }

    private void OnJump(InputValue value)
    {
        anim.SetBool("IsGrounded", false);
        Jump();
    }

    private void GroundChecker()
    {
        RaycastHit hit;

        isGrounded = Physics.SphereCast(transform.position + Vector3.up * 1f, 0.5f, Vector3.down, out hit, 0.6f);
    }

    private void OnDisable()
    {
        Cursor.lockState = CursorLockMode.None;
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
