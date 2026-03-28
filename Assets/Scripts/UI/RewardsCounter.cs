using TMPro;
using UnityEngine;

public class RewardsCounter : MonoBehaviour
{
    private TMP_Text tmpText;

    private int currentRewards = 0;
    private int maxRewards = 0;

    private void Awake()
    {
        tmpText = GetComponent<TMP_Text>();
        Debug.Log(tmpText);
    }

    private void UpdateText()
    {
        tmpText.text = $"{currentRewards} / {maxRewards}";
    }

    // Return true if all rewards have been collected
    public bool AddReward()
    {
        currentRewards++;
        UpdateText();
        return (currentRewards == maxRewards);
    }

    public void Reset()
    {
        currentRewards = 0;
        UpdateText();
    }

    public void UpdateMaxRewards(int maxRewards)
    {
        currentRewards = 0;
        this.maxRewards = maxRewards;
        UpdateText();
    }
}
