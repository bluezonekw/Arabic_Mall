using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FavoriteSign : MonoBehaviour
{
    private Button CurrentB;
    public bool selected;

    // Start is called before the first frame update
    void Start()
    {
        CurrentB = GetComponent<Button>();

    }
    public void OnselectColor()
    {
        selected = !selected;
    }
    // Update is called once per frame
    void Update()
    {
        if (selected)
        {
            CurrentB.GetComponent<Image>().color = CurrentB.colors.pressedColor;

        }
        else
        {
            CurrentB.GetComponent<Image>().color = Color.white;
        }
    }
}
