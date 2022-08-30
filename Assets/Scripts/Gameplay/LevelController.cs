using UnityEngine;

public class LevelController : MonoBehaviour
{
    public Transform enemyHolder;
    private int enemyCount;

    private void Awake()
    {
        enemyCount = enemyHolder.childCount;
        Signals.Get<OnEnemyDieSignal>().AddListener(OnEnemyDie);
    }

    private void OnDestroy()
    {
        Signals.Get<OnEnemyDieSignal>().RemoveListener(OnEnemyDie);
    }

    private void OnEnemyDie()
    {
        enemyCount--;
        Debug.Log(enemyCount);
        if(enemyCount == 0) Debug.Log("VAR");
    }
}
