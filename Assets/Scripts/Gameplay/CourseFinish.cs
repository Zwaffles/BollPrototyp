using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class CourseFinish : MonoBehaviour
{
    private bool hasFinished = false;

    private void Start()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasFinished)
            return;

        if (other.CompareTag("Player"))
        {
            try
            {
                var bossTimer = FindObjectOfType<BossTimer>();
                if(bossTimer != null)
                {
                    GameManager.instance.uiManager?.EndGameplay(bossTimer.GetTimeLeft() < 0 ? false : true);
                }

                else
                {
                    // Stop the timer and handle end of gameplay stuff
                    GameManager.instance.uiManager?.EndGameplay(true);
                }
            }
            catch
            {
                Debug.Log("No GameManager found");
            }

            hasFinished = true;
        }
    }
}
