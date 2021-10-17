using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class collisionController : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        transform.parent.gameObject.GetComponent<gateController>().gate(this.gameObject);
    }
   
}
