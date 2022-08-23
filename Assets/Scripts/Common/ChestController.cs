using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestController : MonoBehaviour
{
    private bool isCollected = false;

    private void Start()
    {
        isCollected = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isCollected && other.CompareTag("Player"))
        {
            GetComponent<Animator>().Play("open");
            isCollected = true;
        }
            
    }
}
