using System;
using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed;
    public float atkSpeed = 0.5f;
    public int hp, atk;

    protected Rigidbody2D rb;
    protected Animator animator;
    protected Status status;
    protected Transform player;
    protected bool canAttack = false;

    protected void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        status = Status.MOVING;
        player = PlayerController.Instance.transform;
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
}

public enum Status
{
    IDLING,
    MOVING,
    TARGETING,
    ATTACKING,
}