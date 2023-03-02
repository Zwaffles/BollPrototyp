using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CourseFinish : MonoBehaviour
{
    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //try
            //{
            //    // Stop the timer and handle end of gameplay stuff
            //    GameManager.instance.uiManager?.EndGameplay(true);
            //}
            //catch
            //{
            //    Debug.Log("No GameManager found");
            //}
            GameManager.instance.uiManager?.EndGameplay(true);
        }
    }
}
