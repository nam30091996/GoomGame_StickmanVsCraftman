using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : MonoBehaviour
{
    private Animator animator;

    [HideInInspector] public float horizontal;
    private float speed = 8f;
    private float jumpingPower = 15f;
    private bool isFacingRight = true;

    private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private int jumpCount = 0;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        horizontal = CrossPlatformInputManager.GetAxis("Horizontal");

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
        {
            if (IsGrounded() || jumpCount == 1)
            {
                jumpCount++;
                rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
            }
        }

        Flip();
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        if(rb.velocity.y > 0) animator.Play("jumpUp");
        else if(rb.velocity.y < 0) animator.Play("jumpDown");
        else if(horizontal != 0) animator.Play("run");
        else animator.Play("idle");
    }

    private bool IsGrounded()
    {
        if (Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer))
        {
            jumpCount = 0;
            return true;
        }
        return false;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    public void Jump()
    {
        
    }

    // public void Move(MoveDirection direction)
    // {
    //     if (direction == MoveDirection.LEFT)
    //     {
    //         transform.rotation = Quaternion.Euler(0, 180, 0);
    //     }
    //     else
    //     {
    //         transform.rotation = Quaternion.Euler(0, 0, 0);
    //     }
    //
    //     animator.Play("run");
    // }
}

public enum MoveDirection
{
    LEFT,
    RIGHT,
}