using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndTimer : MonoBehaviour
{

    private void OnTriggerEnter(Collider other)
    {
        GameObject.Find("Ball").SendMessage("EndTimer");
        
    }
   
}
