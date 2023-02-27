using UnityEngine;
using UnityEngine.UIElements;

public class CourseSelect : MonoBehaviour
{
    [SerializeField, Header("Current Set (starts at 0)")]
    private int currentSet = 0;

    private CourseManager courseManager;

    private TextElement setName;

    private void OnEnable()
    {
        VisualElement root = GetComponent<UIDocument>().rootVisualElement;

        setName = root.Q<TextElement>("UI_LS_Header_Text");
    }

    private void Start()
    {
        courseManager = GameManager.instance.courseManager;

        setName.text = courseManager.GetSetName(currentSet);
    }
}
