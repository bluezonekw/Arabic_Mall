using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckEnterShop : MonoBehaviour
{
    CharacterController CC;
    public static bool EnterShop,ExitShop;
    // Start is called before the first frame update
    void Start()
    {
       
            CC = GetComponent<CharacterController>();
       

       
    }
    // Update is called once per frame
    void Update()
    {

    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.tag == "FloorShop"&&!ExitShop)
        {
            print(hit.gameObject.transform.parent);
            EnterShop = true;
        }
     
        if (EnterShop&& hit.gameObject.tag == "Gate" )
        {
            EnterShop = false;
            ExitShop = true;


        }
                }
    
}
