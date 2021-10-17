using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class paymentMethodItemController : MonoBehaviour
{
    public void gotoPayment(Text id)
    {
        PlayerPrefs.SetString("paymentMethodId",id.text);
        GameObject paymentController = GameObject.FindGameObjectWithTag("paymentController");
        paymentController.transform.GetChild(0).gameObject.SetActive(true);
        //paymentController.GetComponent<paymentController>().proceed(id.text);
        GameObject paymentMethod = GameObject.FindGameObjectWithTag("paymentMethod");
        paymentMethod.SetActive(false);
    }
}
