using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDetect : MonoBehaviour
{
    public List<Enemy> listEnemy;

    private void Start()
    {
        listEnemy = new List<Enemy>();
    }

    public void ResetListEnemy()
    {
        listEnemy.Clear();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            listEnemy.Add(other.GetComponent<Enemy>());
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            listEnemy.Remove(other.GetComponent<Enemy>());
        }
    }
}