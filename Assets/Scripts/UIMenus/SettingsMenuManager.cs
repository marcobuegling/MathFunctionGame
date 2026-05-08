using UnityEngine;

public class SettingsMenuManager : MonoBehaviour
{
    public void CloseSettings()
    {
        SceneLoader.Instance.CloseSettings();
    }
}
