using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeSceneController : MonoBehaviour
{
    public void OnStartClick()
    {
        StartCoroutine(GameUtils.LoadYourAsyncScene("GameplayScene"));
    }
}
