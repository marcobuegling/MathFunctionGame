using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelSelectionManager : MonoBehaviour
{
    [SerializeField] private int totalLevels = 12;
    [SerializeField] private int columnsCount = 4;
    [SerializeField] private Vector2 buttonSize = new Vector2(120f, 120f);
    [SerializeField] private Vector2 buttonSpacing = new Vector2(20f, 20f);

    private readonly Color colBackground = new Color(0.10f, 0.10f, 0.14f);
    private readonly Color colPanel = new Color(0.14f, 0.14f, 0.20f);
    private readonly Color colButtonNormal = new Color(0.18f, 0.42f, 0.78f);
    private readonly Color colButtonHover = new Color(0.24f, 0.54f, 1.00f);
    private readonly Color colBack = new Color(0.22f, 0.22f, 0.28f);
    private readonly Color colBackHover = new Color(0.34f, 0.34f, 0.44f);
    private readonly Color colText = Color.white;

    public void Awake()
    {
        BuildUI();
    }

    void BuildUI()
    {
        // ── Canvas ────────────────────────────────────────────────────────────
        GameObject canvasGO = new GameObject("Canvas");
        Canvas canvas = canvasGO.AddComponent<Canvas>();
        canvas.renderMode = RenderMode.ScreenSpaceOverlay;
        canvas.sortingOrder = 0;

        CanvasScaler scaler = canvasGO.AddComponent<CanvasScaler>();
        scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
        scaler.referenceResolution = new Vector2(1920, 1080);
        scaler.matchWidthOrHeight = 0.5f;

        canvasGO.AddComponent<GraphicRaycaster>();

        // ── Background ────────────────────────────────────────────────────────
        GameObject bg = CreateImage(canvasGO, "Background", colBackground);
        StretchToParent(bg.GetComponent<RectTransform>());

        // ── Title ─────────────────────────────────────────────────────────────
        GameObject title = CreateText(canvasGO, "Title", "SELECT LEVEL", 52, FontStyle.Bold);
        RectTransform titleRT = title.GetComponent<RectTransform>();
        titleRT.anchorMin = new Vector2(0f, 1f);
        titleRT.anchorMax = new Vector2(1f, 1f);
        titleRT.pivot = new Vector2(0.5f, 1f);
        titleRT.anchoredPosition = new Vector2(0f, -40f);
        titleRT.sizeDelta = new Vector2(0f, 80f);

        // ── Scroll view (holds the grid) ──────────────────────────────────────
        GameObject scrollGO = BuildScrollView(canvasGO);

        // ── Grid ──────────────────────────────────────────────────────────────
        GameObject content = scrollGO.transform.Find("Viewport/Content").gameObject;
        PopulateGrid(content);

        // ── Back button ───────────────────────────────────────────────────────
        BuildBackButton(canvasGO);
    }

    // ── Scroll view ───────────────────────────────────────────────────────────

    GameObject BuildScrollView(GameObject parent)
    {
        GameObject scrollGO = new GameObject("ScrollView");
        scrollGO.transform.SetParent(parent.transform, false);

        RectTransform srRT = scrollGO.AddComponent<RectTransform>();
        ScrollRect sr = scrollGO.AddComponent<ScrollRect>();
        sr.horizontal = false;
        srRT.anchorMin = new Vector2(0f, 0.1f);
        srRT.anchorMax = new Vector2(1f, 0.88f);
        srRT.offsetMin = new Vector2(60f, 0f);
        srRT.offsetMax = new Vector2(-60f, 0f);

        // viewport
        GameObject viewport = new GameObject("Viewport");
        viewport.transform.SetParent(scrollGO.transform, false);
        RectTransform vpRT = viewport.AddComponent<RectTransform>();
        Image maskImg = viewport.AddComponent<Image>();
        maskImg.color = Color.clear;
        viewport.AddComponent<Mask>().showMaskGraphic = false;
        StretchToParent(vpRT);

        // content container
        GameObject content = new GameObject("Content");
        content.transform.SetParent(viewport.transform, false);
        RectTransform contentRT = content.AddComponent<RectTransform>();
        contentRT.anchorMin = new Vector2(0f, 1f);
        contentRT.anchorMax = new Vector2(1f, 1f);
        contentRT.pivot = new Vector2(0.5f, 1f);
        contentRT.anchoredPosition = Vector2.zero;

        GridLayoutGroup grid = content.AddComponent<GridLayoutGroup>();
        grid.cellSize = buttonSize;
        grid.spacing = buttonSpacing;
        grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
        grid.constraintCount = columnsCount;
        grid.childAlignment = TextAnchor.UpperCenter;
        grid.padding = new RectOffset(20, 20, 20, 20);

        ContentSizeFitter csf = content.AddComponent<ContentSizeFitter>();
        csf.verticalFit = ContentSizeFitter.FitMode.PreferredSize;

        sr.viewport = vpRT;
        sr.content = contentRT;

        return scrollGO;
    }

    // ── Level buttons ─────────────────────────────────────────────────────────

    void PopulateGrid(GameObject content)
    {
        for (int i = 1; i <= totalLevels; i++)
        {
            int levelIndex = i; // capture for lambda

            GameObject btn = CreateButton(
                content,
                "Level_" + levelIndex,
                levelIndex.ToString(),
                colButtonNormal,
                colButtonHover,
                36,
                () => ChooseLevel(levelIndex)
            );

            // square buttons look better in a grid
            RectTransform rt = btn.GetComponent<RectTransform>();
            rt.sizeDelta = buttonSize;
        }
    }

    // ── Back button ───────────────────────────────────────────────────────────

    void BuildBackButton(GameObject parent)
    {
        GameObject btn = CreateButton(
            parent,
            "BackButton",
            "← BACK",
            colBack,
            colBackHover,
            28,
            () => CloseLevelSelection()
        );

        RectTransform rt = btn.GetComponent<RectTransform>();
        rt.anchorMin = new Vector2(0f, 0f);
        rt.anchorMax = new Vector2(0f, 0f);
        rt.pivot = new Vector2(0f, 0f);
        rt.anchoredPosition = new Vector2(40f, 30f);
        rt.sizeDelta = new Vector2(180f, 60f);
    }

    // ── Primitive UI helpers ──────────────────────────────────────────────────

    GameObject CreateButton(
        GameObject parent,
        string name,
        string label,
        Color normalColor,
        Color hoverColor,
        int fontSize,
        UnityEngine.Events.UnityAction onClick)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent.transform, false);

        Image img = go.AddComponent<Image>();
        img.color = normalColor;

        Button btn = go.AddComponent<Button>();
        btn.onClick.AddListener(onClick);

        // colour transition
        ColorBlock cb = btn.colors;
        cb.normalColor = normalColor;
        cb.highlightedColor = hoverColor;
        cb.pressedColor = normalColor * 0.8f;
        cb.selectedColor = normalColor;
        cb.colorMultiplier = 1f;
        btn.colors = cb;
        btn.targetGraphic = img;

        // label
        GameObject textGO = CreateText(go, "Label", label, fontSize, FontStyle.Bold);
        StretchToParent(textGO.GetComponent<RectTransform>());

        return go;
    }

    GameObject CreateText(GameObject parent, string name, string content, int size, FontStyle style)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent.transform, false);

        Text t = go.AddComponent<Text>();
        t.text = content;
        t.font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");
        t.fontSize = size;
        t.fontStyle = style;
        t.color = colText;
        t.alignment = TextAnchor.MiddleCenter;

        go.AddComponent<RectTransform>();
        return go;
    }

    GameObject CreateImage(GameObject parent, string name, Color color)
    {
        GameObject go = new GameObject(name);
        go.transform.SetParent(parent.transform, false);
        go.AddComponent<RectTransform>();
        Image img = go.AddComponent<Image>();
        img.color = color;
        return go;
    }

    void StretchToParent(RectTransform rt)
    {
        rt.anchorMin = Vector2.zero;
        rt.anchorMax = Vector2.one;
        rt.offsetMin = Vector2.zero;
        rt.offsetMax = Vector2.zero;
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
