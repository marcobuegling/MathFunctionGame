using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static SceneLoader Instance { get; private set; }

    void Awake()
    {
        if (Instance != null) { Destroy(gameObject); return; }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void LoadMainMenu()
    {
        // Full reset: destroy persistent state if needed
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }

    public void LoadGame()
    {
        SceneManager.LoadScene("Game", LoadSceneMode.Single);
    }

    public void OpenLevelSelection()
    {
        ControlsManager.Instance.DisableControls();
        SceneManager.LoadSceneAsync("LevelSelection", LoadSceneMode.Additive);
    }

    public void CloseLevelSelection()
    {
        SceneManager.UnloadSceneAsync("LevelSelection").completed += _ =>
            ControlsManager.Instance.EnableControls();
    }

    public void OpenSettings()
    {
        ControlsManager.Instance.DisableControls();
        SceneManager.LoadSceneAsync("Settings", LoadSceneMode.Additive);
    }

    public void CloseSettings()
    {
        SceneManager.UnloadSceneAsync("Settings").completed += _ =>
            ControlsManager.Instance.EnableControls();
    }
}
