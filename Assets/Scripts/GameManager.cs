using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    // Make GameManager singleton
    public static GameManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private ControlsManager controls;
    [SerializeField] private CameraManager mainCamera;
    [SerializeField] private GraphWithCollider graph;
    [SerializeField] private CoordinateSystem grid;
    [SerializeField] private PlayPause playPauseButton;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private RewardsCounter counter;

    [Header("Game objects")]
    [SerializeField] private GameObject pauseMenu;
    [SerializeField] private GameObject rider;
    [SerializeField] private GameObject rewardPrefab;

    [Header("Starting game state")]
    [SerializeField] private Level currentLevel;

    private GameObject rewards;
    private Rigidbody2D riderRB;

    private bool isRunning;
    private bool menuActive;

    // Current level boundaries
    private float minX;
    private float maxX;
    private float minY;
    private float maxY;

    private float speedFactor;

    private void Awake()
    {
        if (Instance != null && Instance != this) { Destroy(gameObject); return; }
        Instance = this;

        riderRB = rider.GetComponent<Rigidbody2D>();

        isRunning = false;
        menuActive = false;
        pauseMenu.SetActive(false);
        Time.timeScale = 0f;
        speedFactor = 1.0f;
    }

    private void Start()
    {
        LoadLevel(currentLevel);
    }

    private void Update()
    {
        UpdateGameSpeed();
    }

    private void OnEnable()
    {
        controls.PausePressed += TogglePauseMenu;
    }

    private void OnDisable()
    {
        controls.PausePressed -= TogglePauseMenu;
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public void Stop()
    {
        isRunning = false;
    }

    public void Run()
    {
        isRunning = true;
    }

    public void TogglePauseMenu()
    {
        if (menuActive)
        {
            menuActive = false;
            pauseMenu.SetActive(false);
        }
        else
        {
            pauseMenu.SetActive(true);
            menuActive = true;
        }
    }

    private void UpdateGameSpeed()
    {
        if (isRunning && !menuActive)
        {
            Time.timeScale = speedFactor;
        }
        else
        {
            Time.timeScale = 0f;
        }
    }

    public void LoadLevel(Level level)
    {
        minX = level.xBoundLower;
        maxX = level.xBoundUpper;
        minY = level.yBoundLower;
        maxY = level.yBoundUpper;

        grid.UpdateGrid(minX, maxX, minY, maxY);
        mainCamera.UpdateCameraBounds(minX, maxX, minY, maxY);

        counter.UpdateMaxRewards(level.rewards.Length);

        ResetLevel();
    }

    public void ResetLevel()
    {
        Stop();
        playPauseButton.Stop();

        graph.DeleteGraph();
        inputManager.DeleteText();

        counter.Reset();

        Destroy(rewards);
        rewards = new("Rewards");
        rewards.transform.parent = transform;

        foreach (Vector2 r in currentLevel.rewards)
        {
            GameObject reward = Instantiate(rewardPrefab);
            reward.transform.parent = rewards.transform;
            reward.transform.position = new(r.x, r.y, 0f);
        }

        rider.transform.position = new Vector3(currentLevel.riderPos.x, currentLevel.riderPos.y, 0f);
        riderRB.linearVelocity = Vector2.zero;
        riderRB.rotation = 0f;
        Debug.Log("Level reset");
    }

    public void AddReward()
    {
        if (counter.AddReward()) Debug.Log("Congratulations!");
    }

    public void ChangeGameSpeed(float speed)
    {
        speedFactor = speed;
        if (isRunning) Time.timeScale = speedFactor;
    }

    public float GetXBoundLow() { return minX; }
    public float GetXBoundHigh() { return maxX; }
    public float GetYBoundLow() { return minY; }
    public float GetYBoundHigh() {  return maxY; }

    public void QuitGame()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
