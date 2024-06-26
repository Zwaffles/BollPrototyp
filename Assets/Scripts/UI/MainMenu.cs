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

        playButton.RegisterCallback<NavigationMoveEvent>(e =>
         {
             switch (e.direction)
             {
                 case NavigationMoveEvent.Direction.Up: playButton.Focus(); break;
                 case NavigationMoveEvent.Direction.Down: creditsButton.Focus(); break;
                 case NavigationMoveEvent.Direction.Left: playButton.Focus(); break;
                 case NavigationMoveEvent.Direction.Right: optionsButton.Focus(); break;
             }
             e.PreventDefault();
         });

        optionsButton.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up: optionsButton.Focus(); break;
                case NavigationMoveEvent.Direction.Down: quitButton.Focus(); break;
                case NavigationMoveEvent.Direction.Left: playButton.Focus(); break;
                case NavigationMoveEvent.Direction.Right: optionsButton.Focus(); break;
            }
            e.PreventDefault();
        });

        creditsButton.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up: playButton.Focus(); break;
                case NavigationMoveEvent.Direction.Down: creditsButton.Focus(); break;
                case NavigationMoveEvent.Direction.Left: creditsButton.Focus(); break;
                case NavigationMoveEvent.Direction.Right: quitButton.Focus(); break;
            }
            e.PreventDefault();
        });

        quitButton.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up: optionsButton.Focus(); break;
                case NavigationMoveEvent.Direction.Down: quitButton.Focus(); break;
                case NavigationMoveEvent.Direction.Left: creditsButton.Focus(); break;
                case NavigationMoveEvent.Direction.Right: quitButton.Focus(); break;
            }
            e.PreventDefault();
        });

        FocusFirstElement(playButton);
        ignoreInputTime = Time.time + .25f;

    }

    public void FocusFirstElement(VisualElement firstElement)
    {
        firstElement.Focus();
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
