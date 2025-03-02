using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class CameraFollow : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public Transform target;
    public float smoothing = 5f;
    Vector3 offset;
    void Start()
    {
        offset = transform.position - target.position;
    }
    void FixedUpdate()
    {
        Vector3 targetCamPos = target.position + offset;
        transform.position = Vector3.Lerp (transform.position,targetCamPos, smoothing*Time.deltaTime);
    }
}
