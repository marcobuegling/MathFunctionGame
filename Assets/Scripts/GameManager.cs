using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Make GameManager singleton
    public static GameManager Instance { get; private set; }

    [SerializeField] private CameraManager mainCamera;
    [SerializeField] private GraphWithCollider graph;
    [SerializeField] private CoordinateSystem grid;
    [SerializeField] private PlayPause playPauseButton;
    [SerializeField] private InputManager inputManager;
    [SerializeField] private RewardsCounter counter;
    [SerializeField] private GameObject rider;
    [SerializeField] private GameObject rewardPrefab;

    [SerializeField] private Level currentLevel;

    private Rigidbody2D riderRB;

    private bool isRunning;
    //private bool menuActive;

    private GameObject rewards;

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
        Time.timeScale = 0f;
        speedFactor = 1.0f;
        //menuActive = false;
    }

    private void Start()
    {
        
        LoadLevel(currentLevel);
    }

    public bool IsRunning()
    {
        return isRunning;
    }

    public void Stop()
    {
        isRunning = false;
        Time.timeScale = 0f;
    }

    public void Run()
    {
        isRunning = true;
        Time.timeScale = speedFactor;
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
}
