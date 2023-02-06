using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public CourseManager courseManager { get; private set; }

    private void Awake()
    {
        // Ensure that only one instance of the GameManager exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        // Find references to other managers in the scene
        courseManager = FindObjectOfType<CourseManager>();
    }
}
