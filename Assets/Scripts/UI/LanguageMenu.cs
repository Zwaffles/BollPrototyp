using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Localization.Settings;
using UnityEngine.UIElements;

public enum Language
{
    English,
    French,
    German,
    Hawaiian,
    Italian,
    Polish,
    Portuguese,
    Russian,
    Spanish,
    Turkish,
    Ukrainian,
    Chinese,
    Japanese,
    Icelandic
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
            case Language.French:
                menuLanguageText.text = "Français";
                break;
            case Language.German:
                menuLanguageText.text = "Deutsch";
                break;
            case Language.Hawaiian:
                menuLanguageText.text = "ʻŌlelo Hawaiʻi";
                break;
            case Language.Italian:
                menuLanguageText.text = "Italiano";
                break;
            case Language.Polish:
                menuLanguageText.text = "Polski";
                break;
            case Language.Portuguese:
                menuLanguageText.text = "Português Brasileiro";
                break;
            case Language.Russian:
                menuLanguageText.text = "Русский";
                break;
            case Language.Spanish:
                menuLanguageText.text = "Español";
                break;
            case Language.Turkish:
                menuLanguageText.text = "Türkçe";
                break;
            case Language.Ukrainian:
                menuLanguageText.text = "Українська";
                break;
            case Language.Chinese:
                menuLanguageText.text = "中文";
                break;
            case Language.Japanese:
                menuLanguageText.text = "日本語";
                break;
            case Language.Icelandic:
                menuLanguageText.text = "Íslenska";
                break;
        }

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
            case Language.French:
                return "fr";
            case Language.German:
                return "de";
            case Language.Hawaiian:
                return "haw";
            case Language.Italian:
                return "it";
            case Language.Polish:
                return "pl";
            case Language.Portuguese:
                return "pt-BR";
            case Language.Russian:
                return "ru";
            case Language.Spanish:
                return "es";
            case Language.Turkish:
                return "tr";
            case Language.Ukrainian:
                return "uk";
            case Language.Chinese:
                return "zh-Hans";
            case Language.Japanese:
                return "ja";
            case Language.Icelandic:
                return "is";
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
            case "fr":
                return Language.French;
            case "de":
                return Language.German;
            case "haw":
                return Language.Hawaiian;
            case "it":
                return Language.Italian;
            case "pl":
                return Language.Polish;
            case "pt-BR":
                return Language.Portuguese;
            case "ru":
                return Language.Russian;
            case "es":
                return Language.Spanish;
            case "tr":
                return Language.Turkish;
            case "uk":
                return Language.Ukrainian;
            case "zh-Hans":
                return Language.Chinese;
            case "ja":
                return Language.Japanese;
            case "is":
                return Language.Icelandic;
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
            case Language.French:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[1];
                return;
            case Language.German:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[2];
                return;
            case Language.Hawaiian:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[3];
                return;
            case Language.Italian:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[4];
                return;
            case Language.Polish:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[5];
                return;
            case Language.Portuguese:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[6];
                return;
            case Language.Russian:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[7];
                return;
            case Language.Spanish:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[8];
                return;
            case Language.Turkish:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[9];
                return;
            case Language.Ukrainian:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[10];
                return;
            case Language.Chinese:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[11];
                return;
            case Language.Japanese:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[12];
                return;
            case Language.Icelandic:
                LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[13];
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
