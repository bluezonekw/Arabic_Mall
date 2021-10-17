using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class passwordController : MonoBehaviour
{
    bool password;
    private void Start()
    {
        password = true;
    }
    public void changeContentType(Text placeHolder)
    {
        print(password);
        if (password)
        {
            this.GetComponent<InputField>().contentType = InputField.ContentType.Standard;
            placeHolder.text = this.GetComponent<InputField>().text;

            print(placeHolder.text);
            password = false;
        }
        else
        {
            placeHolder.text = "";
            this.GetComponent<InputField>().contentType = InputField.ContentType.Password;
            for (int i = 0; i < this.GetComponent<InputField>().text.Length; i++)
            {
                placeHolder.text += "*";
            }
            password = true;
        }
    }
}
