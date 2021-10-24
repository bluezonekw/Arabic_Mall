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
    public static string ApiToken, UserName, EmailReg, PhoneReg;
    String JasonString;
    AuthenticationAPi AuthenticationAPi;
    AuthenticationAPiFailed AuthenticationAPiFailed;
    AuthenticationAPiFailedsignup AuthenticationAPiFailedsignup;
    SignUpToApi SignUpToApi;
    string msg;
    public InputField Sign_In_Email, Sign_In_Password, Sign_UP_Email, Sign_Up_Password, NameInput, PhoneInput;
    public GameObject popup, lodaing, LoginObj, SignUpobj, CompleteProfileobj, splash;
    public Toggle Toggle;
    private void Start()
    {
        try {
            SignUpobj.SetActive(false);
            CompleteProfileobj.SetActive(false);
        }
        catch
        {

        }
    }
    private void Update()
    {
        if (!splash.active && !splashfinished)
        {
            LoginObj.SetActive(true);

            splashfinished = true;
        }


        if (popUpFlag)
        {
            StartCoroutine(showPopUp(msg));
            popUpFlag = false;

        }
    }
    IEnumerator showPopUp(string msg)
    {
        print("show popup");
        popup.SetActive(true);
        print("popup : " + popup.active);

        popup.transform.GetChild(0).GetChild(0).gameObject.GetComponent<ArabicText>().Text = msg;
        yield return new WaitForSeconds(0.0f);
    }

    public void startsignup()
    {
        if (string.IsNullOrEmpty(Sign_Up_Password.text) || string.IsNullOrEmpty(Sign_UP_Email.text))
        {
            msg = "ÌÃ» „·¡ «·ÕﬁÊ·";
            popUpFlag = true;
        }
        else
        {
            SignUpobj.SetActive(false);
            CompleteProfileobj.SetActive(true);

        }

    }

    public String NameStore()
    {

        return UserName;

    }
    public String EmailStore()
    {
        return EmailReg;
    }
    public String PhoneStore()
    {

        return PhoneReg;
    }
    public void SignUp()
    {
        if (!Toggle.isOn)
        {
            msg = "ÌÃ» «·„Ê«›ﬁ… ⁄·Ì «·‘—Êÿ Ê«·«Õﬂ«„";
            popUpFlag = true;
            return;
        }
        var client = new RestClient("https://mall.openshoop.com/api/v1/register");
        client.Timeout = -1;
        var request = new RestRequest(Method.POST);
        request.AddHeader("Accept", "application/json");
        request.AlwaysMultipartFormData = true;
        request.AddParameter("name", NameInput.text);
        request.AddParameter("email", Sign_UP_Email.text);
        request.AddParameter("phone", PhoneInput.text);
        request.AddParameter("country_id", "10");
        request.AddParameter("password", Sign_Up_Password.text);
        request.AddParameter("password_confirmation", Sign_Up_Password.text);
        IRestResponse response = client.Execute(request);
        //        Console.WriteLine(response.Content);
        try
        {
            SignUpToApi = JsonConvert.DeserializeObject<SignUpToApi>(response.Content);
            failed = false;
        }
        catch
        {
            try
            {
                AuthenticationAPiFailedsignup = JsonConvert.DeserializeObject<AuthenticationAPiFailedsignup>(response.Content);
                failed = true;

            }
            catch
            {

            }
        }



        if (!failed)
        {
            if (SignUpToApi.state == 0)
            {
                if (string.IsNullOrEmpty(Sign_Up_Password.text) || string.IsNullOrEmpty(Sign_UP_Email.text))
                {
                    msg = "ÌÃ» „·¡ «·ÕﬁÊ·";
                    popUpFlag = true;
                    return;

                }


                else
                {
                    msg = SignUpToApi.msg;
                    popUpFlag = true;
                    return;
                }

            }
            else if (SignUpToApi.state == 1)
            {
                ApiToken = SignUpToApi.data.api_token;
                UserName = SignUpToApi.data.clients.name;
                EmailReg = SignUpToApi.data.clients.email;
                PhoneReg = SignUpToApi.data.clients.phone;
                msg = SignUpToApi.msg;
                popUpFlag = true;
                lodaing.SetActive(true);
                SceneManager.LoadScene("mall");

            }

        }
        
    

}
    public void Login()
    {


        var client = new RestClient("https://mall.openshoop.com/api/v1/login");
        client.Timeout = -1;
        var request = new RestRequest(Method.POST);
        request.RequestFormat = DataFormat.Json;
        request.AddJsonBody(new { password = Sign_In_Password.text, email = Sign_In_Email.text });
        IRestResponse response = client.Execute(request);
        print(response.Content);
        try
        {
            AuthenticationAPi = JsonConvert.DeserializeObject<AuthenticationAPi>(response.Content);
            failed = false;
        }
        catch
        {
            try
            {
                AuthenticationAPiFailed = JsonConvert.DeserializeObject<AuthenticationAPiFailed>(response.Content);
                failed = true;

            }
            catch
            {

            }
        }
        if (!failed)
        {
            if (AuthenticationAPi.msg == "»Ì«‰«  «·„” Œœ„ €Ì— ’ÕÌÕ…")
            {
                if (string.IsNullOrEmpty(Sign_In_Password.text) || string.IsNullOrEmpty(Sign_In_Email.text))
                {
                    msg = "ÌÃ» „·¡ «·ÕﬁÊ·";
                    popUpFlag = true;
                    return;
                }
                else
                {
                    msg = "ÌÊÃœ Œÿ√ »«·≈Ì„Ì·/«·»«”Ê—œ";
                    popUpFlag = true;
                    return;

                }

            }
            else
            {
                ApiToken = AuthenticationAPi.data.api_token;
                UserName = AuthenticationAPi.data.client.name;
                EmailReg = AuthenticationAPi.data.client.email;
                PhoneReg = AuthenticationAPi.data.client.phone;
                lodaing.SetActive(true);
                SceneManager.LoadScene("mall");

            }
        }
        else
        {
            if (AuthenticationAPiFailed.msg == "»Ì«‰«  «·„” Œœ„ €Ì— ’ÕÌÕ…")
            {
                if (string.IsNullOrEmpty(Sign_In_Password.text) || string.IsNullOrEmpty(Sign_In_Email.text))
                {
                    msg = "ÌÃ» „·¡ «·ÕﬁÊ·";
                    popUpFlag = true;
                    return;
                }
                else
                {
                    msg = "ÌÊÃœ Œÿ√ »«·≈Ì„Ì·/«·»«”Ê—œ";
                    popUpFlag = true;
                    return;


                }

            }
          
        
        }


        
    }
   


}