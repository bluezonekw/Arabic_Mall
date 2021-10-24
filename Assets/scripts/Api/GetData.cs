using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GetData : MonoBehaviour
{
    public InputField  EmailI;
    public Text NameI, PhoneI;
    public string UserName()
    {

        return ApiClasses.UserName;
    }
    public string Email()
    {
        return ApiClasses.EmailReg;
    }
    public string Phone()
    {

        return ApiClasses.PhoneReg;
    }
    // Start is called before the first frame update
    void Start()
    {
            NameI.text = UserName();
            EmailI.text = Email();
            PhoneI.text = Phone();
    }

    // Update is called once per frame
    void Update()
    {

    }
}
