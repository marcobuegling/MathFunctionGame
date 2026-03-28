using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class HoverResize : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float scaleFactor = 1.1f;

    private Vector3 normalScale;
    private Vector3 hoverScale;

    private Coroutine scaleRoutine;

    private void Start()
    {
        normalScale = Vector3.one;
        hoverScale = Vector3.one * scaleFactor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        AnimateScale(hoverScale);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        AnimateScale(normalScale);
    }

    private void AnimateScale(Vector3 target)
    {
        if (scaleRoutine != null) StopCoroutine(scaleRoutine);
        scaleRoutine = StartCoroutine(ScaleTo(target));
    }

    System.Collections.IEnumerator ScaleTo(Vector3 target)
    {
        Vector3 start = transform.localScale;
        float time = 0f;

        while (time < 0.15f)
        {
            time += Time.unscaledDeltaTime;
            transform.localScale = Vector3.Lerp(start, target, time / 0.15f);
            yield return null;
        }

        transform.localScale = target;
    }
}
