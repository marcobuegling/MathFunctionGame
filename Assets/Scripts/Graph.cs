using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphWithCollider : MonoBehaviour
{
    public int resolution = 10;                 // Number of points per x (actual resolution might be slightly higher if range is not an integer)
    public float minX = 0;                      // Min x for which function is calculated
    public float maxX = 10;                     // Max x for which function is calculated
    public float yScale = 1f;                   // Scale of the graph in y direction
    private int numberOfPoints;

    public delegate float MathFunction(float x); // Delegate for mathematical functions

    // Example mathematical function: y = x^2
    public MathFunction function = x => x * x;

    private LineRenderer lineRenderer;
    private EdgeCollider2D edgeCollider;

    void Start()
    {
        numberOfPoints = (int)Math.Ceiling(resolution * (maxX - minX));

        lineRenderer = GetComponent<LineRenderer>();

        if (lineRenderer == null)
        {
            lineRenderer = gameObject.AddComponent<LineRenderer>();
        }

        lineRenderer.useWorldSpace = false;
        lineRenderer.positionCount = numberOfPoints;

        // Add EdgeCollider2D component for the graph's collider
        edgeCollider = gameObject.AddComponent<EdgeCollider2D>();

        // Create graph and collider
        CreateGraph();
    }

    void CreateGraph()
    {
        // Set LineRenderer settings
        lineRenderer.startWidth = 0.1f;
        lineRenderer.endWidth = 0.1f;
        lineRenderer.material = new Material(Shader.Find("Sprites/Default"));
        lineRenderer.startColor = Color.black;
        lineRenderer.endColor = Color.black;

        Vector2[] points = new Vector2[numberOfPoints];

        // Adjusting the collider to match the positions of the graph perfectly
        for (int i = 0; i < numberOfPoints; i++)
        {
            float x = Mathf.Lerp(minX, maxX, i / (float)(numberOfPoints - 1));
            float y = function(x) * yScale;

            Vector2 point = new Vector2(x, y);
            points[i] = point;
            lineRenderer.SetPosition(i, point);
        }

        // Update the EdgeCollider2D with the points in local space
        edgeCollider.points = points;
    }
}