using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class TimerUIController : MonoBehaviour
{

    private void OnEnable()
    {

        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        TextElement minutes = root.Q<TextElement>("Minutes");

        minutes.text = "12";

    }

}
