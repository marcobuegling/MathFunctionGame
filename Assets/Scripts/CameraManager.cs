using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameManager gameManager;
    [SerializeField] private ControlsManager controls;

    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 2.5f;
    [SerializeField] private float minZoom = 1f;
    [SerializeField] private float maxZoom = 100f;
    [SerializeField] private float zoomSmoothness = 0.1f;

    [Header("Pan Settings")]
    [SerializeField] private float panSpeed = 0.005f;
    [SerializeField] private float panSmoothness = 0.1f;

    private float minX;
    private float maxX;
    private float minY;
    private float maxY;
    private bool boundsSet = false;

    private Camera cam;
    private float camZ;

    private float targetSize;
    private Vector2 targetPosition;

    private bool movementEnabled;

    private void Awake()
    {
        cam = GetComponent<Camera>();
        targetSize = cam.orthographicSize;
        targetPosition = new Vector2(cam.transform.position.x, cam.transform.position.y);
        camZ = cam.transform.position.z;
    }

    public bool GetMovementEnabled()
    {
        return movementEnabled;
    }

    public void EnableMovement()
    {
        movementEnabled = true;
        targetSize = cam.orthographicSize;
        targetPosition = new Vector2(cam.transform.position.x, cam.transform.position.y);
    }

    public void DisableMovement()
    {
        movementEnabled = false;
    }

    private void Update()
    {
        if (!movementEnabled) return;
        // Read scroll wheel and update target zoom first, so position
        // clamping below can use the most up-to-date target size.
        float scroll = controls.CameraScroll;
        targetSize -= scroll * zoomSpeed;
        targetSize = Mathf.Clamp(targetSize, minZoom, maxZoom);

        // Read arrow keys and update target position
        Vector2 movement = controls.CameraMove;
        targetPosition += cam.orthographicSize * panSpeed * movement;
        ClampTargetPosition();

        // Smoothly interpolate to target zoom and position
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.unscaledDeltaTime / zoomSmoothness);
        Vector2 newPosition = new(
            Mathf.Lerp(cam.transform.position.x, targetPosition.x, Time.unscaledDeltaTime / panSmoothness),
            Mathf.Lerp(cam.transform.position.y, targetPosition.y, Time.unscaledDeltaTime / panSmoothness)
        );

        cam.transform.position = new Vector3(newPosition.x, newPosition.y, camZ);
    }

    // Clamps targetPosition so that the camera viewport never exceeds the
    // configured bounds. The visible half-extents scale with the target zoom.
    private void ClampTargetPosition()
    {
        if (!boundsSet) return;

        float halfH = targetSize;                   // vertical   half-extent
        float halfW = targetSize * cam.aspect;      // horizontal half-extent

        // If the bounds are smaller than the viewport in either axis, centre
        // the camera on that axis instead of trying to clamp.
        float clampMinX = (maxX - minX) > halfW * 2f ? minX + halfW : (minX + maxX) * 0.5f;
        float clampMaxX = (maxX - minX) > halfW * 2f ? maxX - halfW : (minX + maxX) * 0.5f;

        float clampMinY = (maxY - minY) > halfH * 2f ? minY + halfH : (minY + maxY) * 0.5f;
        float clampMaxY = (maxY - minY) > halfH * 2f ? maxY - halfH : (minY + maxY) * 0.5f;

        targetPosition.x = Mathf.Clamp(targetPosition.x, clampMinX, clampMaxX);
        targetPosition.y = Mathf.Clamp(targetPosition.y, clampMinY, clampMaxY);
    }

    public void UpdateCameraBounds(float minX, float maxX, float minY, float maxY)
    {
        this.minX = minX; this.maxX = maxX;
        this.minY = minY; this.maxY = maxY;
        boundsSet = true;
        targetPosition = new(0f, 0f);
        cam.transform.position = new(0f, 0f, camZ);
    }
}