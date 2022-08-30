using System.Collections;
using UnityEngine;

public class Enemy2Controller : HorizontalMoveEnemy
{
    protected override void Attack()
    {
        if (die) return;
        status = Status.ATTACKING;
        
        animator.speed = 1;
        animator.Play("attack");
        StartCoroutine(Explode());
    }

    private IEnumerator Explode()
    {
        yield return new WaitForSeconds(1f);
        Die();
        CheckDame();
    }

    protected override void CheckDame()
    {
        if(Mathf.Abs(PlayerController.Instance.transform.position.x - transform.position.x) <= 4) PlayerController.Instance.GetDame(atk);
    } 

    public override void GetDame(int dame)
    {
        if (die) return;
        hpBar.gameObject.SetActive(true);
        currentHp -= dame;
        ShowDame(dame);
        if (currentHp <= 0)
        {
            die = true;
            canAttack = false;
            rb.velocity= Vector2.zero;
            GetComponent<BoxCollider2D>().enabled = false;
            hpBar.gameObject.SetActive(false);
            Signals.Get<OnEnemyDieSignal>().Dispatch();
            gameObject.SetActive(false);
        }
    }
}
