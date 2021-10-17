using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class standController : MonoBehaviour
{
    public GameObject item;
   /* private void OnMouseDown()
    {
        item.SetActive(true);

        item.GetComponent<itemController>().showItems(this.gameObject.name);
    }*/
    public void openItem(GameObject stand)
    {
        print(this.gameObject.name);
        item.SetActive(true);

        item.GetComponent<itemController>().showItems(this.gameObject.name);
    }
}
