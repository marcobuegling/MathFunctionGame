using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{

    public void OpenLevelSelection()
    {
        SceneLoader.Instance.OpenLevelSelection();
    }

    public void OpenSettings()
    {
        SceneLoader.Instance.OpenSettings();
    }

    public void OpenMainMenu()
    {
        SceneLoader.Instance.LoadMainMenu();
    }

    public void QuitGame()
    {
        GameManager.Instance.QuitGame();
    }


}
