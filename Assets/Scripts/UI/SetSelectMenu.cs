using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class SetSelectMenu : MonoBehaviour
{

    private VisualElement root;

    private VisualElement set1Button;
    private VisualElement set2Button;
    private VisualElement set3Button;

    private float ignoreInputTime;
    private bool inputEnabled;

    private InputReader input;

    private void OnEnable()
    {
        inputEnabled = false;

        input = GameManager.instance.Input;
        input.AddSubmitEventListener(Submit);

        root = GetComponent<UIDocument>().rootVisualElement;

        set1Button = root.Q<VisualElement>("UI_SS_Set_Box");
        set2Button = root.Q<VisualElement>("UI_SS_Set2_Box");
        set3Button = root.Q<VisualElement>("UI_SS_Set3_Box");

        //FocusFirstElement(playButton);
        ignoreInputTime = Time.time + .25f;

    }

    private void Update()
    {
        if (Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

    public void Submit()
    {
        if (!inputEnabled)
            return;

        if (!gameObject.activeInHierarchy)
            return;

        var focusedElement = GetFocusedElement();

        if (focusedElement == set1Button)
        {
            GameManager.instance.uiManager.ToggleLevelSelectMenu(true);
            gameObject.SetActive(false);
        }

        if (focusedElement == set2Button)
        {
            GameManager.instance.uiManager.ToggleLevelSelectMenu(true);
            gameObject.SetActive(false);
        }

        if (focusedElement == set3Button)
        {
            GameManager.instance.uiManager.ToggleLevelSelectMenu(true);
            gameObject.SetActive(false);
        }

    }

    public Focusable GetFocusedElement()
    {
        return root.focusController.focusedElement;
    }

}
