using UnityEngine;
using UnityEngine.UI;

public class PlayPause : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private Sprite runSprite;
    [SerializeField] private Sprite stopSprite;

    private void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        if (gameManager.IsRunning())
        {
            gameManager.Stop();
            image.sprite = runSprite;
        }
        else
        {
            gameManager.Run();
            image.sprite = stopSprite;
        }
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnClick);
    }

    public void Stop()
    {
        image.sprite = runSprite;
    }
}