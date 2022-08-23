using System.Collections;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : Singleton<PlayerController>
{
    private Animator animator;

    [HideInInspector] public float horizontal;
    public float moveSpeed = 8f, jumpingPower = 15f, atkSpeed = 0.5f;
    private bool isFacingRight = true;

    private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private int jumpCount = 0;
    private bool canSpeed = true, speeding = false, attacking = false;

    private void Awake()
    {
        Application.targetFrameRate = 60;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
#if UNITY_EDITOR
        horizontal = Input.GetAxisRaw("Horizontal");
#else
        horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
#endif

        Flip();
    }

    private void FixedUpdate()
    {
        if (!speeding && !attacking)
            rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        if (speeding)
        {
            if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
            {
                speeding = false;
            }
        }
        else if (attacking)
        {
        }
        else if (rb.velocity.y > 0) animator.Play("jumpUp");
        else if (rb.velocity.y < 0) animator.Play("jumpDown");
        else if (horizontal != 0) animator.Play("run");
        else animator.Play("idle");
    }

    public void DoJump()
    {
        if (IsGrounded() || jumpCount == 1)
        {
            jumpCount++;
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
    }

    public void DoSpeed()
    {
        if (speeding || !canSpeed) return;
        speeding = true;
        canSpeed = false;
        animator.Play("speed");
        if (isFacingRight) rb.velocity = new Vector2(moveSpeed * 4, rb.velocity.y);
        else rb.velocity = new Vector2(moveSpeed * -4, rb.velocity.y);
        StartCoroutine(StopSpeed());
    }

    public void DoAttack()
    {
        if (attacking || speeding || rb.velocity.y != 0) return;
        attacking = true;
        rb.velocity = Vector2.zero;
        animator.Play(GetComponent<SkinCombined>().weapon.ToString().ToLower() + "_attack" + Random.Range(0, 3));
        StartCoroutine(StopAttack());
    }

    private bool IsGrounded()
    {
        if (Physics2D.OverlapCircle(groundCheck.position, 0.3f, groundLayer))
        {
            jumpCount = 0;
            return true;
        }

        return false;
    }

    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    private IEnumerator StopSpeed()
    {
        yield return new WaitForSeconds(0.2f);
        rb.velocity = new Vector2(horizontal * moveSpeed, rb.velocity.y);
        speeding = false;
        yield return new WaitForSeconds(0.2f);
        canSpeed = true;
    }

    private IEnumerator StopAttack()
    {
        yield return new WaitForSeconds(atkSpeed);
        attacking = false;
    }

    public void GetDame(int dame)
    {
        Debug.Log(dame);
    }
}