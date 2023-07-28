using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    //[SerializeField] DynamicJoystick joystick;
    public float rotateSpeed;
    public float rotateSpeedDefault;

    public float moveSpeed;
    public float moveSpeedDefault;

    [SerializeField] Animator anim;
    [SerializeField] Rigidbody rb;
    Vector3 movementDir;
    float xDir, zDir;
    public bool canMove;
    public static PlayerMovement Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    void Update()
    {

            Move();
            SetAnimationMove();
        
    }

    private void Move()
    {
        if (!canMove) return;

        xDir = Joystick.Instance.Horizontal;
        zDir = Joystick.Instance.Vertical;



        movementDir.Set(xDir, 0, zDir);
        movementDir.Normalize();
        float inputMagnitude = Mathf.Clamp01(movementDir.magnitude);
        anim.SetFloat("Input Magnitude", inputMagnitude, 0.1f, Time.deltaTime);

    }

    private void FixedUpdate()
    {
        if (!canMove) return;

        rb.velocity = movementDir * Time.deltaTime * moveSpeed;
        RotatePlayerFace();
        

    }

    public void PlayerIsDied()
    {
        canMove = false;
        movementDir.Set(0, 0, 0);
        rb.velocity = movementDir;
    }

    public void RotatePlayerFace()
    {
        if (movementDir != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(movementDir.normalized), Time.deltaTime * rotateSpeed);
    }

    void SetAnimationMove()
    {
        if (movementDir != Vector3.zero)
            anim.SetBool("Running", true);
        else
            anim.SetBool("Running", false);
    }

}
