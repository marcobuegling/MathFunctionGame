using UnityEngine;
using UnityEngine.InputSystem;
using System;

public class ControlsManager : MonoBehaviour
{

    private PlayerControls controls;

    public Vector2 CameraMove { get; private set; }
    public float CameraScroll { get; private set; }

    public event Action PausePressed;

    private void Awake()
    {
        controls = new PlayerControls();
    }

    private void OnEnable()
    {
        controls.Enable();

        controls.Game.Movement.performed += OnCameraMove;
        controls.Game.Movement.canceled += OnCameraMove;
        controls.Game.Scroll.performed += OnCameraScroll;
        controls.Game.Scroll.canceled += OnCameraScroll;
        controls.Game.Pause.performed += OnPause;
    }

    private void OnDisable()
    {
        controls.Game.Movement.performed -= OnCameraMove;
        controls.Game.Movement.canceled -= OnCameraMove;
        controls.Game.Scroll.performed -= OnCameraScroll;
        controls.Game.Scroll.canceled -= OnCameraScroll;
        controls.Game.Pause.performed -= OnPause;

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