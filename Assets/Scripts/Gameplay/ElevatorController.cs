using DG.Tweening;
using UnityEngine;

public class ElevatorController : MonoBehaviour
{
    public float top;

    private void Start()
    {
        transform.DOMoveY(top, 3f).SetEase(Ease.Linear).SetLoops(-1, LoopType.Yoyo);
    }
}
