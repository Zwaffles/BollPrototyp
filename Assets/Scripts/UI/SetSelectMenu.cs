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
        //scrollView.RegisterCallback<FocusEvent>(OnMenuItemFocused);

        foreach (VisualElement ve in contentContainer.Children())
        {
            ve.RegisterCallback<FocusEvent>(OnMenuItemFocused);
        }

        // Enable keyboard focus so arrow keys work.
        //scrollView.focusable = true;

        // Set up the scroll wheel event.
        scrollView.contentViewport.RegisterCallback<WheelEvent>(OnMouseWheel);

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
        setB2Button.RegisterCallback<NavigationMoveEvent>(e =>
       {
           switch (e.direction)
           {
               case NavigationMoveEvent.Direction.Up: setB1Button.Focus(); break;
               case NavigationMoveEvent.Direction.Down: setB2Button.Focus(); break;
               case NavigationMoveEvent.Direction.Left: setB2Button.Focus(); break;
               case NavigationMoveEvent.Direction.Right: setB2Button.Focus(); break;
           }
           e.PreventDefault();
       });

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

    Is this the issue?

    private void HandleArrowKeys()
    {
        float verticalInput = Input.GetAxis("Vertical");
        if (Mathf.Abs(verticalInput) > 0.1f)
        {
            int direction = verticalInput > 0f ? -1 : 1;
            SelectNextMenuItem(direction);
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

    */

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

        /*

        if (itemMiddle < containerMiddle)
        {
            scrollOffset = itemTop - itemHeight;
        }
        else if (itemBottom > viewBottom - itemHeight)
        {
            scrollOffset = itemHeight + itemBottom - containerHeight;
        }

        */

        UpdateScrollPosition();
    }
    private void OnMouseWheel(WheelEvent evt)
    {
        //scrollOffset -= evt.delta.y * scrollSpeed;
        //UpdateScrollPosition();
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
