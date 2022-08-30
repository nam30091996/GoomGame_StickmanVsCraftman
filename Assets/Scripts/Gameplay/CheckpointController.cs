using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckpointController : MonoBehaviour
{
    public static Vector2 lastCheckPoint;

    private bool isChecked = false;

    private void Start()
    {
        isChecked = false;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isChecked && other.CompareTag("Player"))
        {
            GetComponent<Animator>().Play("checkpoint");
            lastCheckPoint = this.transform.position;
            isChecked = true;
        }
            
    }
}
