using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class MainMenu : MonoBehaviour
{

    private VisualElement root;

    private VisualElement playButton;
    private VisualElement optionsButton;
    private VisualElement creditsButton;
    private VisualElement quitButton;

    private float ignoreInputTime;
    private bool inputEnabled;

    private InputReader input;

    private void OnEnable()
    {
        inputEnabled = false;

        input = GameManager.instance.Input;
        input.AddSubmitEventListener(Submit);

        root = GetComponent<UIDocument>().rootVisualElement;

        playButton = root.Q<VisualElement>("UI_MM_PlayButton_Box");
        optionsButton = root.Q<VisualElement>("UI_MM_OptionsButton_Box");
        creditsButton = root.Q<VisualElement>("UI_MM_CreditsButton_Box");
        quitButton = root.Q<VisualElement>("UI_MM_QuitButton_Box");

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

        if (focusedElement == playButton)
        {
            GameManager.instance.uiManager.ToggleSetSelectMenu(true);
            gameObject.SetActive(false);
        }

        if (focusedElement == optionsButton)
        {
            GameManager.instance.uiManager.ToggleOptionsMenu(true);
            gameObject.SetActive(false);
        }

        if (focusedElement == creditsButton)
        {
            GameManager.instance.uiManager.ToggleCreditMenu(true);
            gameObject.SetActive(false);
        }

        if (focusedElement == quitButton)
        {
            Application.Quit();
        }

    }

    public Focusable GetFocusedElement()
    {
        return root.focusController.focusedElement;
    }

}
