using System;
using System.Collections;
using DG.Tweening;
using Lean.Pool;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;


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
        if (die) return;
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

    protected virtual void CheckDame()
    {
        if(canAttack) player.GetComponent<PlayerController>().GetDame(atk);
    }

    private IEnumerator hideHpBar;
    public void GetDame(int dame)
    {
        if (die) return;
        hpBar.gameObject.SetActive(true);
        currentHp -= dame;
        ShowDame(dame);
        if (currentHp <= 0)
        {
            Die();
            return;
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
        canAttack = false;
        rb.velocity= Vector2.zero;
        GetComponent<BoxCollider2D>().enabled = false;
        hpBar.gameObject.SetActive(false);
        animator.Play("die");
    }
    
    public void ShowDame(int dame)
    {
        LeanGameObjectPool pool = DamePool.Instance.pool;
        GameObject txtDame = pool.Spawn(hpBar.transform.position, Quaternion.identity, pool.transform);
        txtDame.transform.SetAsLastSibling();
        
        var tmr = txtDame.GetComponent<TMP_Text>();
        tmr.DOFade(1, 0.4f).SetDelay(0.4f);
        tmr.text = dame.ToString();
        Vector2 s = new Vector3(Random.Range(-0.5f, 0.5f), Random.Range(2f, 3f));
        txtDame.GetComponent<Rigidbody2D>().velocity = (s * 2f);
        var lScale = txtDame.transform.localScale;
        txtDame.transform.DOScale(0f, 0.4f).OnComplete(() =>
        {
            txtDame.transform.localScale = lScale;
            pool.Despawn(txtDame);
        }).SetDelay(0.7f);
    }
}

public enum Status
{
    IDLING,
    MOVING,
    TARGETING,
    ATTACKING,
}