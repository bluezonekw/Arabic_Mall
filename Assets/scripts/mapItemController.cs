using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class mapItemController : MonoBehaviour
{
    public void gotoShop(Text id)
    {
        GameObject mapController = GameObject.FindGameObjectWithTag("mapController");
        mapController.GetComponent<mapController>().movePlayer(int.Parse(id.text));
    }
}
