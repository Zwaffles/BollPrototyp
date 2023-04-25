using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSliderController : MonoBehaviour
{
    [SerializeField] private PlayerController playerController;

    [SerializeField] private Slider moveAccelerationSlider;
    [SerializeField] private Slider currentMaxSpeedSlider;
    [SerializeField] private Slider downSlopeSpeedMultiplierSlider;
    [SerializeField] private Slider maxDownSlopeSpeedSlider;
    [SerializeField] private Slider upSlopeSpeedMultiplierSlider;
    [SerializeField] private Slider minUpSlopeSpeedSlider;
    [SerializeField] private Slider fastSpeedRampUpFactorSlider;
    [SerializeField] private Slider slowSpeedRampUpFactorSlider;
    [SerializeField] private Slider maxOverspeedSlider;
    [SerializeField] private Slider targetMaxSpeedSlider;
    [SerializeField] private Slider standardGravitySlider;
    [SerializeField] private Slider gravityIncreaseFactorSlider;
    [SerializeField] private Slider maximumGravitySlider;
    [SerializeField] private Slider jumpHeightSlider;
    [SerializeField] private Slider boostSpeedFactorSlider;
    [SerializeField] private Slider boostDurationSlider;
    [SerializeField] private Slider boostGravityFactorSlider;
    [SerializeField] private Slider boostRocketFactorSlider;

    // Add more sliders as needed

    private float defaultMoveAcceleration;
    private float defaultCurrentMaxSpeed;
    private float defaultDownSlopeSpeedMultiplier;
    private float defaultMaxDownSlopeSpeed;
    private float defaultUpSlopeSpeedMultiplier;
    private float defaultMinUpSpeed;
    private float defaultFastSpeedRampUpFactor;
    private float defaultSlowSpeedRampUpFactor;
    private float defaultMaxOverspeed;
    private float defaultTargetMaxSpeed;
    private float defaultStandardGravity;
    private float defaultGravityIncreaseFactor;
    private float defaultMaximumGravity;
    private float defaultJumpHeight;
    private float defaultBoostSpeedFactor;
    private float defaultBoostDuration;
    private float defaultBoostGravityFactor;
    private float defaultBoostRocketFactor;

    private void OnEnable()
    {
        moveAccelerationSlider.onValueChanged.AddListener(OnMoveAccelerationSliderChanged);
        currentMaxSpeedSlider?.onValueChanged.AddListener(OnCurrentMaxSpeedSliderChanged);
        downSlopeSpeedMultiplierSlider.onValueChanged.AddListener(OnDownSlopeSpeedMultiplierSliderChanged);
        maxDownSlopeSpeedSlider.onValueChanged.AddListener(OnMaxDownSlopeSpeedSliderChanged);
        upSlopeSpeedMultiplierSlider.onValueChanged.AddListener(OnUpSlopeSpeedMultiplierSliderChanged);
        minUpSlopeSpeedSlider.onValueChanged.AddListener(OnMinUpSlopeSpeedSliderChanged);
        fastSpeedRampUpFactorSlider.onValueChanged.AddListener(OnFastSpeedRampUpFactorSliderChanged);
        slowSpeedRampUpFactorSlider.onValueChanged.AddListener(OnSlowSpeedRampUpFactorSliderChanged);
        maxOverspeedSlider.onValueChanged.AddListener(OnMaxOverspeedSliderChanged);
        targetMaxSpeedSlider.onValueChanged.AddListener(OnTargetMaxSpeedSliderChanged);
        standardGravitySlider.onValueChanged.AddListener(OnStandardGravitySliderChanged);
        gravityIncreaseFactorSlider.onValueChanged.AddListener(OnGravityIncreaseFactorSliderChanged);
        maximumGravitySlider.onValueChanged.AddListener(OnMaximumGravitySliderChanged);
        jumpHeightSlider.onValueChanged.AddListener(OnJumpHeightSliderChanged);
        boostSpeedFactorSlider.onValueChanged.AddListener(OnBoostSpeedFactorChanged);
        boostDurationSlider.onValueChanged.AddListener(OnBoostDurationChanged);
        boostGravityFactorSlider.onValueChanged.AddListener(OnBoostGravityFactorChanged);
        boostRocketFactorSlider.onValueChanged.AddListener(OnBoostRocketFactorChanged);
    }

    private void OnDisable()
    {
        moveAccelerationSlider.onValueChanged.RemoveListener(OnMoveAccelerationSliderChanged);
        currentMaxSpeedSlider?.onValueChanged.RemoveListener(OnCurrentMaxSpeedSliderChanged);
        downSlopeSpeedMultiplierSlider.onValueChanged.RemoveListener(OnDownSlopeSpeedMultiplierSliderChanged);
        maxDownSlopeSpeedSlider.onValueChanged.RemoveListener(OnMaxDownSlopeSpeedSliderChanged);
        upSlopeSpeedMultiplierSlider.onValueChanged.RemoveListener(OnUpSlopeSpeedMultiplierSliderChanged);
        minUpSlopeSpeedSlider.onValueChanged.RemoveListener(OnMinUpSlopeSpeedSliderChanged);
        fastSpeedRampUpFactorSlider.onValueChanged.RemoveListener(OnFastSpeedRampUpFactorSliderChanged);
        slowSpeedRampUpFactorSlider.onValueChanged.RemoveListener(OnSlowSpeedRampUpFactorSliderChanged);
        maxOverspeedSlider.onValueChanged.RemoveListener(OnMaxOverspeedSliderChanged);
        targetMaxSpeedSlider.onValueChanged.RemoveListener(OnTargetMaxSpeedSliderChanged);
        standardGravitySlider.onValueChanged.RemoveListener(OnStandardGravitySliderChanged);
        gravityIncreaseFactorSlider.onValueChanged.RemoveListener(OnGravityIncreaseFactorSliderChanged);
        maximumGravitySlider.onValueChanged.RemoveListener(OnMaximumGravitySliderChanged);
        jumpHeightSlider.onValueChanged.RemoveListener(OnJumpHeightSliderChanged);
        boostSpeedFactorSlider.onValueChanged.RemoveListener(OnBoostSpeedFactorChanged);
        boostDurationSlider.onValueChanged.RemoveListener(OnBoostDurationChanged);
        boostGravityFactorSlider.onValueChanged.RemoveListener(OnBoostGravityFactorChanged);
        boostRocketFactorSlider.onValueChanged.RemoveListener(OnBoostRocketFactorChanged);
    }

    private void Start()
    {
        // Store the default values
        defaultMoveAcceleration = playerController.MoveAcceleration;
        defaultCurrentMaxSpeed = playerController.CurrentMaxSpeed;
        defaultDownSlopeSpeedMultiplier = playerController.DownSlopeSpeedMultiplier;
        defaultMaxDownSlopeSpeed = playerController.MaxDownSlopeSpeed;
        defaultUpSlopeSpeedMultiplier = playerController.UpSlopeSpeedMultiplier;
        defaultMinUpSpeed = playerController.MinUpSlopeSpeed;
        defaultFastSpeedRampUpFactor = playerController.FastSpeedRampUpFactor;
        defaultSlowSpeedRampUpFactor = playerController.SlowSpeedRampUpFactor;
        defaultMaxOverspeed = playerController.MaxOverSpeed;
        defaultTargetMaxSpeed = playerController.TargetMaxSpeed;
        defaultStandardGravity = playerController.StandardGravity;
        defaultGravityIncreaseFactor = playerController.GravityIncreaseFactor;
        defaultMaximumGravity = playerController.MaximumGravity;
        defaultJumpHeight = playerController.JumpHeight;
        defaultBoostSpeedFactor = playerController.BoostSpeedFactor;
        defaultBoostDuration = playerController.BoostDuration;
        defaultBoostGravityFactor = playerController.BoostGravityFactor;
        defaultBoostRocketFactor = playerController.BoostRocketFactor;

        // Set the slider values to match the current PlayerController values
        moveAccelerationSlider.value = playerController.MoveAcceleration;
        //currentMaxSpeedSlider.value = playerController.CurrentMaxSpeed;
        downSlopeSpeedMultiplierSlider.value = playerController.DownSlopeSpeedMultiplier;
        maxDownSlopeSpeedSlider.value = playerController.MaxDownSlopeSpeed;
        upSlopeSpeedMultiplierSlider.value = playerController.UpSlopeSpeedMultiplier;
        minUpSlopeSpeedSlider.value = playerController.MinUpSlopeSpeed;
        fastSpeedRampUpFactorSlider.value = playerController.FastSpeedRampUpFactor;
        slowSpeedRampUpFactorSlider.value = playerController.SlowSpeedRampUpFactor;
        maxOverspeedSlider.value = playerController.MaxOverSpeed;
        targetMaxSpeedSlider.value = playerController.TargetMaxSpeed;
        standardGravitySlider.value = playerController.StandardGravity;
        gravityIncreaseFactorSlider.value = playerController.GravityIncreaseFactor;
        maximumGravitySlider.value = playerController.MaximumGravity;
        //jumpHeightSlider.value = playerController.JumpHeight;
        boostSpeedFactorSlider.value = playerController.BoostSpeedFactor;
        boostDurationSlider.value = playerController.BoostDuration;
        boostGravityFactorSlider.value = playerController.BoostGravityFactor;
        boostRocketFactorSlider.value = playerController.BoostRocketFactor;
    }

    public void OnMoveAccelerationSliderChanged(float value)
    {
        playerController.MoveAcceleration = value;
    }

    private void OnCurrentMaxSpeedSliderChanged(float value)
    {
        playerController.CurrentMaxSpeed = value;
    }

    public void OnDownSlopeSpeedMultiplierSliderChanged(float value)
    {
        playerController.DownSlopeSpeedMultiplier = value;
    }

    public void OnMaxDownSlopeSpeedSliderChanged(float value)
    {
        playerController.MaxDownSlopeSpeed = value;
    }

    public void OnUpSlopeSpeedMultiplierSliderChanged(float value)
    {
        playerController.UpSlopeSpeedMultiplier = value;
    }

    public void OnMinUpSlopeSpeedSliderChanged(float value)
    {
        playerController.MinUpSlopeSpeed = value;
    }

    public void OnFastSpeedRampUpFactorSliderChanged(float value)
    {
        playerController.FastSpeedRampUpFactor = value;
    }

    public void OnSlowSpeedRampUpFactorSliderChanged(float value)
    {
        playerController.SlowSpeedRampUpFactor = value;
    }

    public void OnMaxOverspeedSliderChanged(float value)
    {
        playerController.MaxOverSpeed = value;
    }

    public void OnTargetMaxSpeedSliderChanged(float value)
    {
        playerController.TargetMaxSpeed = value;
    }

    public void OnStandardGravitySliderChanged(float value)
    {
        playerController.StandardGravity = value;
    }

    public void OnGravityIncreaseFactorSliderChanged(float value)
    {
        playerController.GravityIncreaseFactor = value;
    }

    public void OnMaximumGravitySliderChanged(float value)
    {
        playerController.MaximumGravity = value;
    }

    public void OnJumpHeightSliderChanged(float value)
    {
        playerController.JumpHeight = value;
    }

    public void OnBoostSpeedFactorChanged(float value)
    {
        playerController.BoostSpeedFactor = value;
    }

    public void OnBoostDurationChanged(float value)
    {
        playerController.BoostDuration = value;
    }

    public void OnBoostGravityFactorChanged(float value)
    {
        playerController.BoostGravityFactor = value;
    }

    public void OnBoostRocketFactorChanged(float value)
    {
        playerController.BoostRocketFactor = value;
    }

    // Add more slider change methods as needed

    public void ResetToDefault()
    {
        playerController.MoveAcceleration = defaultMoveAcceleration;
        playerController.CurrentMaxSpeed = defaultCurrentMaxSpeed;
        playerController.DownSlopeSpeedMultiplier = defaultDownSlopeSpeedMultiplier;
        playerController.MaxDownSlopeSpeed = defaultMaxDownSlopeSpeed;
        playerController.UpSlopeSpeedMultiplier = defaultUpSlopeSpeedMultiplier;
        playerController.MinUpSlopeSpeed = defaultMinUpSpeed;
        playerController.FastSpeedRampUpFactor = defaultFastSpeedRampUpFactor;
        playerController.SlowSpeedRampUpFactor = defaultSlowSpeedRampUpFactor;
        playerController.MaxOverSpeed = defaultMaxOverspeed;
        playerController.TargetMaxSpeed = defaultTargetMaxSpeed;
        playerController.StandardGravity = defaultStandardGravity;
        playerController.GravityIncreaseFactor = defaultGravityIncreaseFactor;
        playerController.MaximumGravity = defaultMaximumGravity;
        playerController.JumpHeight = defaultJumpHeight;
        playerController.BoostSpeedFactor = defaultBoostSpeedFactor;
        playerController.BoostDuration = defaultBoostDuration;
        playerController.BoostGravityFactor = defaultBoostGravityFactor;
        playerController.BoostRocketFactor = defaultBoostRocketFactor;

        moveAccelerationSlider.value = defaultMoveAcceleration;
        //currentMaxSpeedSlider.value = defaultCurrentMaxSpeed;
        downSlopeSpeedMultiplierSlider.value = defaultDownSlopeSpeedMultiplier;
        maxDownSlopeSpeedSlider.value = defaultMaxDownSlopeSpeed;
        upSlopeSpeedMultiplierSlider.value = defaultUpSlopeSpeedMultiplier;
        minUpSlopeSpeedSlider.value = defaultMinUpSpeed;
        fastSpeedRampUpFactorSlider.value = defaultFastSpeedRampUpFactor;
        slowSpeedRampUpFactorSlider.value = defaultSlowSpeedRampUpFactor;
        maxOverspeedSlider.value = defaultMaxOverspeed;
        targetMaxSpeedSlider.value = defaultTargetMaxSpeed;
        standardGravitySlider.value = defaultStandardGravity;
        gravityIncreaseFactorSlider.value = defaultGravityIncreaseFactor;
        maximumGravitySlider.value = defaultMaximumGravity;
        //jumpHeightSlider.value = defaultJumpHeight;
        boostSpeedFactorSlider.value = defaultBoostSpeedFactor;
        boostDurationSlider.value = defaultBoostDuration;
        boostGravityFactorSlider.value = defaultBoostGravityFactor;
        boostRocketFactorSlider.value = defaultBoostRocketFactor;
    }
}
