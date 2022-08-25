using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using Lean.Pool;
using TMPro;
using UnityEngine;

public class HPBarController : MonoBehaviour
{
    public SpriteRenderer hpBar, secHpBar;
    [HideInInspector] public int maxHp, currentHp;

    public void Init(int hp)
    {
        maxHp = hp;
        currentHp = hp;
        hpBar.transform.DOScaleX(1, 0);
        secHpBar.transform.DOScaleX(1, 0);
    }

    public void GetDame(int dame)
    {
        currentHp -= dame;
        ShowDame(dame);
        hpBar.transform.DOScaleX(currentHp * 1.0f / maxHp, 0)
            .OnComplete(() =>
            {
                Sequence hpRun = DOTween.Sequence();
                hpRun.AppendInterval(1f);
                hpRun.Append(secHpBar.transform.DOScaleX(currentHp * 1.0f / maxHp, 2));
            });
    }

    public void ShowDame(int dame)
    {
        LeanGameObjectPool pool = DamePool.Instance.pool;
        GameObject txtDame = pool.Spawn(transform.position, Quaternion.identity, pool.transform);
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