using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class confirmationPaymentController : MonoBehaviour
{
    public InputField email;
    public InputField customerName;
    public InputField mobile;
    public InputField city;
    public InputField square;
    public InputField street;
    public InputField apartment;
    public GameObject popup;
    public ArabicText cost;
    public ArabicText shipping;
    public ArabicText total;
    string token = "";
    bool finished = true;
    private void OnEnable()
    {
      
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("MallFullName")))
        {
            customerName.text = PlayerPrefs.GetString("MallFullName");
        }

        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("MallPhoneNumber")))
        {
            mobile.text = PlayerPrefs.GetString("MallPhoneNumber");
        }
        if (!string.IsNullOrEmpty(PlayerPrefs.GetString("MallEmail")))
        {
            email.text = PlayerPrefs.GetString("MallEmail");
        }
        print(PlayerPrefs.GetString("MallCost"));
        print(PlayerPrefs.GetString("MallShipping"));
        cost.Text = PlayerPrefs.GetString("MallCost");
        shipping.Text = PlayerPrefs.GetString("MallShipping");
        total.Text = (float.Parse(PlayerPrefs.GetString("MallCost")) + float.Parse(PlayerPrefs.GetString("MallShipping"))).ToString();

    }  
    public void payNow(){
        if (!string.IsNullOrEmpty(mobile.text) && !string.IsNullOrEmpty(email.text) && !string.IsNullOrEmpty(customerName.text) && !string.IsNullOrEmpty(city.text) && !string.IsNullOrEmpty(square.text) && !string.IsNullOrEmpty(street.text) && !string.IsNullOrEmpty(apartment.text))
        {
            if (checkNumber(mobile.text))
            {
                if(IsValid(email.text))
                {

                    StartCoroutine(pay());
                }
                else
                {
                    popup.transform.GetChild(0).GetChild(0).gameObject.GetComponent<ArabicText>().Text = "يجب إدخال إيميل صحيح";
                    popup.SetActive(true);

                }
            }
            else
            {
                popup.transform.GetChild(0).GetChild(0).gameObject.GetComponent<ArabicText>().Text = "يجب أن تكون صيغة رقم الهاتف لا تقل عن 8 أرقام";
                popup.SetActive(true);
            }
        }
        else
        {

            popup.transform.GetChild(0).GetChild(0).gameObject.GetComponent<ArabicText>().Text = "يجب ملء جميع الحقول";
            popup.SetActive(true);
        }
    }
    public void generateToken()
    {
        finished = false;

        Firebase.Auth.FirebaseAuth auth;
        auth = Firebase.Auth.FirebaseAuth.DefaultInstance;
        Firebase.Auth.FirebaseUser user = auth.CurrentUser;

        user.TokenAsync(true).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                Debug.LogError("TokenAsync was canceled.");
                return;
            }

            if (task.IsFaulted)
            {
                Debug.LogError("TokenAsync encountered an error: " + task.Exception);
                return;
            }
            finished = true;
            string idToken = task.Result;
            token = idToken;



        });

    }
    IEnumerator pay()
    {

        while (!finished)
        {
            yield return new WaitForSeconds(0.1f);

        }
        WWWForm form = new WWWForm();
        form.AddField("mobile", mobile.text);
        form.AddField("method_id", PlayerPrefs.GetString("paymentMethodId"));
        form.AddField("name", customerName.text);
        form.AddField("email", email.text);
        form.AddField("square", square.text);
        form.AddField("street", street.text);
        form.AddField("apartment", apartment.text);
        form.AddField("city", city.text);

        if (token != "")
        {
            PlayerPrefs.SetString("MallTokenId", token);
        }

        using (UnityWebRequest www = UnityWebRequest.Post(hostManager.domain + "api/orders/pay/", form))
        {

            www.SetRequestHeader("Content-Type", "application/x-www-form-urlencoded");
            www.SetRequestHeader("Authorization", PlayerPrefs.GetString("MallTokenId"));
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                generateToken();
                StartCoroutine(pay());
                Debug.Log(www.error);
                Debug.Log(www.downloadHandler.text);
            }
            else
            {
                print(www.downloadHandler.text);
                Response res= JsonUtility.FromJson<Response>(www.downloadHandler.text);
                print(res.Data.PaymentURL);
                browser.GetComponent<BrowserOpener>().reopenBrowser(res.Data.PaymentURL);
                payment.SetActive(false);
            }
        }


    }
    public GameObject browser;
    public GameObject payment;
    bool checkNumber(string number)
    {
        if (number.Length == 8 )
        {
            return true;
        }
        return false;
    }


    public bool IsValid(string emailaddress)
    {
        if (!string.IsNullOrEmpty(emailaddress))
        {
            Regex regex = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
            Match match = regex.Match(emailaddress);
            if (match.Success)
                return true;
            else
                return false;
        }
        return false;
    }
    [System.Serializable]
    public class Response
    {
        public bool IsSuccess;
        public string Message;
        public string ValidationErrors;
        public result Data;
    }

    [System.Serializable]
    public class result
    {
        public int InvoiceId;
        public bool IsDirectPayment;
        public string PaymentURL;
        public int CustomerReference;
        public string UserDefinedField;
    }
}
