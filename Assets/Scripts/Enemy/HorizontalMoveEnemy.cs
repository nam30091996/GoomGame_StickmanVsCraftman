using System;
using System.Collections;
using UnityEngine;

public class HorizontalMoveEnemy: Enemy
{
    public float firstPoint, lastPoint;
    
    private bool isMovingLeft = true;
    private void Awake()
    {
        base.Awake();
        status = Status.MOVING;
    }

    private void Update()
    {
        if (status == Status.ATTACKING || die) return;
        if (player.position.x >= firstPoint && player.position.x <= lastPoint && player.position.y < this.transform.position.y + 1)
        {
            status = Status.TARGETING;
            if (player.position.x < this.transform.position.x) isMovingLeft = true;
            else isMovingLeft = false;
        }
        else
        {
            status = Status.MOVING;
        }
        
        Flip();
    }

    private void FixedUpdate()
    {
        if (die) return;
        if (status == Status.ATTACKING)
        {
            rb.velocity = Vector2.zero;
        }
        else if (status == Status.MOVING)
        {
            rb.velocity = new Vector2((isMovingLeft ? -1 : 1) * moveSpeed, rb.velocity.y);
            animator.speed = 1;
            animator.Play("run");
        }
        else if (status == Status.TARGETING)
        {
            isMovingLeft = player.position.x < this.transform.position.x;
            rb.velocity = new Vector2((isMovingLeft ? -2 : 2) * moveSpeed, rb.velocity.y);
            animator.speed = 2;
            animator.Play("run");
        }
    }
    
    private void Flip()
    {
        if (status == Status.MOVING && (isMovingLeft && transform.position.x < firstPoint || !isMovingLeft && transform.position.x > lastPoint))
        {
            isMovingLeft = !isMovingLeft;
        }
        
        Vector3 localScale = transform.localScale;
        if (isMovingLeft)
        {
            localScale.x = Mathf.Abs(localScale.y);
        }
        else
        {
            localScale.x = -1f * Mathf.Abs(localScale.y);
        }
        transform.localScale = localScale;
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canAttack = true;
            Attack();
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            canAttack = false;
        }
    }

    protected override void CheckDame()
    {
        if(canAttack) player.GetComponent<PlayerController>().GetDame(atk);
    }
}
