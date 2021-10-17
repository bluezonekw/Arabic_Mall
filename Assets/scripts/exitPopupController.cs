using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityStandardAssets.Characters.ThirdPerson;
public class exitPopupController : MonoBehaviour
{
    string token = "";
    bool finished = true;
    [HideInInspector]
    public GameObject shop;

    // Start is called before the first frame update
    void OnEnable()
    {
       
        


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
    public void clearCart()
    {
        StartCoroutine(clearCartItems());
    }

    IEnumerator clearCartItems()
    {
        while (!finished)
        {
            yield return new WaitForSeconds(0.1f);

        }
        if (token != "")
        {
            PlayerPrefs.SetString("MallTokenId", token);
        }
        using (UnityWebRequest www = UnityWebRequest.Delete(hostManager.domain + "api/clear-cart/"))
        {
           
            www.SetRequestHeader("Content-Type", "application/json");
            www.SetRequestHeader("Authorization", PlayerPrefs.GetString("MallTokenId"));
            yield return www.SendWebRequest();
            if (www.isNetworkError || www.isHttpError)
            {
                generateToken();
                clearCart();
                Debug.Log(www.error);
            }
            else

            {
                Debug.Log(www.responseCode);
                if (www.responseCode == 204)
                {

                    GameObject counter = GameObject.FindGameObjectWithTag("carFavControllerCounter");
                    counter.GetComponent<FavCartCountercontroller>().reCall();
                    cart.GetComponent<cartController>().fromExit = false; ;
                    this.gameObject.SetActive(false);
                    header.SetActive(true);
                    if (shop != null)
                    {
                        shop.GetComponent<gateController>().Exit();
                    }
                    //     joystick._continue();
                }
            }
        }
    }
    public GameObject header;
   // public ThirdPersonUserInput joystick;
    public GameObject cart;
    public void showCart()
    {
        this.gameObject.SetActive(false);
        cart.SetActive(true);

        cart.GetComponent<cartController>().fromExit=true;

    }

}
