using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationForFrame : MonoBehaviour
{
    public bool Paused;
    public Animator []  animat;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Paused)
        {
            foreach(Animator a in animat)
            {

                a.speed = 0;

            }
        }
        else
        {
            foreach (Animator a in animat)
            {

                a.speed = 1;

            }
        }

    }

    public void StopAnimation()
    {
        Paused = !Paused;

    }
}
