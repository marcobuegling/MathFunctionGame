using UnityEngine;

public class CoordinateSystem : MonoBehaviour
{
    [Header("Grid Settings")]
    [SerializeField] private int gridSize = 50;
    [SerializeField] private float gridSpacing = 1f;
    [SerializeField] private Color gridColor = new Color(0.5f, 0.5f, 0.5f, 0.3f);
    [SerializeField] private float gridLineWidth = 0.002f;

    [Header("Axis Settings")]
    [SerializeField] private Color axisColor = Color.black;
    [SerializeField] private float axisWidth = 0.05f;

    //[Header("Label Settings")]
    //[SerializeField] private int labelInterval = 5;
    //[SerializeField] private Font labelFont;
    //[SerializeField] private int fontSize = 12;
    //[SerializeField] private int characterSize = 100; // Higher = sharper text
    //[SerializeField] private Color labelColor = Color.black;

    [Header("Camera")]
    [SerializeField] private Camera mainCamera;

    void Start()
    {
        if (mainCamera == null)
            mainCamera = Camera.main;

        CreateGrid();
        CreateAxes();
        //CreateLabels();
    }

    void LateUpdate()
    {
        // Scale line widths based on camera zoom to keep them visible
        UpdateLineWidths();
    }

    void UpdateLineWidths()
    {
        if (mainCamera == null) return;

        float camSize = mainCamera.orthographicSize;

        // Update grid lines
        GameObject gridParent = transform.Find("Grid")?.gameObject;
        if (gridParent != null)
        {
            foreach (LineRenderer lr in gridParent.GetComponentsInChildren<LineRenderer>())
            {
                lr.startWidth = gridLineWidth * camSize;
                lr.endWidth = gridLineWidth * camSize;
            }
        }
    }

    void CreateGrid()
    {
        GameObject gridParent = new GameObject("Grid");
        gridParent.transform.parent = transform;

        // Vertical lines
        for (int x = -gridSize; x <= gridSize; x++)
        {
            if (x == 0) continue; // Skip center (axis)
            CreateLine(
                $"GridV_{x}", 
                new Vector3(x * gridSpacing, -gridSize * gridSpacing, 0),
                new Vector3(x * gridSpacing, gridSize * gridSpacing, 0), 
                gridColor,
                gridLineWidth, 
                gridParent.transform
            );
        }

        // Horizontal lines
        for (int y = -gridSize; y <= gridSize; y++)
        {
            if (y == 0) continue; // Skip center (axis)
            CreateLine(
                $"GridH_{y}", 
                new Vector3(-gridSize * gridSpacing, y * gridSpacing, 0),
                new Vector3(gridSize * gridSpacing, y * gridSpacing, 0), 
                gridColor,
                gridLineWidth, 
                gridParent.transform
            );
        }
    }

    void CreateAxes()
    {
        GameObject axesParent = new GameObject("Axes");
        axesParent.transform.parent = transform;

        // X-axis
        CreateLine(
            "X-Axis", 
            new Vector3(-gridSize * gridSpacing, 0, 0),
            new Vector3(gridSize * gridSpacing, 0, 0), 
            axisColor, 
            axisWidth, 
            axesParent.transform
        );

        // Y-axis
        CreateLine(
            "Y-Axis", 
            new Vector3(0, -gridSize * gridSpacing, 0),
            new Vector3(0, gridSize * gridSpacing, 0), 
            axisColor, 
            axisWidth, 
            axesParent.transform
        );
    }

    void CreateLine(string name, Vector3 start, Vector3 end, Color color, float width, Transform parent)
    {
        GameObject lineObj = new GameObject(name);
        lineObj.transform.parent = parent;
        LineRenderer lr = lineObj.AddComponent<LineRenderer>();

        lr.material = new Material(Shader.Find("Sprites/Default"));
        lr.startColor = color;
        lr.endColor = color;
        lr.startWidth = width;
        lr.endWidth = width;
        lr.positionCount = 2;
        lr.SetPosition(0, start);
        lr.SetPosition(1, end);
        lr.sortingOrder = -1;
    }

    //void CreateLabels()
    //{
    //    GameObject labelsParent = new GameObject("Labels");
    //    labelsParent.transform.parent = transform;

    //    // X-axis labels
    //    for (int x = -gridSize; x <= gridSize; x += labelInterval)
    //    {
    //        if (x == 0) continue;
    //        CreateTextLabel($"{x * gridSpacing}", new Vector3(x * gridSpacing, -0.5f, 0), labelsParent.transform);
    //    }

    //    // Y-axis labels
    //    for (int y = -gridSize; y <= gridSize; y += labelInterval)
    //    {
    //        if (y == 0) continue;
    //        CreateTextLabel($"{y * gridSpacing}", new Vector3(-0.5f, y * gridSpacing, 0), labelsParent.transform);
    //    }
    //}

    //    void CreateTextLabel(string text, Vector3 position, Transform parent)
    //    {
    //        GameObject textObj = new GameObject($"Label_{text}");
    //        textObj.transform.parent = parent;
    //        textObj.transform.position = position;

    //        TextMesh tm = textObj.AddComponent<TextMesh>();
    //        tm.text = text;
    //        tm.fontSize = fontSize;
    //        tm.characterSize = characterSize * 0.001f; // Convert to proper scale
    //        tm.color = labelColor;
    //        tm.anchor = TextAnchor.MiddleCenter;
    //        tm.alignment = TextAlignment.Center;

    //        if (labelFont != null)
    //            tm.font = labelFont;

    //        // Scale down the text object to make it appropriately sized
    //        textObj.transform.localScale = Vector3.one * 0.1f;

    //        // Make text face camera
    //        MeshRenderer mr = textObj.GetComponent<MeshRenderer>();
    //        mr.sortingOrder = 1;
    //    }
}