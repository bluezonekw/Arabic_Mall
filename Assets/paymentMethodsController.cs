using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class paymentMethodsController : MonoBehaviour
{
    public GameObject loading;
    public GameObject content;
    public GameObject container;
    public GameObject paymentMethod;
    public GameObject paymentMethodItem;
    bool finished = true;
    string token = "";
    public GameObject mall;
    public GameObject cart;
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
    public void back()
    {

        if (cart.GetComponent<cartController>().fromExit)
        {
            cart.SetActive(true);
        }
            gameObject.SetActive(false);;
        
    }
    private void OnEnable()
    {
        mall.SetActive(false);
        paymentMethod.SetActive(true);
        container.SetActive(false);
        for (int i = content.transform.childCount - 1; i >= 0; i--)
        {
            Destroy(content.transform.GetChild(i).gameObject);
        }
        loading.SetActive(true);
      
        showMethods(hostManager.domain + "api/orders/payment-methods/");
    }

    public void showMethods(string url)
    {
        StartCoroutine(getMethods(url));
    }

    IEnumerator getMethods(string url)
    {
        while (!finished)
        {
            yield return new WaitForSeconds(0.1f);

        }
        if (token != "")
        {
            PlayerPrefs.SetString("MallTokenId", token);
        }
        print(PlayerPrefs.GetString("MallTokenId"));
        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {

            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", PlayerPrefs.GetString("MallTokenId"));
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {

                generateToken();
                showMethods(hostManager.domain + "api/orders/payment-methods/");
                Debug.Log(www.error);
            }
            else

            {
                responseMethods res = JsonUtility.FromJson<responseMethods>(www.downloadHandler.text);
                fillPaymentMethod(res.Data.PaymentMethods);

                Debug.Log(res.Data.PaymentMethods[0].ImageUrl);
            }
        }
        
        

    }
    void fillPaymentMethod(paymentMethods[] PaymentMethods)
    {
        for (int i = 0; i < PaymentMethods.Length; i++)
        {
            addItemToMethod(PaymentMethods[i]);
        }
        loading.SetActive(false);

    }
    private void addItemToMethod(paymentMethods method)
    {
        GameObject item = Instantiate(paymentMethodItem);
        item.transform.parent = content.transform;

        item.transform.GetChild(0).GetComponent<Text>().text = method.PaymentMethodId.ToString();
        int openBracket = method.PaymentMethodAr.IndexOf('(');
        int clossedBracket = method.PaymentMethodAr.IndexOf(')');
        char[] ch = method.PaymentMethodAr.ToCharArray();

        if (openBracket!=-1) 
            ch[openBracket] = ')'; // index starts at 0!
        if (clossedBracket != -1)
            ch[clossedBracket] = '('; // index starts at 0!
        method.PaymentMethodAr = new string(ch);

        item.transform.GetChild(1).GetComponent<ArabicText>().Text = method.PaymentMethodAr;

        StartCoroutine(GetTextureRaw(method.ImageUrl, item.transform.GetChild(2).GetComponent<RawImage>()));

    }
    IEnumerator GetTextureRaw(string url, RawImage rawImage)
    {
        Resources.UnloadUnusedAssets();
        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);
        yield return www.SendWebRequest();

        if (www.isNetworkError || www.isHttpError)
        {
            Debug.Log(www.error);
        }
        else
        {

            if (rawImage.texture)
            {
                Destroy(rawImage.texture);
            }

            rawImage.texture = DownloadHandlerTexture.GetContent(www);
        }

        /* DestroyImmediate(((DownloadHandlerTexture)www.downloadHandler).texture);
         www.Dispose();
         Resources.UnloadUnusedAssets();
         www = null;*/
    }
    [System.Serializable]
    public class responseMethods
    {
        public bool IsSuccess;
        public string Message;
        public string ValidationErrors;
        public string price;
        public dataMethods Data;

    }

    [System.Serializable]
    public class dataMethods
    {
        public paymentMethods[] PaymentMethods;
    }







        [System.Serializable]
    public class paymentMethods
    {
        public int PaymentMethodId;
        public string PaymentMethodAr;
        public string PaymentMethodEn;
        public string PaymentMethodCode;
        public bool IsDirectPayment;
        public float ServiceCharge;
        public int TotalAmount;
        public string CurrencyIso;
        public string ImageUrl;

    }

}
