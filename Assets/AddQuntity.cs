using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AddQuntity : MonoBehaviour
{
    int quntity = 1;
    Text text1;
    // Start is called before the first frame update
    void Start()
    {
        text1 = GetComponent<Text>();
        text1.text = quntity.ToString();
    }
    public void add()
    {
        quntity++;
        text1.text = quntity.ToString();


    }
    public void Minus() { 
    if(quntity!=1){
        quntity--;
        text1.text = quntity.ToString();

        }


    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
