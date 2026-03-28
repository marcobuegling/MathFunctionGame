using UnityEngine;
using UnityEngine.UI;

public class PlaySpeed : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] private Button button;
    [SerializeField] private Image image;
    [SerializeField] private Sprite[] sprites;
    private int currentSpeed;

    void Start()
    {
        button.onClick.AddListener(OnClick);
        image.sprite = sprites[0];
        currentSpeed = 1;
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnClick);
    }

    void OnClick()
    {
        currentSpeed += 1;
        if (currentSpeed > sprites.Length) { currentSpeed = 1; }
        gameManager.ChangeGameSpeed((float)currentSpeed);
        image.sprite = sprites[currentSpeed - 1];
    }
}
