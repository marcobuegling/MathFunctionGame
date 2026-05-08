using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class ControlsManager : MonoBehaviour
{
    public static ControlsManager Instance { get; private set; } // make ControlsManager singleton

    private PlayerControls controls;

    public Vector2 CameraMove { get; private set; }
    public float CameraScroll { get; private set; }

    public event Action PausePressed;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        DontDestroyOnLoad(gameObject);

        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Game.Movement.performed += OnCameraMove;
        controls.Game.Movement.canceled += OnCameraMove;
        controls.Game.Scroll.performed += OnCameraScroll;
        controls.Game.Scroll.canceled += OnCameraScroll;
        controls.Game.Pause.performed += OnPause;

        EnableControls();
    }

    public void EnableControls()
    {
        controls.Enable();
    }

    private void OnDisable()
    {
        controls.Game.Movement.performed -= OnCameraMove;
        controls.Game.Movement.canceled -= OnCameraMove;
        controls.Game.Scroll.performed -= OnCameraScroll;
        controls.Game.Scroll.canceled -= OnCameraScroll;
        controls.Game.Pause.performed -= OnPause;

        DisableControls();
    }

    public void DisableControls()
    {
        controls.Disable();
    }

    public void OnCameraMove(InputAction.CallbackContext context)
    {
        CameraMove = context.ReadValue<Vector2>();
    }

    public void OnCameraScroll(InputAction.CallbackContext context)
    {
        CameraScroll = context.ReadValue<float>();
    }

    public void OnPause(InputAction.CallbackContext context)
    {
        if (context.performed)
            PausePressed?.Invoke();
    }
}