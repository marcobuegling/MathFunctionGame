using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphWithCollider : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private int resolution = 10;                 // Number of points per x (actual resolution might be slightly higher if range is not an integer)
    [SerializeField] private float lineWidth = 0.5f;

    private Func<float, float> function;

    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;

    private void Awake()
    {
        lineRenderer = GetComponent<LineRenderer>();
        if (lineRenderer == null) lineRenderer = gameObject.AddComponent<LineRenderer>();
        lineRenderer.startWidth = lineWidth;
        lineRenderer.endWidth = lineWidth;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;
        lineRenderer.useWorldSpace = false;

        edgeCollider = GetComponent<EdgeCollider2D>();
        if (edgeCollider == null) edgeCollider = gameObject.AddComponent<EdgeCollider2D>();

        DeleteGraph();
    }

    public void DeleteGraph()
    {
        lineRenderer.positionCount = 0;
        edgeCollider.points = new Vector2[0];
        edgeCollider.enabled = false;
    }

    private void CreateGraph()
    {
        int numberOfPoints = (int)Math.Ceiling(resolution * (gameManager.GetXBoundHigh() - gameManager.GetXBoundLow()));
        edgeCollider.enabled = true;
        Vector2[] points = new Vector2[numberOfPoints];
        lineRenderer.positionCount = numberOfPoints;

        // Adjusting the collider to match the positions of the graph perfectly
        for (int i = 0; i < numberOfPoints; i++)
        {
            float x = Mathf.Lerp(gameManager.GetXBoundLow(), gameManager.GetXBoundHigh(), i / (float)(numberOfPoints - 1));
            float y = function(x);
            if (y < gameManager.GetYBoundLow()) y = gameManager.GetYBoundLow();
            if (y > gameManager.GetYBoundHigh()) y = gameManager.GetYBoundHigh();

            Vector2 point = new(x, y);
            points[i] = point;
            lineRenderer.SetPosition(i, point);
        }

        // Update the EdgeCollider2D with the points in local space
        edgeCollider.points = points;
    }

    public void UpdateFunction(Func<float, float> f)
    {
        function = f;
        CreateGraph();
    }
}