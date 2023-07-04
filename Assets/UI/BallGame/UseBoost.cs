using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class UseBoost: MonoBehaviour
{
    private VisualElement _Boost1;
    private VisualElement _Boost2;
    private VisualElement _Boost3;

    private const float GREY_COLOR = 44 / 255;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        _Boost1 = root.Q<VisualElement>("UI_InGame_Boosts_Icon1");
        _Boost2 = root.Q<VisualElement>("UI_InGame_Boosts_Icon2");
        _Boost3 = root.Q<VisualElement>("UI_InGame_Boosts_Icon3");
    }

    public void playBoostUIAnimation(int number)
    {
        
        switch (number)
        {

            case 1:
                UseBoost1();
                break;
            case 2:
                UseBoost2();
                break;
            case 3:
                UseBoost3();
                break;
            default:
                break;

        }

    }

    private void UseBoost1()
    {
        _Boost1.style.backgroundColor = new Color(GREY_COLOR, GREY_COLOR, GREY_COLOR, 1);
    }

    private void UseBoost2()
    {
        _Boost2.style.backgroundColor = new Color(GREY_COLOR, GREY_COLOR, GREY_COLOR, 1);
    }

    private void UseBoost3()
    {
        _Boost3.style.backgroundColor = new Color(GREY_COLOR, GREY_COLOR, GREY_COLOR, 1);
    }
}