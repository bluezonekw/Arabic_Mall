using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ApiClasses : MonoBehaviour
{


    bool popUpFlag, failed, splashfinished;
    public static Login Login;
    public static Register Register;
    String JasonString;
    string msg;
    public InputField Sign_In_Email, Sign_In_Password, Sign_UP_Email, Sign_Up_Password, NameInput, PhoneInput;
    public GameObject popup, lodaing, LoginObj, SignUpobj, CompleteProfileobj, splash;
    public Toggle Polcies,Male,Female;
    public float Waittime;
    public Dropdown dropdown;
    private void Start()
    {
        SaveScript.LoadData();
        if (SaveScript.GameEmail!= null  && SaveScript.GamePassword != null)
        {
            Sign_In_Email.text = SaveScript.GameEmail;
            Sign_In_Password.text = SaveScript.GamePassword;
            Login_To_Mall();

        }
        try
        {
            SignUpobj.SetActive(false);
            CompleteProfileobj.SetActive(false);
        }
        catch
        {

        }
    }
    private void Update()
    {
      
        if (popUpFlag)
        {
            StartCoroutine(showPopUp(msg));
            popUpFlag = false;
        }
        if (Waittime < 0 &&!splashfinished)
        {            splashfinished = true;

            splash.SetActive(false);
            LoginObj.SetActive(true);
        }
        else if (Waittime > 0 )
        {
            splash.SetActive(true);

            Waittime -= Time.deltaTime;


        }
        SaveScript.GameEmail = Sign_In_Email.text;
        SaveScript.GamePassword = Sign_In_Password.text;
    }
    IEnumerator showPopUp(string msg)
    {
        print("show popup");
        popup.SetActive(true);
        print("popup : " + popup.active);

        popup.transform.GetChild(0).GetChild(0).gameObject.GetComponent<ArabicText>().Text = msg;
        yield return new WaitForSeconds(0.0f);
    }

    public void Register_Email_Password()
    {
        if (string.IsNullOrEmpty(Sign_Up_Password.text) || string.IsNullOrEmpty(Sign_UP_Email.text))
        {
            msg = "��� ��� ������";
            popUpFlag = true;
        }
        else
        {
            Sign_In_Email.text = Sign_UP_Email.text;
            Sign_In_Password.text = Sign_Up_Password.text;
            SignUpobj.SetActive(false);
            CompleteProfileobj.SetActive(true);

        }

    }



    public void CompleteRegister()
    {
        if (!Polcies.isOn)
        {
            msg = "��� �������� ��� ������ ��������";
            popUpFlag = true;
            return;
        }
      
        


        var client = new RestClient("https://mall.openshoop.com/api/V1/register");
        client.Timeout = -1;
        var request = new RestRequest(Method.POST);
        request.AddHeader("password_api", "mall_2021_m3m");
        request.AddHeader("lang_api", "ar");
        request.AlwaysMultipartFormData = true;
        request.AddParameter("name", NameInput.text);
        request.AddParameter("email", Sign_UP_Email.text);
        request.AddParameter("password", Sign_Up_Password.text);
        request.AddParameter("password_confirmation", Sign_Up_Password.text);
        request.AddParameter("phone", PhoneInput.text);
        request.AddParameter("address", dropdown.options[dropdown.value].text);

        if (Male.isOn)
        {
            request.AddParameter("gander", "0");


        }

        if (Female.isOn)
        {
            request.AddParameter("gander", "1");



        }
        lodaing.SetActive(true);

        IRestResponse response = client.Execute(request);



        Register = JsonConvert.DeserializeObject<Register>(response.Content);

        print(NameInput.text + "//" + Sign_UP_Email.text + "///" + PhoneInput.text + "///" + Sign_Up_Password.text + "//"+ dropdown.options[dropdown.value].text);





        print("REgisterStatues   :" + Register.statsu+"    / MEssage  :   "+Register.message);

        if (Register.statsu == 0)
        {




            msg = Register.message;
            popUpFlag = true;
            lodaing.SetActive(false);
            return;


        }
        if (Register.statsu == 1)
        {
            msg = Register.message;
            popUpFlag = true;
            SaveScript.SaveData();
            SceneManager.LoadScene("mall");

        }

    }




    public void Login_To_Mall()
    {

        var client = new RestClient("https://mall.openshoop.com/api/V1/login");
        client.Timeout = -1;
        var request = new RestRequest(Method.POST);
        request.AddHeader("password", "mall_2021_m3m");
        request.AddHeader("lang_api", "ar");
        request.AlwaysMultipartFormData = true;
        request.AddParameter("email", Sign_In_Email.text);
        request.AddParameter("password", Sign_In_Password.text);
        lodaing.SetActive(true);
        IRestResponse response = client.Execute(request);
        Login = JsonConvert.DeserializeObject<Login>(response.Content);


        if (string.IsNullOrEmpty(Sign_In_Password.text) || string.IsNullOrEmpty(Sign_In_Email.text))
        {
            msg = "��� ��� ������";
            popUpFlag = true;
            return;
        }

        if (Login.statsu == 0)
        {

            msg = Login.message;
            popUpFlag = true;
            lodaing.SetActive(false);
            return;

        }


        if (Login.statsu == 1)
        {

            SaveScript.SaveData();
            SceneManager.LoadScene("mall");

        }









    }
}