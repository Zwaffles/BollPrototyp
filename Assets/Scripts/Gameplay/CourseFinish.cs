using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CourseFinish : MonoBehaviour
{
    private void Start()
    {
        GameManager.instance.uiManager.StartGameplay();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Stop the timer and handle end of gameplay stuff
            GameManager.instance.uiManager.EndGameplay(true);
        }
    }
}
