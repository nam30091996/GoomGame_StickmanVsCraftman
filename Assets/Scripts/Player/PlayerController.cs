using System.Collections;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class PlayerController : Singleton<PlayerController>
{
    private Animator animator;

    public int atk, maxHp, currentHp;
    public float moveSpeed = 8f, jumpingPower = 15f, atkSpeed = 0.5f;
    public EnemyDetect noneWeaponDetect, swordDetect;
    
    private bool isFacingRight = true;

    private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;

    private int jumpCount = 0, attackNum = 0;
    private bool canSpeed = true, speeding = false, attacking = false;
    private float timeLastAttack = 0;
    private float horizontal;
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
        if (attacking || speeding) return;
        attacking = true;
        rb.velocity = Vector2.zero;
        if (Time.timeSinceLevelLoad - timeLastAttack >= atkSpeed + 0.25f)
        {
            attackNum = 0;
        }
        else
        {
            attackNum++;
            if (attackNum > 2) attackNum = 0;
        }
        animator.Play(GetComponent<SkinCombined>().weapon.ToString().ToLower() + "_attack" + attackNum);
        timeLastAttack = Time.timeSinceLevelLoad;
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
        yield return new WaitForSeconds(0.25f);
        switch (GetComponent<SkinCombined>().weapon)
        {
            case Weapon.NONE:
                foreach (Enemy enemy in noneWeaponDetect.listEnemy.ToList())
                {
                    enemy.GetDame(atk);
                }
                break;
            case Weapon.SWORD:
                foreach (Enemy enemy in swordDetect.listEnemy.ToList())
                {
                    enemy.GetDame(atk);
                }
                break;
        }
        yield return new WaitForSeconds(atkSpeed - 0.25f);
        attacking = false;
    }

    public void GetDame(int dame)
    {
    }
}