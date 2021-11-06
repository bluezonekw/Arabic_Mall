using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scrollOffset : MonoBehaviour
{
    // Scroll main texture based on time

public RectTransform T1,T2;
    float reset = -2.6f;

    Vector2 resetposition;

    void Start()
    {
        resetposition = T2.anchoredPosition;
    }

    void Update()
    {
        if (T1.anchoredPosition.x < reset)
        {

            T1.anchoredPosition = resetposition;

        }
        else
        {
            try {
                T1.anchoredPosition = new Vector2(T1.anchoredPosition.x- .6f * Time.deltaTime, T1.anchoredPosition.y);
            }
            catch
            {


            }
            }


        if (T2.anchoredPosition.x < reset)
        {

            T2.anchoredPosition = resetposition;
        }
        else
        {
            try {

                T2.anchoredPosition = new Vector2(T2.anchoredPosition.x - .6f * Time.deltaTime, T2.anchoredPosition.y);
            }
            catch
            {


            }

            }


    }
}
