using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;
    private Transform camTransform;
    private Vector3 offset;
    private float SmoothTime = 0.1f;
 
    private Vector3 velocity = Vector3.zero;
 
    private void Awake()
    {
        target = PlayerController.Instance.transform;
        PlayerController.Instance.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        camTransform = GetComponent<Transform>();
        offset = camTransform.position - target.position;
    }
 
    private void LateUpdate()
    {
        if (target == null) return;
        Vector3 targetPosition = target.position + offset;
        camTransform.position = Vector3.SmoothDamp(transform.position, targetPosition, ref velocity, SmoothTime);
 
        // transform.LookAt(target);
    }
}
