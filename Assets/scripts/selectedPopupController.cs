using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class selectedPopupController : MonoBehaviour
{
    public void select(ArabicText value)
    {
        this.transform.parent.parent.parent.parent.parent.gameObject.GetComponent<selectedItemController>().setSize(value.Text);
    }
    public void selectColor(ArabicText value)
    {
        this.transform.parent.parent.parent.parent.parent.gameObject.GetComponent<selectedItemController>().setColor(value.Text);
    }
}
