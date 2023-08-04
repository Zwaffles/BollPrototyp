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

    private float ignoreInputTime;
    private bool inputEnabled;

    private InputReader input;

    [SerializeField, Header("Lock icon for locked sets")]
    private Texture2D lockIcon;

    /* ChatGPT told me to do this
    private ScrollView scrollView;
    private VisualElement selectedItem;
    private VisualElement contentContainer;
    private float itemHeight;
    private int selectedMenuItemIndex;
    private const float scrollSpeed = 200f;
    private float scrollOffset;

    */

    private void OnEnable()
    {
        inputEnabled = false;

        input = GameManager.instance.Input;
        input.AddSubmitEventListener(Submit);

        root = GetComponent<UIDocument>().rootVisualElement;

        /*
        
        Stuff ChatGPT told me to do
        
        scrollView = root.Q<ScrollView>("ScrollView");
        itemHeight = 30f; // Set this to the height of your menu items.
        selectedMenuItemIndex = 0;

        contentContainer = scrollView.contentContainer;
        scrollView.RegisterCallback<FocusEvent>(OnMenuItemFocused);
        scrollView.RegisterCallback<WheelEvent>(OnMouseWheel);

        // Enable keyboard focus so arrow keys work.
        scrollView.focusable = true;

        // Set up the scroll wheel event.
        scrollView.contentViewport.RegisterCallback<WheelEvent>(OnMouseWheel);

        */

        // Forest sets

        setF1Button = root.Q<VisualElement>("UI_SS_Set_Box_F1");
        if (!GameManager.instance.courseManager.GetUnlockStatusOfSet(0))
            setF1Button.Q<VisualElement>("UI_SS_Set_Image").style.backgroundImage = lockIcon;

        setF2Button = root.Q<VisualElement>("UI_SS_Set_Box_F2");
        if (!GameManager.instance.courseManager.GetUnlockStatusOfSet(1))
            setF2Button.Q<VisualElement>("UI_SS_Set_Image").style.backgroundImage = lockIcon;

        setF3Button = root.Q<VisualElement>("UI_SS_Set_Box_F3");
        if (!GameManager.instance.courseManager.GetUnlockStatusOfSet(2))
            setF3Button.Q<VisualElement>("UI_SS_Set_Image").style.backgroundImage = lockIcon;

        // Winter sets

        setW1Button = root.Q<VisualElement>("UI_SS_Set_Box_W1");
        if (!GameManager.instance.courseManager.GetUnlockStatusOfSet(3))
            setW1Button.Q<VisualElement>("UI_SS_Set2_Image").style.backgroundImage = lockIcon;

        setW2Button = root.Q<VisualElement>("UI_SS_Set_Box_W2");
        if (!GameManager.instance.courseManager.GetUnlockStatusOfSet(4))
            setW2Button.Q<VisualElement>("UI_SS_Set2_Image").style.backgroundImage = lockIcon;

        setW3Button = root.Q<VisualElement>("UI_SS_Set_Box_W3");
        if (!GameManager.instance.courseManager.GetUnlockStatusOfSet(5))
            setW3Button.Q<VisualElement>("UI_SS_Set2_Image").style.backgroundImage = lockIcon;

        // Beach sets

        setB1Button = root.Q<VisualElement>("UI_SS_Set_Box_B1");
        if (!GameManager.instance.courseManager.GetUnlockStatusOfSet(6))
            setB1Button.Q<VisualElement>("UI_SS_Set3Image").style.backgroundImage = lockIcon;

        setB2Button = root.Q<VisualElement>("UI_SS_Set_Box_B2");
        if (!GameManager.instance.courseManager.GetUnlockStatusOfSet(7))
            setB2Button.Q<VisualElement>("UI_SS_Set3Image").style.backgroundImage = lockIcon;

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

        //HandleArrowKeys();
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

    }

    /*
    
    Stuff ChatGPT told me to do

    private void HandleArrowKeys()
    {
        float verticalInput = Input.GetAxis("Vertical");
        if (Mathf.Abs(verticalInput) > 0.1f)
        {
            int direction = verticalInput > 0f ? -1 : 1;
            SelectNextMenuItem(direction);
            Debug.Log("This does get called");
        }
    }

    private void SelectNextMenuItem(int direction)
    {
        int nextIndex = selectedMenuItemIndex + direction;
        nextIndex = Mathf.Clamp(nextIndex, 0, contentContainer.childCount - 1);

        if (nextIndex != selectedMenuItemIndex)
        {
            selectedMenuItemIndex = nextIndex;
            selectedItem.RemoveFromClassList("focused");
            selectedItem = contentContainer.ElementAt(selectedMenuItemIndex);
            selectedItem.AddToClassList("focused");
            ScrollToSelected();
        }
    }

    private void ScrollToSelected()
    {
        float containerHeight = scrollView.contentViewport.layout.height;
        float itemTop = selectedMenuItemIndex * itemHeight;
        float itemBottom = itemTop + itemHeight;
        float viewTop = scrollOffset;
        float viewBottom = scrollOffset + containerHeight;

        if (itemTop < viewTop)
        {
            scrollOffset = itemTop;
        }
        else if (itemBottom > viewBottom)
        {
            scrollOffset = itemBottom - containerHeight;
        }

        UpdateScrollPosition();
    }

    private void OnMouseWheel(WheelEvent evt)
    {
        scrollOffset -= evt.delta.y * scrollSpeed;
        UpdateScrollPosition();
        Debug.Log("Halo sucks");
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
        selectedMenuItemIndex = contentContainer.IndexOf(selectedItem);
        ScrollToSelected();
    }

    */

    public Focusable GetFocusedElement()
    {
        return root.focusController.focusedElement;
    }

}
