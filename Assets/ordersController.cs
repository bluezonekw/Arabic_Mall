using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ordersController : MonoBehaviour
{
    public GameObject openContent;
    public GameObject closeContent;
     public void OnEnable()
     {
       
        closedScroll.SetActive(false);
         openScroll.SetActive(true);
         closedbar.SetActive(false);
         openBar.SetActive(true);
        
        for (int i = openContent.transform.childCount - 1; i >= 0; i--)
         {
             Destroy(openContent.transform.GetChild(i).gameObject);
         }
         for (int i = closeContent.transform.childCount - 1; i >= 0; i--)
         {
             Destroy(closeContent.transform.GetChild(i).gameObject);
         }
         
         showOrders(hostManager.domain + "api/orders/");
     }
    public void selectSection(int index)
    {
        if (index == 0)
        {
            openScroll.SetActive(true);
            closedScroll.SetActive(false);
            openBar.SetActive(true);
            closedbar.SetActive(false);
        }
        else if (index == 1)
        {
            openScroll.SetActive(false);
            closedScroll.SetActive(true);
            openBar.SetActive(false);
            closedbar.SetActive(true);
        }
       

    }
    public void showOrders(string url)
    {
        StartCoroutine(getOrders(url));
    }
    bool finished = true;
    string token = "";
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
    IEnumerator getOrders(string url)
    {
        while (!finished)
        {
            yield return new WaitForSeconds(0.1f);

        }
        if (token != "")
        {
            PlayerPrefs.SetString("MallTokenId", token);
        }

        using (UnityWebRequest www = UnityWebRequest.Get(url))
        {

            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization",  PlayerPrefs.GetString("MallTokenId"));
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                generateToken();
                showOrders(hostManager.domain + "api/orders/");
                Debug.Log(www.error);
            }
            else

            {
                responseOrders res = JsonUtility.FromJson<responseOrders>(www.downloadHandler.text);
                //fillPaymentMethod(res.Data.PaymentMethods);
                iterateOrders(res.results);
                if (!string.IsNullOrEmpty(res.next))
                {
                    showOrders(res.next);
                }
            }
        }



    }
    public GameObject orderItem;
    public GameObject closedScroll, openScroll;
    public GameObject closedbar, openBar ;

    private void iterateOrders(order[] results)
    {

        for (int i = 0; i < results.Length; i++)
        {
            addOrderToView(results[i]);
        }
    }
    private void addOrderToView(order ord)
    {
        GameObject item = Instantiate(orderItem);

        if (ord.is_closed)
        {
            item.transform.parent = closeContent.transform;
            item.transform.GetChild(0).GetComponent<ArabicText>().Text = "تم التوصيل";

        }
        else 
        {
            item.transform.parent = openContent.transform;
            item.transform.GetChild(0).GetComponent<ArabicText>().Text = "جارى التوصيل";

        }

        item.transform.GetChild(1).GetChild(0).GetComponent<ArabicText>().Text = "رقم الفاتورة : "+ord.invoice_id;
        item.transform.GetChild(1).GetChild(1).GetComponent<ArabicText>().Text = "إجمالى السعر :  : "+ord.total_price;
      


    }

    [System.Serializable]
    public class responseOrders
    {
        public int count;
        public string next;
        public string previous;
        public order[] results;

    }

    [System.Serializable]
    public class order
    {
        public string id;
        public bool is_closed;
        public string total_price;
        public string invoice_id;
    }



}
