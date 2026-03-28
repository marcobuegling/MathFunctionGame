using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverColorChange : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float highlight = 0.95f;
    [SerializeField] private Image image;

    void Start()
    {
        image.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        image.color = Color.white * highlight;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        image.color = Color.white;
    }
}
