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
    private VisualElement setF3Button;

    private VisualElement setW1Button;
    private VisualElement setW2Button;
    private VisualElement setW3Button;

    private VisualElement setB1Button;
    private VisualElement setB2Button;
    private VisualElement setB3Button;

    private float ignoreInputTime;
    private bool inputEnabled;

    private InputReader input;

    [SerializeField, Header("Lock icon for locked sets")]
    private Texture2D lockIcon;

    private ScrollView scrollView;
    private VisualElement selectedItem;
    private VisualElement contentContainer;
    private float itemHeight = 0f;
    private int selectedMenuItemIndex;
    private const float scrollSpeed = 200f;
    private float scrollOffset;

    private void OnEnable()
    {
        inputEnabled = false;

        input = GameManager.instance.Input;
        input.AddSubmitEventListener(Submit);

        root = GetComponent<UIDocument>().rootVisualElement;
        
        scrollView = root.Q<ScrollView>("ScrollView");
        selectedMenuItemIndex = 0;

        contentContainer = scrollView.contentContainer;

        foreach (VisualElement ve in contentContainer.Children())
        {
            ve.RegisterCallback<FocusEvent>(OnMenuItemFocused);
        }

        // Set up the scroll wheel event.
        scrollView.contentViewport.RegisterCallback<WheelEvent>(OnMouseWheel);

        SetupSetsUI();

        //FocusFirstElement(playButton);
        ignoreInputTime = Time.time + .25f;

        FocusFirstElement(setF1Button);

    }

    private void SetupSetsUI()
    {

        int stars;

        // Forest sets

        setF1Button = root.Q<VisualElement>("UI_SS_Set_Box_F1");
        if (!GameManager.instance.courseManager.GetUnlockStatusOfSet(0))
            setF1Button.Q<VisualElement>("UI_SS_Set_Image").style.backgroundImage = lockIcon;
        stars = GameManager.instance.courseManager.GetSetData()[0].stars;

        if (stars >= 3) setF1Button.Q<VisualElement>("UI_SS_Set1_CompletetionIcon3").style.unityBackgroundImageTintColor = Color.white;
        if (stars >= 2) setF1Button.Q<VisualElement>("UI_SS_Set1_CompletetionIcon2").style.unityBackgroundImageTintColor = Color.white;
        if (stars >= 1) setF1Button.Q<VisualElement>("UI_SS_Set1_CompletetionIcon1").style.unityBackgroundImageTintColor = Color.white;

        setF2Button = root.Q<VisualElement>("UI_SS_Set_Box_F2");
        if (!GameManager.instance.courseManager.GetUnlockStatusOfSet(1))
            setF2Button.Q<VisualElement>("UI_SS_Set_Image").style.backgroundImage = lockIcon;
        stars = GameManager.instance.courseManager.GetSetData()[1].stars;

        if (stars >= 3) setF2Button.Q<VisualElement>("UI_SS_Set1_CompletetionIcon3").style.unityBackgroundImageTintColor = Color.white;
        if (stars >= 2) setF2Button.Q<VisualElement>("UI_SS_Set1_CompletetionIcon2").style.unityBackgroundImageTintColor = Color.white;
        if (stars >= 1) setF2Button.Q<VisualElement>("UI_SS_Set1_CompletetionIcon1").style.unityBackgroundImageTintColor = Color.white;

        setF3Button = root.Q<VisualElement>("UI_SS_Set_Box_F3");
        if (!GameManager.instance.courseManager.GetUnlockStatusOfSet(2))
            setF3Button.Q<VisualElement>("UI_SS_Set_Image").style.backgroundImage = lockIcon;
        stars = GameManager.instance.courseManager.GetSetData()[2].stars;

        if (stars >= 3) setF3Button.Q<VisualElement>("UI_SS_Set1_CompletetionIcon3").style.unityBackgroundImageTintColor = Color.white;
        if (stars >= 2) setF3Button.Q<VisualElement>("UI_SS_Set1_CompletetionIcon2").style.unityBackgroundImageTintColor = Color.white;
        if (stars >= 1) setF3Button.Q<VisualElement>("UI_SS_Set1_CompletetionIcon1").style.unityBackgroundImageTintColor = Color.white;

        // Winter sets

        setW1Button = root.Q<VisualElement>("UI_SS_Set_Box_W1");
        if (!GameManager.instance.courseManager.GetUnlockStatusOfSet(3))
            setW1Button.Q<VisualElement>("UI_SS_Set2_Image").style.backgroundImage = lockIcon;
        stars = GameManager.instance.courseManager.GetSetData()[3].stars;

        if (stars >= 3) setW1Button.Q<VisualElement>("UI_SS_Set2_CompletetionIcon3").style.unityBackgroundImageTintColor = Color.white;
        if (stars >= 2) setW1Button.Q<VisualElement>("UI_SS_Set2_CompletetionIcon2").style.unityBackgroundImageTintColor = Color.white;
        if (stars >= 1) setW1Button.Q<VisualElement>("UI_SS_Set2_CompletetionIcon1").style.unityBackgroundImageTintColor = Color.white;

        setW2Button = root.Q<VisualElement>("UI_SS_Set_Box_W2");
        if (!GameManager.instance.courseManager.GetUnlockStatusOfSet(4))
            setW2Button.Q<VisualElement>("UI_SS_Set2_Image").style.backgroundImage = lockIcon;
        stars = GameManager.instance.courseManager.GetSetData()[4].stars;

        if (stars >= 3) setW2Button.Q<VisualElement>("UI_SS_Set2_CompletetionIcon3").style.unityBackgroundImageTintColor = Color.white;
        if (stars >= 2) setW2Button.Q<VisualElement>("UI_SS_Set2_CompletetionIcon2").style.unityBackgroundImageTintColor = Color.white;
        if (stars >= 1) setW2Button.Q<VisualElement>("UI_SS_Set2_CompletetionIcon1").style.unityBackgroundImageTintColor = Color.white;

        setW3Button = root.Q<VisualElement>("UI_SS_Set_Box_W3");
        if (!GameManager.instance.courseManager.GetUnlockStatusOfSet(5))
            setW3Button.Q<VisualElement>("UI_SS_Set2_Image").style.backgroundImage = lockIcon;
        stars = GameManager.instance.courseManager.GetSetData()[5].stars;

        if (stars >= 3) setW3Button.Q<VisualElement>("UI_SS_Set2_CompletetionIcon3").style.unityBackgroundImageTintColor = Color.white;
        if (stars >= 2) setW3Button.Q<VisualElement>("UI_SS_Set2_CompletetionIcon2").style.unityBackgroundImageTintColor = Color.white;
        if (stars >= 1) setW3Button.Q<VisualElement>("UI_SS_Set2_CompletetionIcon1").style.unityBackgroundImageTintColor = Color.white;

        // Beach sets

        setB1Button = root.Q<VisualElement>("UI_SS_Set_Box_B1");
        if (!GameManager.instance.courseManager.GetUnlockStatusOfSet(6))
            setB1Button.Q<VisualElement>("UI_SS_Set3Image").style.backgroundImage = lockIcon;
        stars = GameManager.instance.courseManager.GetSetData()[6].stars;

        if (stars >= 3) setB1Button.Q<VisualElement>("UI_SS_Set3_CompletetionIcon3").style.unityBackgroundImageTintColor = Color.white;
        if (stars >= 2) setB1Button.Q<VisualElement>("UI_SS_Set3_CompletetionIcon2").style.unityBackgroundImageTintColor = Color.white;
        if (stars >= 1) setB1Button.Q<VisualElement>("UI_SS_Set3_CompletetionIcon1").style.unityBackgroundImageTintColor = Color.white;

        setB2Button = root.Q<VisualElement>("UI_SS_Set_Box_B2");
        if (!GameManager.instance.courseManager.GetUnlockStatusOfSet(7))
            setB2Button.Q<VisualElement>("UI_SS_Set3Image").style.backgroundImage = lockIcon;
        stars = GameManager.instance.courseManager.GetSetData()[7].stars;

        if (stars >= 3) setB2Button.Q<VisualElement>("UI_SS_Set3_CompletetionIcon3").style.unityBackgroundImageTintColor = Color.white;
        if (stars >= 2) setB2Button.Q<VisualElement>("UI_SS_Set3_CompletetionIcon2").style.unityBackgroundImageTintColor = Color.white;
        if (stars >= 1) setB2Button.Q<VisualElement>("UI_SS_Set3_CompletetionIcon1").style.unityBackgroundImageTintColor = Color.white;

        setB3Button = root.Q<VisualElement>("UI_SS_Set_Box_B3");
        if (!GameManager.instance.courseManager.GetUnlockStatusOfSet(8))
            setB3Button.Q<VisualElement>("UI_SS_Set3Image").style.backgroundImage = lockIcon;
        stars = GameManager.instance.courseManager.GetSetData()[8].stars;

        if (stars >= 3) setB3Button.Q<VisualElement>("UI_SS_Set3_CompletetionIcon3").style.unityBackgroundImageTintColor = Color.white;
        if (stars >= 2) setB3Button.Q<VisualElement>("UI_SS_Set3_CompletetionIcon2").style.unityBackgroundImageTintColor = Color.white;
        if (stars >= 1) setB3Button.Q<VisualElement>("UI_SS_Set3_CompletetionIcon1").style.unityBackgroundImageTintColor = Color.white;

        // Special navigation cases for top and bottom
        setF1Button.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up: setF1Button.Focus(); break;
                case NavigationMoveEvent.Direction.Down: setF2Button.Focus(); break;
                case NavigationMoveEvent.Direction.Left: setF1Button.Focus(); break;
                case NavigationMoveEvent.Direction.Right: setF1Button.Focus(); break;
            }
            e.PreventDefault();
        });
        setB3Button.RegisterCallback<NavigationMoveEvent>(e =>
        {
            switch (e.direction)
            {
                case NavigationMoveEvent.Direction.Up: setB2Button.Focus(); break;
                case NavigationMoveEvent.Direction.Down: setB3Button.Focus(); break;
                case NavigationMoveEvent.Direction.Left: setB3Button.Focus(); break;
                case NavigationMoveEvent.Direction.Right: setB3Button.Focus(); break;
            }
            e.PreventDefault();
        });

    }

    public void FocusFirstElement(VisualElement firstElement)
    {
        firstElement.Focus();

        SetData focusedSet = GameManager.instance.courseManager.GetSetData()[0]; // Here I assume it's always the first set - Forest I
        GameManager.instance.uiManager.SetTargetColors(focusedSet.primarySetColor, focusedSet.secondarySetColor);
    }

    private void Update()
    {

        if (Time.time > ignoreInputTime)
        {
            inputEnabled = true;
        }

        // Allt nedanför hade nog kunnat göras event-drivet.

        var focusedElement = GetFocusedElement();

        SetData focusedSet = GameManager.instance.courseManager.GetSetData()[0];

        // Forest

        if (focusedElement == setF1Button) focusedSet = GameManager.instance.courseManager.GetSetData()[0];

        if (focusedElement == setF2Button) focusedSet = GameManager.instance.courseManager.GetSetData()[1];

        if (focusedElement == setF3Button) focusedSet = GameManager.instance.courseManager.GetSetData()[2];

        // Winter

        if (focusedElement == setW1Button) focusedSet = GameManager.instance.courseManager.GetSetData()[3];

        if (focusedElement == setW2Button) focusedSet = GameManager.instance.courseManager.GetSetData()[4];

        if (focusedElement == setW3Button) focusedSet = GameManager.instance.courseManager.GetSetData()[5];

        // Beach

        if (focusedElement == setB1Button) focusedSet = GameManager.instance.courseManager.GetSetData()[6];

        if (focusedElement == setB2Button) focusedSet = GameManager.instance.courseManager.GetSetData()[7];

        if (focusedElement == setB3Button) focusedSet = GameManager.instance.courseManager.GetSetData()[8];

        GameManager.instance.uiManager.SetTargetColors(focusedSet.primarySetColor, focusedSet.secondarySetColor);

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

        if (focusedElement == setF3Button && GameManager.instance.courseManager.GetUnlockStatusOfSet(2))
        {
            GameManager.instance.uiManager.ToggleLevelSelectMenu(true, 2);
            gameObject.SetActive(false);
        }

        // Winter

        if (focusedElement == setW1Button && GameManager.instance.courseManager.GetUnlockStatusOfSet(3))
        {
            GameManager.instance.uiManager.ToggleLevelSelectMenu(true, 3);
            gameObject.SetActive(false);
        }

        if (focusedElement == setW2Button && GameManager.instance.courseManager.GetUnlockStatusOfSet(4))
        {
            GameManager.instance.uiManager.ToggleLevelSelectMenu(true, 4);
            gameObject.SetActive(false);
        }

        if (focusedElement == setW3Button && GameManager.instance.courseManager.GetUnlockStatusOfSet(5))
        {
            GameManager.instance.uiManager.ToggleLevelSelectMenu(true, 5);
            gameObject.SetActive(false);
        }

        // Beach

        if (focusedElement == setB1Button && GameManager.instance.courseManager.GetUnlockStatusOfSet(6))
        {
            GameManager.instance.uiManager.ToggleLevelSelectMenu(true, 6);
            gameObject.SetActive(false);
        }

        if (focusedElement == setB2Button && GameManager.instance.courseManager.GetUnlockStatusOfSet(7))
        {
            GameManager.instance.uiManager.ToggleLevelSelectMenu(true, 7);
            gameObject.SetActive(false);
        }

        if (focusedElement == setB3Button && GameManager.instance.courseManager.GetUnlockStatusOfSet(8))
        {
            GameManager.instance.uiManager.ToggleLevelSelectMenu(true, 8);
            gameObject.SetActive(false);
        }

    }

    private void ScrollToSelected()
    {

        float containerHeight = scrollView.contentViewport.layout.height;
        float itemTop = selectedMenuItemIndex * itemHeight;
        float itemBottom = itemTop + itemHeight;
        float viewTop = scrollOffset;
        float viewBottom = scrollOffset + containerHeight;
        float itemMiddle = itemTop + itemHeight / 2;
        float containerMiddle = scrollView.contentViewport.layout.height / 2;

        scrollOffset = itemMiddle - containerMiddle;

        UpdateScrollPosition();
    }
    private void OnMouseWheel(WheelEvent evt)
    {
        evt.StopPropagation();
    }

    private void UpdateScrollPosition()
    {
        float maxOffset = Mathf.Max(0f, contentContainer.layout.height - scrollView.contentViewport.layout.height);
        scrollOffset = Mathf.Clamp(scrollOffset, 0f, maxOffset);
        contentContainer.style.top = new StyleLength(-scrollOffset);
    }

    private void OnMenuItemFocused(FocusEvent evt)
    {
        selectedItem = evt.target as VisualElement;
        itemHeight = Mathf.Max(itemHeight, selectedItem.contentRect.height);
        selectedMenuItemIndex = contentContainer.IndexOf(selectedItem);
        ScrollToSelected();
    }

    public Focusable GetFocusedElement()
    {
        return root.focusController.focusedElement;
    }

}
