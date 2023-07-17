using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UIElements;

public class SetSelectMenu : MonoBehaviour
{

    private VisualElement root;

    private VisualElement setF1Button;
    private VisualElement setF2Button;

    private VisualElement setW1Button;
    private VisualElement setW2Button;

    private VisualElement setB1Button;

    private float ignoreInputTime;
    private bool inputEnabled;

    private InputReader input;

    [SerializeField, Header("Lock icon for locked sets")]
    private Texture2D lockIcon;

    private void OnEnable()
    {
        inputEnabled = false;

        input = GameManager.instance.Input;
        input.AddSubmitEventListener(Submit);

        root = GetComponent<UIDocument>().rootVisualElement;

        // Forest sets

        setF1Button = root.Q<VisualElement>("UI_SS_Set_Box_F1");
        if (!GameManager.instance.courseManager.GetUnlockStatusOfSet(0))
            setF1Button.Q<VisualElement>("UI_SS_Set_Image").style.backgroundImage = lockIcon;

        setF2Button = root.Q<VisualElement>("UI_SS_Set_Box_F2");
        if (!GameManager.instance.courseManager.GetUnlockStatusOfSet(1))
            setF2Button.Q<VisualElement>("UI_SS_Set_Image").style.backgroundImage = lockIcon;

        // Winter sets

        setW1Button = root.Q<VisualElement>("UI_SS_Set_Box_W1");
        if (!GameManager.instance.courseManager.GetUnlockStatusOfSet(2))
            setW1Button.Q<VisualElement>("UI_SS_Set2_Image").style.backgroundImage = lockIcon;

        setW2Button = root.Q<VisualElement>("UI_SS_Set_Box_W2");
        if (!GameManager.instance.courseManager.GetUnlockStatusOfSet(3))
            setW2Button.Q<VisualElement>("UI_SS_Set2_Image").style.backgroundImage = lockIcon;

        // Beach sets

        setB1Button = root.Q<VisualElement>("UI_SS_Set_Box_B1");
        if (!GameManager.instance.courseManager.GetUnlockStatusOfSet(4))
            setB1Button.Q<VisualElement>("UI_SS_Set3_Image").style.backgroundImage = lockIcon;

        //FocusFirstElement(playButton);
        ignoreInputTime = Time.time + .25f;

        FocusFirstElement(setF1Button);

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

        // Forest

        if (focusedElement == setF1Button && GameManager.instance.courseManager.GetUnlockStatusOfSet(0))
        {
            GameManager.instance.uiManager.ToggleLevelSelectMenu(true, 0);
            gameObject.SetActive(false);
        }

        if (focusedElement == setF2Button && GameManager.instance.courseManager.GetUnlockStatusOfSet(1))
        {
            GameManager.instance.uiManager.ToggleLevelSelectMenu(true, 1);
            gameObject.SetActive(false);
        }

        // Winter

        if (focusedElement == setW1Button && GameManager.instance.courseManager.GetUnlockStatusOfSet(2))
        {
            GameManager.instance.uiManager.ToggleLevelSelectMenu(true, 2);
            gameObject.SetActive(false);
        }

        if (focusedElement == setW2Button && GameManager.instance.courseManager.GetUnlockStatusOfSet(3))
        {
            GameManager.instance.uiManager.ToggleLevelSelectMenu(true, 3);
            gameObject.SetActive(false);
        }

        // Beach

        if (focusedElement == setB1Button && GameManager.instance.courseManager.GetUnlockStatusOfSet(4))
        {
            GameManager.instance.uiManager.ToggleLevelSelectMenu(true, 4);
            gameObject.SetActive(false);
        }

    }

    public Focusable GetFocusedElement()
    {
        return root.focusController.focusedElement;
    }

}
