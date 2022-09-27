using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public static PlayerController instance;

    //Player movement
    public PlayerAction inputAction;
    Vector2 move;
    Vector2 rotate;
    private float walkSpeed = 5f;
    public Camera playerCamera;
    Vector3 cameraRotation;

    //Player jump
    Rigidbody rb;
    public float distanceToGround;
    public bool isGrounded = true;
    private bool doGroundCheck;
    public float jump = 5f;

    //Player animation
    Animator playerAnimator;
    // private bool isWalking = false;

    //Projectile bullets
    public GameObject bullet;
    public Transform projectilePos;

    private void Start() {
        inputAction.Player.Enable();
    }

    private void OnDisable() {
        inputAction.Player.Disable();
    }
     
    // Start is called before the first frame update
    void Awake()
    {
        if (!instance)
        {
            instance = this;
        }

        inputAction = new PlayerAction();

        inputAction.Player.Move.performed += cntxt => move = cntxt.ReadValue<Vector2>();
        inputAction.Player.Move.canceled += cntxt => move = Vector2.zero;

        inputAction.Player.Jump.performed += cntxt => Jump();

        inputAction.Player.Punch.performed += cntxt => Shoot();

        rb = GetComponent<Rigidbody>();
        playerAnimator = GetComponent<Animator>();

        distanceToGround = (GetComponent<Collider>().bounds.extents.y);
    }

    public void Jump()
    {
        if(isGrounded)
        {
            UnityEngine.Debug.Log("Jump was called.");
            //rb.velocity = new Vector3(rb.velocity.x, jump, rb.velocity.z);
            rb.AddForce(new Vector3(0f, 10f, 0f), ForceMode.Impulse);
            //StartCoroutine(DelayGroundCheck());
        }
    }

    public void Shoot()
    {
        if (ScoreManager.instance.score > 0)
        {
            Rigidbody bulletRb = Instantiate(bullet, projectilePos.position, Quaternion.identity).GetComponent<Rigidbody>();
            bulletRb.AddForce(transform.forward * 32f, ForceMode.Impulse);
            bulletRb.AddForce(transform.up * 1f, ForceMode.Impulse);
            ScoreManager.instance.DecreaseScore();
        }
    }
    /*
    private IEnumerator DelayGroundCheck()
    {
        doGroundCheck = false;
        yield return new WaitForSeconds(1);
        doGroundCheck = true;
    }
    */
    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * move.y * Time.deltaTime * walkSpeed, Space.Self);
        transform.Translate(Vector3.right * move.x * Time.deltaTime * walkSpeed, Space.Self);

        isGrounded = Physics.Raycast(rb.position, -Vector3.up, distanceToGround);

        playerAnimator.SetBool("isGrounded", isGrounded);
        if (move.x != 0 || move.y != 0)
        {
            playerAnimator.SetBool("isMoving", true);
        }
        else
        {
            playerAnimator.SetBool("isMoving", false);
        }
    }

   // void OnDrawGizmos()
    //{
        //Gizmos.DrawLine(transform.position, transform.position - Vector3.up * distanceToGround);
    //}
}
