using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    public float atkSpeed = 0.5f;
    public int maxHp, atk, currentHp;
    public HPBarController hpBar;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected Status status;
    protected Transform player;
    protected bool canAttack = false, die = false;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        status = Status.MOVING;
        player = PlayerController.Instance.transform;
        hpBar.Init(maxHp);
    }

    protected void Attack()
    {
        status = Status.ATTACKING;
        
        Vector3 localScale = transform.localScale;
        if (player.position.x < this.transform.position.x)
        {
            localScale.x = Mathf.Abs(localScale.y);
        }
        else
        {
            localScale.x = -1f * Mathf.Abs(localScale.y);
        }
        transform.localScale = localScale;
        
        animator.speed = 1;
        animator.Play("attack");
        StartCoroutine(StopAttack());
    }

    private IEnumerator StopAttack()
    {
        yield return new WaitForSeconds(0.25f);
        CheckDame();
        yield return new WaitForSeconds(atkSpeed - 0.25f);
        if (canAttack)
        {
            Attack();
        }
        else status = Status.TARGETING;
    }

    protected virtual void CheckDame(){}

    private IEnumerator hideHpBar;
    public void GetDame(int dame)
    {
        if (die) return;
        hpBar.gameObject.SetActive(true);
        currentHp -= dame;
        if (currentHp <= 0)
        {
            Die();
        }
        hpBar.GetDame(dame);
        if (hideHpBar != null) StopCoroutine(hideHpBar);
        hideHpBar = HideHpBar();
        StartCoroutine(hideHpBar);
    }

    private IEnumerator HideHpBar()
    {
        yield return new WaitForSeconds(5f);
        hpBar.gameObject.SetActive(false);
    }

    private void Die()
    {
        die = true;
        rb.velocity= Vector2.zero;
        GetComponent<BoxCollider2D>().enabled = false;
        animator.Play("die");
    }
}

public enum Status
{
    IDLING,
    MOVING,
    TARGETING,
    ATTACKING,
}