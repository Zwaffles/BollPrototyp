using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private InputReader input;

    public InputReader Input
    {
        get
        {
            return input;
        }
    }

    public enum GameState
    {
        Play,
        Pause,
        Menu,
    }

    public static GameManager instance;

    public CourseManager courseManager { get; private set; }
    public DataManager dataManager { get; private set; }
    public UIManager uiManager { get; private set; }

    [SerializeField] private GameState currentGameState = GameState.Menu;

    public GameState CurrentGameState
    {
        get 
        { 
            return currentGameState; 
        }

        set
        {
            currentGameState = value;
        }
    }

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
        dataManager = FindObjectOfType<DataManager>();
        uiManager = FindObjectOfType<UIManager>();
    }

    public void SetGameState(GameState state)
    {
        currentGameState = state;
    }
}
