using UnityEngine;
using UnityEngine.UIElements;

public class BossTimer : MonoBehaviour
{
    TextElement minutes;
    TextElement seconds;
    TextElement milliSeconds;

    private float timeLeft = 1;

    private bool isRunning = false;
    private bool textIsRed = false;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        minutes = root.Q<TextElement>("Minutes");
        seconds = root.Q<TextElement>("Seconds");
        milliSeconds = root.Q<TextElement>("Milliseconds");
    }

    private void Update()
    {
        if (isRunning)
        {
            timeLeft -= Time.deltaTime;
            UpdateTimerUI(timeLeft);
        }

        // if time left has reached 0
        if(timeLeft <= 0)
        {
            if (textIsRed)
                return;

            minutes.style.color = Color.red;
            seconds.style.color = Color.red;
            milliSeconds.style.color = Color.red;

            textIsRed = true;

            //StopTimer();
            //timeLeft = 0;
            //UpdateTimerUI(timeLeft);
            //GameManager.instance.uiManager.EndGameplay(false);
        }
    }

    public void UpdateTimerUI(float timeSpent)
    {
        minutes.text = Mathf.FloorToInt(timeSpent / 60f).ToString();
        seconds.text = Mathf.FloorToInt(timeSpent % 60f).ToString();
        milliSeconds.text = Mathf.FloorToInt((timeSpent % 1) * 1000).ToString();
    }

    public void StartTimer()
    {
        isRunning = true;
    }

    public void StopTimer()
    {
        isRunning = false;
    }

    public void ResetTimer()
    {
        minutes.style.color = Color.white;
        seconds.style.color = Color.white;
        milliSeconds.style.color = Color.white;

        timeLeft = GameManager.instance.courseManager.GetCurrentBossTimeLimit();
        UpdateTimerUI(timeLeft);
    }
}
