using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;
using UnityEngine.UIElements;

public enum Language
{
    English,
    Norwegian
}

public class LanguageMenu : MonoBehaviour
{

    private VisualElement root;

    private VisualElement menuLanguage;

    private TextElement menuLanguageText;

    private Button confirmButton;
    private Button defaultButton;

    private float ignoreInputTime;
    private bool inputEnabled;

    private const Language defaultLanguage = Language.English;
    private Language currentLanguage = defaultLanguage;

    private Language initialLanguage;

    private InputReader input;

    private void OnEnable()
    {
        inputEnabled = false;

        input = GameManager.instance.Input;
        input.AddSubmitEventListener(Submit);

        root = GetComponent<UIDocument>().rootVisualElement;

        menuLanguage = root.Q<VisualElement>("DisplayModeBox");

        menuLanguageText = root.Q<TextElement>("DisplayModeText");

        confirmButton = root.Q<Button>("Confirm");
        defaultButton = root.Q<Button>("Default");

        FocusFirstElement(menuLanguage);
        ignoreInputTime = Time.time + .25f;

        InitializeVideoMenu();

        initialLanguage = currentLanguage;
    }

    private void InitializeVideoMenu()
    {

        currentLanguage = GetLanguageFromLocale(PlayerPrefs.GetString("selected-locale"));

        switch (currentLanguage)
        {
            case Language.English:
                menuLanguageText.text = "English";
                break;
            case Language.Norwegian:
                menuLanguageText.text = "Norsk";
                break;
        }

        menuLanguage.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up: menuLanguage.Focus(); break;
                case NavigationMoveEvent.Direction.Down: confirmButton.Focus(); break;

                case NavigationMoveEvent.Direction.Left:
                    switch (currentLanguage)
                    {
                        case Language.English:
                            menuLanguageText.text = "Norsk";
                            currentLanguage = Language.Norwegian;
                            return;
                        case Language.Norwegian:
                            menuLanguageText.text = "English";
                            currentLanguage = Language.English;
                            return;
                    }
                    PlayerPrefs.SetString("selected-locale", GetLocaleFromLanguage(currentLanguage));
                    SetLocaleFromLanguage(currentLanguage);
                    break;

                case NavigationMoveEvent.Direction.Right:
                    switch (currentLanguage)
                    {
                        case Language.English:
                            menuLanguageText.text = "Norsk";
                            currentLanguage = Language.Norwegian;
                            return;
                        case Language.Norwegian:
                            menuLanguageText.text = "English";
                            currentLanguage = Language.English;
                            return;
                    }
                    PlayerPrefs.SetString("selected-locale", GetLocaleFromLanguage(currentLanguage));
                    SetLocaleFromLanguage(currentLanguage);
                    break;
            }
            e.PreventDefault();
        });

        confirmButton.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up: menuLanguage.Focus(); break;
                case NavigationMoveEvent.Direction.Down: confirmButton.Focus(); break;
                case NavigationMoveEvent.Direction.Left: confirmButton.Focus(); break;
                case NavigationMoveEvent.Direction.Right: defaultButton.Focus(); break;
            }
            e.PreventDefault();
        });

        defaultButton.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up: menuLanguage.Focus(); break;
                case NavigationMoveEvent.Direction.Down: defaultButton.Focus(); break;
                case NavigationMoveEvent.Direction.Left: confirmButton.Focus(); break;
                case NavigationMoveEvent.Direction.Right: defaultButton.Focus(); break;
            }
            e.PreventDefault();
        });

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

        if (focusedElement == confirmButton)
        {

            PlayerPrefs.SetString("selected-locale", GetLocaleFromLanguage(currentLanguage));
            SetLocaleFromLanguage(currentLanguage);

            GameManager.instance.uiManager.ToggleOptionsMenu(true);
            gameObject.SetActive(false);
        }

        if (focusedElement == defaultButton)
        {
            currentLanguage = defaultLanguage;

            menuLanguageText.text = "English";

            PlayerPrefs.SetString("selected-locale", GetLocaleFromLanguage(currentLanguage));
            SetLocaleFromLanguage(currentLanguage);
        }
    }

    public Focusable GetFocusedElement()
    {
        return root.focusController.focusedElement;
    }

    private string GetLocaleFromLanguage(Language language)
    {
        switch (language)
        {
            case Language.English:
                return "en";
            case Language.Norwegian:
                return "no";
            default:
                return "en";
        }
    }

    private Language GetLanguageFromLocale(string locale)
    {
        switch (locale)
        {
            case "en":
                return Language.English;
            case "no":
                return Language.Norwegian;
            default:
                return Language.English;
        }
    }

    private void SetLocaleFromLanguage(Language language)
    {
        switch (language)
        {
            case Language.English:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[0];
                return;
            case Language.Norwegian:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
                return;
            default:

                return;
        }
    }

    public void Cancel(InputAction.CallbackContext context)
    {

        if (!gameObject.activeInHierarchy)
            return;

        var phase = context.phase;
        if (phase != InputActionPhase.Performed)
            return;

        currentLanguage = initialLanguage;
        PlayerPrefs.SetString("selected-locale", GetLocaleFromLanguage(currentLanguage));
        SetLocaleFromLanguage(currentLanguage);

        GameManager.instance.uiManager.ToggleOptionsMenu(true);
        gameObject.SetActive(false);

    }

}
