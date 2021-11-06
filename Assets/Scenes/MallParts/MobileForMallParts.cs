using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MobileForMallParts : MobileDisableAutoSwitchControls
{
    [SerializeField]
    public static PlayerInput mobile;
    // Start is called before the first frame update
    void Start()
    {
        try {
            
            mobile = GameObject.FindWithTag("Player").GetComponent<PlayerInput>();
            print(mobile);
            playerInput = mobile;


        }
        catch
        {
            print("›‘·       ");
        }
        }

    // Update is called once per frame
    void Update()
    {

    }
}
