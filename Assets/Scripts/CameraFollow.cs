using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;
    public int cameraSize_ = 15;

    private Camera camera_;

    private void Awake()
    {
        camera_ = GetComponent<Camera>();
        camera_.orthographicSize = cameraSize_;
        Debug.Log("Establecida camera size " + cameraSize_, DLogType.CameraSetup);
    }

    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;

        //transform.LookAt(target);

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            cameraSize_--;
            camera_.orthographicSize = cameraSize_;
            Debug.Log("Cambiado camera size " + cameraSize_, DLogType.Setup);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            cameraSize_++;
            camera_.orthographicSize = cameraSize_;
            Debug.Log("Cambiado camera size " + cameraSize_, DLogType.Setup);
        }
    }
}
