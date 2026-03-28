using UnityEngine;
using UnityEngine.UI;

public class Reset : MonoBehaviour
{
    [SerializeField] GameManager gameManager;
    [SerializeField] private Button button;

    void Start()
    {
        button.onClick.AddListener(OnClick);
    }

    private void OnDestroy()
    {
        button.onClick.RemoveListener(OnClick);
    }

    void OnClick()
    {
        gameManager.ResetLevel();
    }
}
