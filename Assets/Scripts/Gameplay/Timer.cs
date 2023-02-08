using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Timer : MonoBehaviour
{

  

    public TextMeshProUGUI timerText;


    private float currentTime;
    private bool countUp;
    
 

    void Update()
    {
      if (countUp ==true)
        {
            currentTime += Time.deltaTime;
            timerText.text = currentTime.ToString();
        }
    }

    public void StartTimer()
    {
        countUp = true;
    }

    public void EndTimer()
    {
       
        countUp = false;
    }
}
