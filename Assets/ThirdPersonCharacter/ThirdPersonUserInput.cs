using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;

public class ThirdPersonUserInput : MonoBehaviour
{
    //public float minDistance = 1.0f;
    //public float maxDistance = 4.0f;
    //public float smoooth = 10.0f;
    //Vector3 dollyDir;
    //public Vector3 dollyDirAdjusted;
    //public float distance;



    public Vector3 offset;
   // public GameObject target;
    float someDistance;
    Vector3 dir;
    public Camera cam;
   // public LayerMask mask;
    public FixedJoystick LeftJoyStick;
   // public FixedButton Button;
    public FixedTouchField TouchField;
    protected ThirdPersonUserControl Control;
    public float x, y, z;
    protected float CameraAngle;
    protected float CameraAngley;
    protected float CameraAngleSpeed = 0.2f;
    bool collided;
    Vector3 camPosition;
    public float smooth = 4.0f;
    // Start is called before the first frame update
    void Start()
    {
        CameraAngley = 0;
         //  playerNewPosition(leave.playerPosition);
         //  Debug.Log(leave.playerPosition);
         Control = GetComponent<ThirdPersonUserControl>();
    }
    //void Awake()
    //{
    //    dollyDir = cam.transform.localPosition.normalized;
    //    distance = cam.transform.localPosition.magnitude;
    //}
    // Update is called once per frame
    void Update()
    {


        // Control.m_Jump = Button.Pressed;
        if (!Stop)
        {
            Control.Hinput = LeftJoyStick.Horizontal;
            Control.Vinput = LeftJoyStick.Vertical;
        }
        else
        {

            Control.Hinput = 0f;
            Control.Vinput = 0f;
        }
    }
    bool Stop = false;
    public void stop()
    {
        Stop = true;
    //    this.gameObject.GetComponent<ThirdPersonCharacter>().Move(new Vector3(0f, 0f, 0f), false, false);
    }
    public void _continue()
    {
        Stop = false;
    }
    private void LateUpdate()
    {



      






       CameraAngle += FixedTouchField.TouchDist.x * CameraAngleSpeed;
         CameraAngley += FixedTouchField.TouchDist.y * CameraAngleSpeed;

         camPosition = transform.position + Quaternion.AngleAxis(CameraAngle, Vector3.up) * offset; //Default

         cam.transform.position = camPosition;

         cam.transform.rotation = Quaternion.LookRotation(transform.position + Vector3.up * 2f - cam.transform.position, Vector3.up);
     

    }

    void smoothCamMethod()
    {

        smooth = 10f;
        cam.transform.position = Vector3.Lerp(cam.transform.position, camPosition, Time.deltaTime * smooth);
    }

    public void playerNewPosition(Vector3 newPosition)
    {
        transform.position = newPosition;
    }
  
}
