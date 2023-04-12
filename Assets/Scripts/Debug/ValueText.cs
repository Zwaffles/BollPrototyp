using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class ValueText : MonoBehaviour
{
    private TextMeshProUGUI valueText;
    [SerializeField] private Slider slider;

    private void Awake()
    {
        valueText = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        valueText.text = slider.value.ToString();
    }

    public void UpdateText()
    {
        var sliderValue = Mathf.Round(slider.value * 100f) / 100f;

        valueText.text = sliderValue.ToString();
    }
}
