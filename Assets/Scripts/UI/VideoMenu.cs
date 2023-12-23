using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public enum FramerateCap
{
    FPS30,
    FPS60,
    FPS90,
    FPS120,
    Unlimited,
}

public class VideoMenu : MonoBehaviour
{
    private VisualElement root;

    private VisualElement displayMode;
    private VisualElement resolution;
    private VisualElement framerateCap;
    private VisualElement vsync;

    private TextElement displayModeText;
    private TextElement resolutionText;
    private TextElement framerateCapText;
    private TextElement vsyncText;

    private Button confirmButton;
    private Button defaultButton;

    private float ignoreInputTime;
    private bool inputEnabled;

    private const FullScreenMode defaultScreenMode = FullScreenMode.FullScreenWindow;
    private FullScreenMode currentScreenMode = defaultScreenMode;

    private Resolution[] resolutions;
    private List<string> resolutionOptions = new List<string>();
    private int currentResolutionIndex = 0;

    private const FramerateCap defaultFramerateCap = FramerateCap.Unlimited;
    private FramerateCap currentFramerateCap = defaultFramerateCap;

    private const bool defaultVSyncEnabled = true;
    private bool vSyncEnabled = defaultVSyncEnabled;

    private InputReader input;

    private void OnEnable()
    {
        inputEnabled = false;

        input = GameManager.instance.Input;
        input.AddSubmitEventListener(Submit);

        root = GetComponent<UIDocument>().rootVisualElement;

        displayMode = root.Q<VisualElement>("DisplayModeBox");
        resolution = root.Q<VisualElement>("ResolutionBox");
        framerateCap = root.Q<VisualElement>("FramerateCapBox");
        vsync = root.Q<VisualElement>("VSyncBox");

        displayModeText = root.Q<TextElement>("DisplayModeText");
        resolutionText = root.Q<TextElement>("ResolutionText");
        framerateCapText = root.Q<TextElement>("FramerateCapText");
        vsyncText = root.Q<TextElement>("VSyncText");

        confirmButton = root.Q<Button>("Confirm");
        defaultButton = root.Q<Button>("Default");

        FocusFirstElement(displayMode);
        ignoreInputTime = Time.time + .25f;

        InitializeVideoMenu();
    }

    private void InitializeVideoMenu()
    {
        // Get supported resolutions and add them to the list
        resolutions = Screen.resolutions;

        for (int i = 0; i < resolutions.Length; i++)
        {
            Resolution resolution = resolutions[i];

            string resolutionOption = resolution.width + " x " + resolution.height;
            resolutionOptions.Add(resolutionOption);

            if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
                resolutionText.text = resolutionOptions[i];
            }
        }

        switch (currentScreenMode)
        {
            case FullScreenMode.ExclusiveFullScreen:
                displayModeText.style.fontSize = 48;
                displayModeText.text = GetLocalizedVariant("Fullscreen");
                break;
            case FullScreenMode.FullScreenWindow:
                displayModeText.style.fontSize = 48;
                displayModeText.text = GetLocalizedVariant("Borderless Windowed");
                break;
            case FullScreenMode.Windowed:
                displayModeText.text = GetLocalizedVariant("Windowed");
                break;
        }

        resolutionText.text = resolutionOptions[currentResolutionIndex];

        switch (currentFramerateCap)
        {
            case FramerateCap.FPS30:
                framerateCapText.text = "30 fps";
                break;
            case FramerateCap.FPS60:
                framerateCapText.text = "60 fps";
                break;
            case FramerateCap.FPS90:
                framerateCapText.text = "90 fps";
                break;
            case FramerateCap.FPS120:
                framerateCapText.text = "120 fps";
                break;
            case FramerateCap.Unlimited:
                framerateCapText.text = GetLocalizedVariant("Unlimited");
                break;
        }

        vsyncText.text = vSyncEnabled ? GetLocalizedVariant("Enabled") : GetLocalizedVariant("Disabled");

        displayMode.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up: displayMode.Focus(); break;
                case NavigationMoveEvent.Direction.Down: resolution.Focus(); break;
                case NavigationMoveEvent.Direction.Left:
                    switch (currentScreenMode)
                    {
                        case FullScreenMode.ExclusiveFullScreen:
                            displayModeText.text = GetLocalizedVariant("Windowed");
                            currentScreenMode = FullScreenMode.Windowed;
                            return;
                        case FullScreenMode.FullScreenWindow:
                            displayModeText.style.fontSize = 48;
                            displayModeText.text = GetLocalizedVariant("Fullscreen");
                            currentScreenMode = FullScreenMode.ExclusiveFullScreen;
                            return;
                        case FullScreenMode.Windowed:
                            displayModeText.style.fontSize = 48;
                            displayModeText.text = GetLocalizedVariant("Borderless Windowed");
                            currentScreenMode = FullScreenMode.FullScreenWindow;
                            return;
                    }
                    break;
                case NavigationMoveEvent.Direction.Right:
                    switch (currentScreenMode)
                    {
                        case FullScreenMode.ExclusiveFullScreen:
                            displayModeText.style.fontSize = 48;
                            displayModeText.text = GetLocalizedVariant("Borderless Windowed");
                            currentScreenMode = FullScreenMode.FullScreenWindow;
                            return;
                        case FullScreenMode.FullScreenWindow:
                            displayModeText.style.fontSize = 48;
                            displayModeText.text = GetLocalizedVariant("Windowed");
                            currentScreenMode = FullScreenMode.Windowed;
                            return;
                        case FullScreenMode.Windowed:
                            displayModeText.text = GetLocalizedVariant("Fullscreen");
                            currentScreenMode = FullScreenMode.ExclusiveFullScreen;
                            return;
                    }
                    break;
            }
            e.PreventDefault();
        });

        resolution.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up: displayMode.Focus(); break;
                case NavigationMoveEvent.Direction.Down: framerateCap.Focus(); break;
                case NavigationMoveEvent.Direction.Left:
                    currentResolutionIndex = (currentResolutionIndex - 1 + resolutions.Length) % resolutions.Length;
                    resolutionText.text = resolutionOptions[currentResolutionIndex]; break;
                case NavigationMoveEvent.Direction.Right:
                    currentResolutionIndex = (currentResolutionIndex + 1) % resolutions.Length;
                    resolutionText.text = resolutionOptions[currentResolutionIndex]; break;
            }
            e.PreventDefault();
        });

        framerateCap.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up: resolution.Focus(); break;
                case NavigationMoveEvent.Direction.Down: vsync.Focus(); break;
                case NavigationMoveEvent.Direction.Left:
                    switch (currentFramerateCap)
                    {
                        case FramerateCap.FPS30:
                            framerateCapText.text = GetLocalizedVariant("Unlimited");
                            currentFramerateCap = FramerateCap.Unlimited;
                            return;
                        case FramerateCap.FPS60:
                            framerateCapText.text = "30 fps";
                            currentFramerateCap = FramerateCap.FPS30;
                            return;
                        case FramerateCap.FPS90:
                            framerateCapText.text = "60 fps";
                            currentFramerateCap = FramerateCap.FPS60;
                            return;
                        case FramerateCap.FPS120:
                            framerateCapText.text = "90 fps";
                            currentFramerateCap = FramerateCap.FPS90;
                            return;
                        case FramerateCap.Unlimited:
                            framerateCapText.text = "120 fps";
                            currentFramerateCap = FramerateCap.FPS120;
                            return;
                    }
                    break;
                case NavigationMoveEvent.Direction.Right:
                    switch (currentFramerateCap)
                    {
                        case FramerateCap.FPS30:
                            framerateCapText.text = "60 fps";
                            currentFramerateCap = FramerateCap.FPS60;
                            return;
                        case FramerateCap.FPS60:
                            framerateCapText.text = "90 fps";
                            currentFramerateCap = FramerateCap.FPS90;
                            return;
                        case FramerateCap.FPS90:
                            framerateCapText.text = "120 fps";
                            currentFramerateCap = FramerateCap.FPS120;
                            return;
                        case FramerateCap.FPS120:
                            framerateCapText.text = GetLocalizedVariant("Unlimited");
                            currentFramerateCap = FramerateCap.Unlimited;
                            return;
                        case FramerateCap.Unlimited:
                            framerateCapText.text = "30 fps";
                            currentFramerateCap = FramerateCap.FPS30;
                            return;
                    }
                    break;
            }
            e.PreventDefault();
        });

        vsync.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up: framerateCap.Focus(); break;
                case NavigationMoveEvent.Direction.Down: confirmButton.Focus(); break;
                case NavigationMoveEvent.Direction.Left:
                    vSyncEnabled = !vSyncEnabled;
                    vsyncText.text = vSyncEnabled ? GetLocalizedVariant("Enabled") : GetLocalizedVariant("Disabled"); break;
                case NavigationMoveEvent.Direction.Right:
                    vSyncEnabled = !vSyncEnabled;
                    vsyncText.text = vSyncEnabled ? GetLocalizedVariant("Enabled") : GetLocalizedVariant("Disabled"); break;
            }
            e.PreventDefault();
        });

        confirmButton.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up: vsync.Focus(); break;
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
                case NavigationMoveEvent.Direction.Up: vsync.Focus(); break;
                case NavigationMoveEvent.Direction.Down: defaultButton.Focus(); break;
                case NavigationMoveEvent.Direction.Left: confirmButton.Focus(); break;
                case NavigationMoveEvent.Direction.Right: defaultButton.Focus(); break;
            }
            e.PreventDefault();
        });
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

        if (focusedElement == confirmButton)
        {
            Screen.SetResolution(resolutions[currentResolutionIndex].width, resolutions[currentResolutionIndex].height, currentScreenMode);

            switch (currentFramerateCap)
            {
                case FramerateCap.FPS30:
                    Application.targetFrameRate = 30;
                    break;
                case FramerateCap.FPS60:
                    Application.targetFrameRate = 60;
                    break;
                case FramerateCap.FPS90:
                    Application.targetFrameRate = 90;
                    break;
                case FramerateCap.FPS120:
                    Application.targetFrameRate = 120;
                    break;
                case FramerateCap.Unlimited:
                    Application.targetFrameRate = -1;
                    break;
            }

            QualitySettings.vSyncCount = vSyncEnabled ? 1 : 0;

            GameManager.instance.uiManager.ToggleOptionsMenu(true);
            gameObject.SetActive(false);
        }

        if (focusedElement == defaultButton)
        {
            currentScreenMode = defaultScreenMode;
            currentResolutionIndex = resolutions.Length - 1;
            currentFramerateCap = defaultFramerateCap;
            vSyncEnabled = defaultVSyncEnabled;

            displayModeText.style.fontSize = 48;
            displayModeText.text = GetLocalizedVariant("Borderless Windowed");
            resolutionText.text = resolutionOptions[resolutionOptions.Count - 1];
            framerateCapText.text = GetLocalizedVariant("Unlimited");
            vsyncText.text = GetLocalizedVariant("Enabled");
        }
    }

    public void Navigate(InputAction.CallbackContext context)
    {
        if (!gameObject.activeInHierarchy)
            return;

        var phase = context.phase;
        if (phase != InputActionPhase.Performed)
            return;

        try
        {
            // New
            GameManager.instance.audioManager.PlaySfx("Pop Sound 1");

        }
        catch
        {
            Debug.LogWarning("AudioManager not found. Perhaps you're not using the GameManager prefab?");
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

        GameManager.instance.uiManager.ToggleOptionsMenu(true);
        gameObject.SetActive(false);

    }

    #region Localization

    private string GetLocalizedVariant(string english)
    {

        string localizedString = "UNDEFINED";

        switch (GetCurrentLanguage())
        {

            case Language.English:
                if (english == "Unlimited") localizedString = "Unlimited";
                if (english == "Enabled") localizedString = "Enabled";
                if (english == "Disabled") localizedString = "Disabled";
                if (english == "Borderless Windowed") localizedString = "Borderless Windowed";
                if (english == "Windowed") localizedString = "Windowed";
                if (english == "Fullscreen") localizedString = "Fullscreen";
                break;
            case Language.Norwegian:
                if (english == "Unlimited") localizedString = "Ubegrenset";
                if (english == "Enabled") localizedString = "Aktivert";
                if (english == "Disabled") localizedString = "Frånslått";
                if (english == "Borderless Windowed") localizedString = "Ugrenset Vindue";
                if (english == "Windowed") localizedString = "Vinduet";
                if (english == "Fullscreen") localizedString = "Fullskjerm";
                break;
            default:
                if (english == "Unlimited") localizedString = "Unlimited";
                if (english == "Enabled") localizedString = "Enabled";
                if (english == "Disabled") localizedString = "Disabled";
                if (english == "Borderless Windowed") localizedString = "Borderless Windowed";
                if (english == "Windowed") localizedString = "Windowed";
                if (english == "Fullscreen") localizedString = "Fullscreen";
                break;
        }

        return localizedString;

    }

    private Language GetCurrentLanguage()
    {

        Language returnLanguage = Language.English;

        if (PlayerPrefs.GetString("selected-locale") != null)
        {
            switch (PlayerPrefs.GetString("selected-locale"))
            {
                case "en":
                    returnLanguage = Language.English;
                    break;
                case "no":
                    returnLanguage = Language.Norwegian;
                    break;
                default:
                    returnLanguage = Language.English;
                    break;
            }
        }

        return returnLanguage;

    }

#endregion

}
