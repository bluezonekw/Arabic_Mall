using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SelectColor : MonoBehaviour
{
    // Start is called before the first frame update
    public Button B1, B2, B3;
    private Button CurrentB;
    public bool selected;
    void Start()
    {
        CurrentB = GetComponent<Button>();
    }
    public void OnselectColor()
    {
        selected = true;
    }
    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            CurrentB.GetComponent<Image>().color = Color.green;
            B1.GetComponent<Image>().color = Color.white;
            B2.GetComponent<Image>().color = Color.white;
            B3.GetComponent<Image>().color = Color.white;
            selected = false;
        }
        
    }
}
