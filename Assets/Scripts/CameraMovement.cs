using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform target;
    public Transform Pivot;

    public bool isCustomOffset;
    public Vector3 offset;

    public float smoothSpeed = 0.1f;

    private void Start()
    {
        // You can also specify your own offset from inspector
        // by making isCustomOffset bool to true
        if (!isCustomOffset)
        {
            offset = Pivot.transform.position - target.position;
        }
    }

    private void LateUpdate()
    {
        SmoothFollow();   
    }

    public void SmoothFollow()
    {
        Vector3 targetPos = target.position + offset;
        Vector3 smoothFollow = Vector3.Lerp(Pivot.transform.position,
        targetPos, smoothSpeed);

        Pivot.transform.position = smoothFollow;
        Pivot.transform.LookAt(target);
    }
}
