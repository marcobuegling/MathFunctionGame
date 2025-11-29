using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CameraManager : MonoBehaviour
{
    [Header("Zoom Settings")]
    [SerializeField] private float zoomSpeed = 10f;
    [SerializeField] private float minZoom = 20f;
    [SerializeField] private float maxZoom = 100f;
    [SerializeField] private float zoomSmoothness = 0.1f;

    [Header("Pan Settings")]
    [SerializeField] private float panSpeed = 1f;
    [SerializeField] private float panSmoothness = 0.1f;
    [SerializeField] private float edgeTolerance = 0.99f;

    private Camera cam;
    private float camZ;

    private float targetSize;
    private Vector2 targetPosition;

    private void Start()
    {
        cam = GetComponent<Camera>();
        targetSize = cam.orthographicSize;
        targetPosition = new Vector2(cam.transform.position.x, cam.transform.position.y);
        camZ = cam.transform.position.z;
        Screen.fullScreen = true;
    }

    private void Update()
    {
        float scrollInput = Input.mouseScrollDelta.y;
        Vector3 mousePosition = Input.mousePosition;

        Debug.Log($"{mousePosition}, {scrollInput}");
        Debug.Log($"Screen Width: {Screen.width}, Screen Height: {Screen.height}");
        Debug.Log($"Main Display Resolution: {Display.main.systemWidth}x{Display.main.systemHeight}");
        Debug.Log(Screen.fullScreen);
    }


    //    Attempt to use new input system - failed because there was an offset in the mouse data that I could not fix - not finished, different tries ended in chaos that hasn't been cleaned, thus code in update is contradictory

    //    private void Awake()
    //    {
    //        playerControls = new PlayerControls();
    //        cam = GetComponent<Camera>();
    //        targetSize = cam.orthographicSize;
    //        targetPosition = new Vector2(cam.transform.position.x, cam.transform.position.y);
    //        camZ = cam.transform.position.z;


    //    }

    //    private void OnEnable()
    //    {
    //        playerControls.Enable();
    //        playerControls.Gameplay.Zoom.performed += OnZoom;
    //    }

    //    private void OnDisable()
    //    {
    //        playerControls.Gameplay.Zoom.performed -= OnZoom;
    //        playerControls.Disable();
    //    }

    //    private void OnZoom(InputAction.CallbackContext context)
    //    {
    //        Vector2 scrollValue = context.ReadValue<Vector2>();

    //        targetSize = targetSize - scrollValue.y * zoomSpeed;
    //        targetSize = Mathf.Clamp(targetSize, minZoom, maxZoom);
    //    }

    //    private void Update()
    //    {
    //        Vector2 mousePositionScreen = Mouse.current.position.ReadValue();
    //        Vector3 mousePositionWorldVec3 = cam.ScreenToWorldPoint(mousePositionScreen);
    //        Vector2 mousePositionWorld = new(mousePositionWorldVec3.x, mousePositionWorldVec3.y);
    //        Vector2 mousePosition = new((mousePositionWorld.x - cam.transform.position.x) * 23.95f + Screen.width, (mousePositionWorld.y - cam.transform.position.y) * 23.95f + Screen.height);
    //        Debug.Log($"Mouse: {mousePosition}, Screen: {Screen.width}x{Screen.height}");
    //        Vector2 delta = Vector2.zero;

    //        // BUG: mousePosition doesn't align with actual position on screen: could use old input system to fix that, but meh
    //        if (mousePosition.x >= -496f + Screen.width * edgeTolerance) delta.x += 1f;
    //        else if (mousePosition.x <= 496f + Screen.width * (1f - edgeTolerance)) delta.x -= 1f;
    //        if (mousePosition.y >= -290f + Screen.height * edgeTolerance) delta.y += 1f;
    //        else if (mousePosition.y <= 290f + Screen.height * (1f - edgeTolerance)) delta.y -= 1f;

    //        targetPosition += delta * panSpeed * cam.orthographicSize;

    //        // Smoothly interpolate to target zoom and position
    //        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetSize, Time.deltaTime / zoomSmoothness);
    //        Vector2 newPosition = new(
    //            Mathf.Lerp(cam.transform.position.x, targetPosition.x, Time.deltaTime / panSmoothness),
    //            Mathf.Lerp(cam.transform.position.y, targetPosition.y, Time.deltaTime / panSmoothness)
    //        );

    //        cam.transform.position = new Vector3(newPosition.x, newPosition.y, camZ);
    //    }
}