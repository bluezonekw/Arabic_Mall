using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class GetData : MonoBehaviour
{
    public InputField EmailI, PhoneI;
    public Text NameI, AdressI;
    public UnityEngine.UI.Toggle Male, Female;
    public string Adress()
    {

        try
        {
            return ApiClasses.Register.data.user.address;
        }
        catch
        {
            return ApiClasses.Login.data.original.user.address;

        }


    }
    public void Gender()
    {

        try
        {
            if (ApiClasses.Register.data.user.gander =="0")
            {
                Male.isOn = true;

            }
            if (ApiClasses.Register.data.user.gander == "1")
            {
                Female.isOn = true;
            }
        }
        catch
        {

            if (ApiClasses.Login.data.original.user.gander == 0)
            {
                Male.isOn = true;
                Debug.Log("Male hhhhhhhhhhhhhhhhh");

            }
            if (ApiClasses.Login.data.original.user.gander == 1)
            {
                Female.isOn = true;
                Debug.Log("FeMale hhhhhhhhhhhhhhhhh");
            }

        }

    }
    public string UserName()
    {
        try
        {
            return ApiClasses.Register.data.user.name;
        }
        catch
        {
            return ApiClasses.Login.data.original.user.name;

        }
    }
    public string Email()
    {
        try
        {
            return ApiClasses.Register.data.user.email;
        }
        catch
        {
            return ApiClasses.Login.data.original.user.email;
        }
    }
    public string Phone()
    {

        try
        {
            return ApiClasses.Register.data.user.phone;
        }
        catch
        {
            return ApiClasses.Login.data.original.user.phone;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        NameI.GetComponent<ArabicText>().Text =  UserName();
        EmailI.text = Email();
        PhoneI.text = Phone();
        AdressI.text = Adress();
    }
    private void Awake()
    {
        Gender();




    }
    // Update is called once per frame
    void Update()
    {
    }
}
