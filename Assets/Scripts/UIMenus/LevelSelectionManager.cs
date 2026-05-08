using UnityEngine;

public class LevelSelectionManager : MonoBehaviour
{
    public void Awake()
    {
        // Create grid of levels
    }

    // Reference this function from buttons if possible
    public void ChooseLevel(int levelId)
    {
        // Find level by id
        // GameManager.Instance.LoadLevel() // or maybe move this functionality here and make it singleton?
    }

    public void CloseLevelSelection()
    {
        SceneLoader.Instance.CloseLevelSelection();
    }
}
