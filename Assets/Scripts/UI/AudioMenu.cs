using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class AudioMenu : MonoBehaviour
{
    private VisualElement root;

    private VisualElement masterContainer;
    private VisualElement musicContainer;
    private VisualElement sfxContainer;
    private VisualElement voiceContainer;

    private SliderInt masterSlider;
    private SliderInt musicSlider;
    private SliderInt sfxSlider;
    private SliderInt voiceSlider;

    private TextElement masterAmount;
    private TextElement musicAmount;
    private TextElement sfxAmount;
    private TextElement voiceAmount;

    private Button confirmButton;
    private Button defaultButton;

    private float ignoreInputTime;
    private bool inputEnabled;

    private const int defaultSliderValue = 50;
    private int masterValue;
    private int musicValue;
    private int sfxValue;
    private int voiceValue;

    private GameManager gameManager;
    private AudioManager audioManager;

    private int initialMasterValue;
    private int initialMusicValue;
    private int initialSFXValue;
    private int initialVoiceValue;

    private InputReader input;

    private void OnEnable()
    {
        inputEnabled = false;

        input = GameManager.instance.Input;
        input.AddSubmitEventListener(Submit);

        root = GetComponent<UIDocument>().rootVisualElement;

        masterContainer = root.Q<VisualElement>("MasterContainer");
        musicContainer = root.Q<VisualElement>("MusicContainer");
        sfxContainer = root.Q<VisualElement>("SFXContainer");
        voiceContainer = root.Q<VisualElement>("VoiceContainer");

        masterSlider = root.Q<SliderInt>("MasterSlider");
        musicSlider = root.Q<SliderInt>("MusicSlider");
        sfxSlider = root.Q<SliderInt>("SFXSlider");
        voiceSlider = root.Q<SliderInt>("VoiceSlider");

        masterAmount = root.Q<TextElement>("MasterAmountText");
        musicAmount = root.Q<TextElement>("MusicAmountText");
        sfxAmount = root.Q<TextElement>("SFXAmountText");
        voiceAmount = root.Q<TextElement>("VoiceAmountText");

        confirmButton = root.Q<Button>("Confirm");
        defaultButton = root.Q<Button>("Default");

        FocusFirstElement(masterContainer);
        ignoreInputTime = Time.time + .25f;

        gameManager = GameManager.instance;
        audioManager = gameManager.audioManager;

        InitializeAudioMenu();

        initialMasterValue = masterValue;
        initialMusicValue = musicValue;
        initialSFXValue = sfxValue;
        initialVoiceValue = voiceValue;

    }

    public void FocusFirstElement(VisualElement firstElement)
    {
        firstElement.Focus();
    }

    private void InitializeAudioMenu()
    {
        masterValue = (int)(audioManager.MasterVolume * 100);
        musicValue = (int)(audioManager.MusicVolume * 100);
        sfxValue = (int)(audioManager.SfxVolume * 100);
        voiceValue = (int)(audioManager.VoiceVolume * 100);

        masterSlider.value = masterValue;
        musicSlider.value = musicValue;
        sfxSlider.value = sfxValue;
        voiceSlider.value = voiceValue;

        masterAmount.text = masterValue.ToString();
        musicAmount.text = musicValue.ToString();
        sfxAmount.text = sfxValue.ToString();
        voiceAmount.text = voiceValue.ToString();
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
            //audioManager.ChangeAudioSettings(master: (float)masterValue / 100, music: (float)musicValue / 100, sfx: (float)sfxValue / 100, voice: (float)voiceValue / 100);
            SetVolumes();

            gameManager.uiManager.ToggleOptionsMenu(true);

            gameObject.SetActive(false);
        }

        if (focusedElement == defaultButton)
        {
            masterValue = defaultSliderValue;
            musicValue = defaultSliderValue;
            sfxValue = defaultSliderValue;
            voiceValue = defaultSliderValue;

            masterSlider.value = defaultSliderValue;
            musicSlider.value = defaultSliderValue;
            sfxSlider.value = defaultSliderValue;
            voiceSlider.value = defaultSliderValue;

            masterAmount.text = defaultSliderValue.ToString();
            musicAmount.text = defaultSliderValue.ToString();
            sfxAmount.text = defaultSliderValue.ToString();
            voiceAmount.text = defaultSliderValue.ToString();
        }
    }

    public Focusable GetFocusedElement()
    {
        return root.focusController.focusedElement;
    }

    private void SetVolumes()
    {
        audioManager.ChangeAudioSettings(master: (float)masterValue / 100, music: (float)musicValue / 100, sfx: (float)sfxValue / 100, voice: (float)voiceValue / 100);
    }

    public void Cancel(InputAction.CallbackContext context)
    {

        if (!gameObject.activeInHierarchy)
            return;

        var phase = context.phase;
        if (phase != InputActionPhase.Performed)
            return;

        masterValue = initialMasterValue;
        musicValue = initialMusicValue;
        sfxValue = initialSFXValue;
        voiceValue = initialVoiceValue;
        SetVolumes();

        GameManager.instance.uiManager.ToggleOptionsMenu(true);
        gameObject.SetActive(false);

    }

}