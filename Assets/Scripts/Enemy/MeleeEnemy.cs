using DG.Tweening;
using Lean.Pool;
using UnityEngine;

public class MeleeEnemy : Enemy
{
    public LeanGameObjectPool weaponPool;
    
    private void Awake()
    {
        base.Awake();
        status = Status.IDLING;
        weaponPool = transform.GetChild(0).GetComponent<LeanGameObjectPool>();
    }

    private void Update()
    {
        if (player.position.x >= transform.position.x - 10 && player.position.x <= transform.position.x + 10 && player.position.y < this.transform.position.y + 1)
        {
            canAttack = true;
            if(status != Status.ATTACKING) Attack();
        }
        else
        {
            canAttack = false;
            if (status == Status.ATTACKING) return;
            status = Status.IDLING;
            animator.Play("idle");
        }
        
        Flip();
    }
    
    private void Flip()
    {
        Vector3 localScale = transform.localScale;
        if (PlayerController.Instance.transform.position.x > this.transform.position.x)
        {
            localScale.x = -1f * Mathf.Abs(localScale.y);
        }
        else
        {
            localScale.x = Mathf.Abs(localScale.y);
        }

        transform.localScale = localScale;
    }

    protected override void CheckDame()
    {
        GameObject weapon = weaponPool.Spawn(weaponPool.transform.position, Quaternion.identity);
        weapon.GetComponent<EnemyMeleeWeaponController>().atk = this.atk;
        weapon.GetComponent<EnemyMeleeWeaponController>().pool = weaponPool;
        weapon.transform.DORotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360).SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Restart);
        float moveDirect = -1;
        if (weapon.transform.position.x > transform.position.x) moveDirect = 1;
        weapon.transform.DOMoveX(weapon.transform.position.x + 100 * moveDirect, 10f).SetEase(Ease.Linear)
            .OnComplete(() =>
            {
                weaponPool.Despawn(weapon);
            });

    }
}
