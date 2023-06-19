using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;

public class CourseFinish : MonoBehaviour
{
    private bool hasFinished = false;

    private void Start()
    {

    }

    /// <summary>
    /// Event triggered when the player enters the trigger zone.
    /// </summary>
    /// <param name="other">The collider of the other object.</param>
    private void OnTriggerEnter(Collider other)
    {
        if (hasFinished)
            return;

        if (other.CompareTag("Player"))
        {
            try
            {
                var bossTimer = FindObjectOfType<BossTimer>();
                if (bossTimer != null)
                {
                    GameManager.instance.uiManager?.EndGameplay(bossTimer.GetTimeLeft() < 0 ? false : true);
                }
                else
                {
                    // Stop the timer and handle end of gameplay stuff
                    GameManager.instance.uiManager?.EndGameplay(true);
                }
            }
            catch (Exception e)
            {
                Debug.Log("No GameManager found");
                Debug.Log(e);
            }

            hasFinished = true;
        }
    }
}
