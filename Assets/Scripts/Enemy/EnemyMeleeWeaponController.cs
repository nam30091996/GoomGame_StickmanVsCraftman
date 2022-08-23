using DG.Tweening;
using Lean.Pool;
using UnityEngine;

public class EnemyMeleeWeaponController : MonoBehaviour
{
    [HideInInspector] public int atk;
    public LeanGameObjectPool pool;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            transform.DOKill();
            other.GetComponent<PlayerController>().GetDame(atk);
            DoDespawn();
        }
        else if (!other.CompareTag("GamePlayObject"))
        {
            transform.DOKill();
            Invoke(nameof(DoDespawn), 3f);
        }
    }

    private void DoDespawn()
    {
        pool.Despawn(this.gameObject);
    }
}