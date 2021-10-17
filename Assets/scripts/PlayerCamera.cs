using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{









    /*






    public Vector3 offset;
    public bool isFirstPerson = false;
    public float mouseSensitivity = 10f;
    public Transform target;

    public float dstFromTarget = 2f;
    public bool invertedPitch = false;
    public float smoothTime = .12f;
    public float minimumX = -85f;
    public float maximumX = 85f;
    // LateUpdate, so the Camera Target will definitely have been set
    void LateUpdate()
    {
        // first person updating is handled by Inverse kinematics stuff in my case
        if (isFirstPerson) return;
        UpdateStuff();
    }

    public void UpdateStuff()
    {
        float yaw = Input.GetAxisRaw("Mouse X");
        float pitch = -Input.GetAxisRaw("Mouse Y");
        if (invertedPitch)
        {
            pitch *= -1f;
        }
        Vector3 yRot = new Vector3(0f, yaw, 0f) * mouseSensitivity;
        Vector3 xRot = new Vector3(pitch, 0f, 0f) * mouseSensitivity;
        xRot = ClampRotationAroundXAxis(transform.rotation, xRot);
        Transform newTrans = transform;
        newTrans.Rotate(xRot);
        newTrans.Rotate(yRot, Space.World);
        transform.rotation = Quaternion.Slerp(transform.rotation, newTrans.rotation, smoothTime * Time.deltaTime);
        transform.position = (target.position - transform.forward * dstFromTarget);
        transform.position= transform.position + offset;
        
    }
    public void ResetCamera(Transform lookAtTarget)
    {
        transform.LookAt(lookAtTarget);
    }

    Vector3 ClampRotationAroundXAxis(Quaternion q, Vector3 xrot)
    {
        q.x /= q.w;
        q.y /= q.w;
        q.z /= q.w;
        q.w = 1.0f;
        float angleX = 2.0f * Mathf.Rad2Deg * Mathf.Atan(q.x);
        if (angleX < minimumX && xrot.x < 0f)
        {
            xrot = Vector3.zero;
        }
        if (angleX > maximumX && xrot.x > 0f)
        {
            xrot = Vector3.zero;
        }
        return xrot;
    }*/
    public Transform target;
    public Vector3 offset;

    private float smoothFactor = 0.3f;

    // Use this for initialization
    void Start()
    {
       // target = GameObject.FindGameObjectWithTag("Player").transform;
        //offset = new Vector3 (0f, 2.0f, -3.0f);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FixedUpdate()
    {

        Vector3 desiredPos = target.position + offset;
        Vector3 smoothedPos = Vector3.Lerp(transform.position, desiredPos, smoothFactor);
        transform.position = smoothedPos;

        transform.LookAt (target);
    }
}