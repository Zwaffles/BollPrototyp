using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class OptionsMenu : MonoBehaviour
{
    private VisualElement root;

    private Button videoButton;
    private Button audioButton;
    private Button languageButton;
    private Button creditsButton;
    private Button returnButton;

    private float ignoreInputTime;
    private bool inputEnabled;

    private InputReader input;

    private void OnEnable()
    {
        inputEnabled = false;

        input = GameManager.instance.Input;
        input.AddSubmitEventListener(Submit);

        root = GetComponent<UIDocument>().rootVisualElement;

        videoButton = root.Q<Button>("VideoButton");
        audioButton = root.Q<Button>("AudioButton");
        languageButton = root.Q<Button>("LanguageButton");
        creditsButton = root.Q<Button>("CreditsButton");
        returnButton = root.Q<Button>("ReturnButton");

        FocusFirstElement(videoButton);
        ignoreInputTime = Time.time + .25f;
    }

    private void Update()
    {
        if (Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }
    }

    public void FocusFirstElement(VisualElement firstElement)
    {
        firstElement.Focus();
    }

    public void Submit()
    {
        if (!inputEnabled)
            return;

        if (!gameObject.activeInHierarchy)
            return;

        var focusedElement = GetFocusedElement();

        if (focusedElement == videoButton)
        {
            GameManager.instance.uiManager.ToggleVideoMenu(true);
            gameObject.SetActive(false);
        }

        if (focusedElement == audioButton)
        {
            GameManager.instance.uiManager.ToggleAudioMenu(true);
            gameObject.SetActive(false);
        }

        if (focusedElement == languageButton)
        {
            GameManager.instance.uiManager.ToggleLanguageMenu(true);
            gameObject.SetActive(false);
        }

        if (focusedElement == creditsButton)
        {
            GameManager.instance.uiManager.ToggleCreditMenu(true);
            gameObject.SetActive(false);
            //Debug.LogWarning("Credits not yet implemented :(");
        }

        if (focusedElement == returnButton)
        {
            GameManager.instance.uiManager.ToggleMainMenu(true);
            gameObject.SetActive(false);
        }
    }

    public Focusable GetFocusedElement()
    {
        return root.focusController.focusedElement;
    }

    public void Cancel(InputAction.CallbackContext context)
    {

        if (!gameObject.activeInHierarchy)
            return;

        var phase = context.phase;
        if (phase != InputActionPhase.Performed)
            return;

        GameManager.instance.uiManager.ToggleMainMenu(true);
        gameObject.SetActive(false);

    }

}
