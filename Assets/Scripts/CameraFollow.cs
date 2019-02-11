using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour {

    public Transform target;

    public float smoothSpeed = 0.125f;
    public Vector3 offset;    

    private Camera camera_;

    private void Start()
    {
        camera_ = GetComponent<Camera>();
        camera_.orthographicSize = GameManager.instance.cameraSize_;
        Debug.Log("Establecida camera size " + GameManager.instance.cameraSize_, DLogType.CameraSetup);
    }

    private void FixedUpdate()
    {
        Vector3 desiredPosition = target.position + offset;
        Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

        transform.position = smoothedPosition;

        //transform.LookAt(target);

        if (Input.GetAxis("Mouse ScrollWheel") > 0f) // forward
        {
            GameManager.instance.cameraSize_--;
            camera_.orthographicSize = GameManager.instance.cameraSize_;
            Debug.Log("Cambiado camera size " + GameManager.instance.cameraSize_, DLogType.Setup);
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f) // backwards
        {
            GameManager.instance.cameraSize_++;
            camera_.orthographicSize = GameManager.instance.cameraSize_;
            Debug.Log("Cambiado camera size " + GameManager.instance.cameraSize_, DLogType.Setup);
        }
    }
}
