using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OwnThirdPersonController : MonoBehaviour {


    private Player playerinput;
 //   public FixedTouchField TouchField;
    protected Rigidbody Rigidbody;

    protected Actions Actions;
    public float CameraAngleY;
    protected float CameraAngleSpeed = 0.1f;
    protected float CameraPosY = 3f;
    protected float CameraPosSpeed = 0.02f;

    protected bool isCrouching = false;
    protected CapsuleCollider CapCollider;

    //car
    protected SkinnedMeshRenderer Renderer;

    protected float Cooldown;


    private void Awake()
    {
        playerinput = new Player();

    }
    private void OnEnable()
    {
        playerinput.Enable();
    }
    private void OnDisable()
    {

        playerinput.Disable();
    }

    // Use this for initialization
    void Start ()
    {
       
        // Camera.main.cullingMask = 1 << 0;
        maxCameraDistance = Camera.main.transform.localPosition.z-.5f;

        Actions = GetComponent<Actions>();
        Rigidbody = GetComponent<Rigidbody>();
    }
    public float walkSpeed=5f;
	// Update is called once per frame
	void Update () {
          Walk();
    }
    [Header("Third person camera")]
    [SerializeField] private Transform cameraPole;
    [Header("Third person camera settings")]
    [SerializeField] private LayerMask cameraObstacleLayers;
    private float maxCameraDistance;
    private void MoveCamera()
    {

        Vector3 rayDir = Camera.main.transform.position - cameraPole.position;
        
        
        Debug.DrawRay(cameraPole.position, rayDir, Color.red);
        // Check if the camera would be colliding with any obstacle


        if (Physics.Raycast(cameraPole.position, rayDir, out RaycastHit hit, Mathf.Abs(maxCameraDistance-1f), cameraObstacleLayers))
        {
            // Camera.main.transform.position = cameraPole.position   + Quaternion.AngleAxis(CameraAngleY, Vector3.up) * hit.point;
            Camera.main.transform.position = hit.point;
            Camera.main.transform.rotation = Quaternion.LookRotation(transform.position + Vector3.up * 2f - Camera.main.transform.position, Vector3.up);
        }
        else
        {
            // Move the camera to the max distance on the local z axis
            Camera.main.transform.position = transform.position + Quaternion.AngleAxis(CameraAngleY, Vector3.up) * new Vector3(0, CameraPosY + 1f, -5.5f);
            Camera.main.transform.rotation = Quaternion.LookRotation(transform.position + Vector3.up * 2f - Camera.main.transform.position, Vector3.up);

        }
    }
    private void Walk()
    {
         var input = new Vector3(playerinput.PlayerMan.MoveAround.ReadValue<Vector2>().x, 0, playerinput.PlayerMan.MoveAround.ReadValue<Vector2>().y);
        var vel = Quaternion.AngleAxis(CameraAngleY, Vector3.up) * input * walkSpeed;
        
        transform.rotation = Quaternion.AngleAxis(CameraAngleY + Vector3.SignedAngle(Vector3.forward, input.normalized + Vector3.forward * 0.0001f, Vector3.up) , Vector3.up);
       
        Rigidbody.velocity = new Vector3(vel.x, Rigidbody.velocity.y, vel.z );

      //  CameraAngleY += FixedTouchField.TouchDist.x * CameraAngleSpeed;
    //   CameraPosY = Mathf.Clamp(CameraPosY - FixedTouchField.TouchDist.y * CameraPosSpeed, 0, 5f);
     
        Camera.main.transform.position = transform.position + Quaternion.AngleAxis(CameraAngleY, Vector3.up) * new Vector3(0, CameraPosY+1f, -5.5f);
        Camera.main.transform.rotation = Quaternion.LookRotation(transform.position + Vector3.up * 2f - Camera.main.transform.position, Vector3.up);
        MoveCamera();
        if (Mathf.Abs(playerinput.PlayerMan.MoveAround.ReadValue<Vector2>().x) > Mathf.Abs(playerinput.PlayerMan.MoveAround.ReadValue<Vector2>().y))
        {

            Actions.Walk(Mathf.Abs(playerinput.PlayerMan.MoveAround.ReadValue<Vector2>().x));
        }
        else
        {
            Actions.Walk(Mathf.Abs(playerinput.PlayerMan.MoveAround.ReadValue<Vector2>().y));

        }
    }

  
}
