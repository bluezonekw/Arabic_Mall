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
            return ApiClasses.Register.data.client.address;
        }
        catch
        {
            return ApiClasses.Login.data.client.address;

        }


    }
    public void Gender()
    {

        try
        {
            if (ApiClasses.Register.data.client.gander ==0)
            {
                Male.isOn = true;
            }
            if (ApiClasses.Register.data.client.gander == 1)
            {
                Female.isOn = true;
            }
        }
        catch
        {

            if (ApiClasses.Login.data.client.gander == 0)
            {
                Male.isOn = true;

            }
            if (ApiClasses.Register.data.client.gander == 1)
            {
                Female.isOn = true;
            }

        }

    }
    public string UserName()
    {
        try
        {
            return ApiClasses.Register.data.client.name;
        }
        catch
        {
            return ApiClasses.Login.data.client.name;

        }
    }
    public string Email()
    {
        try
        {
            return ApiClasses.Register.data.client.email;
        }
        catch
        {
            return ApiClasses.Login.data.client.email;
        }
    }
    public string Phone()
    {

        try
        {
            return ApiClasses.Register.data.client.phone;
        }
        catch
        {
            return ApiClasses.Login.data.client.phone;
        }
    }
    // Start is called before the first frame update
    void Start()
    {
        NameI.GetComponent<ArabicText>().Text =  UserName();
        EmailI.text = Email();
        PhoneI.text = Phone();
        Gender();
        AdressI.text = Adress();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
