using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonMovement : MonoBehaviour
{
    CharacterController CharacterController;
    public FixedJoystick LeftJoystick;
    public float speed = 6.0f;
    public float TurnSmoothTime = 0.1f;
    float turnsmoothvelocity;
    public Transform cam;
    // Start is called before the first frame update
    void Start()
    {
        CharacterController = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = new Vector3(LeftJoystick.inputVector.x, 0, LeftJoystick.inputVector.y).normalized;


        if (direction.magnitude >= 0.1f)
        {
            float targetangle = Mathf.Atan2(direction.x, direction.z)*Mathf.Rad2Deg+cam.eulerAngles .y;
            float Angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetangle, ref turnsmoothvelocity, TurnSmoothTime);
            transform.rotation = Quaternion.Euler(0f, Angle, 0f);

            Vector3 movdir = Quaternion.Euler(0f, targetangle, 0f) * Vector3.forward;
            CharacterController.Move(movdir.normalized * speed * Time.deltaTime);

        }
    }
}
