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
        hpBar.transform.DOScaleX(currentHp * 1.0f / maxHp, 0)
            .OnComplete(() =>
            {
                Sequence hpRun = DOTween.Sequence();
                hpRun.AppendInterval(1f);
                hpRun.Append(secHpBar.transform.DOScaleX(currentHp * 1.0f / maxHp, 2));
            });
    }

    
}