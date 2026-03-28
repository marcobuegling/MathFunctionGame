using NCalc;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;
    [SerializeField] private CameraManager mainCamera;
    [SerializeField] private GraphWithCollider graph;
    private TMP_InputField inputField;
    private TMP_Text textComponent;

    private void Awake()
    {
        inputField = GetComponent<TMP_InputField>();
        textComponent = inputField.textComponent;
    }

    void Start()
    {
        inputField.onValueChanged.AddListener(OnValueChanged);
        inputField.onEndEdit.AddListener(OnSubmit);
    }

    private void Update()
    {
        if 
        (
            EventSystem.current.currentSelectedGameObject != null
            && EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>() != null
            && EventSystem.current.currentSelectedGameObject.GetComponent<TMP_InputField>().isFocused
        )
            mainCamera.DisableMovement();
        else if (!mainCamera.GetMovementEnabled())
            mainCamera.EnableMovement();
    }

    private Func<float, float> MakeFunction(string expression)
    {
        return (x) =>
        {
            var expr = new Expression(expression);
            expr.Parameters["x"] = (double)x;
            return (float)Convert.ToDouble(expr.Evaluate());
        };
    }

    private void OnValueChanged(string currentText)
    {
        SetTextColor(Color.black);
    }

    private void OnSubmit(string submittedText)
    {
        if (!gameManager.IsRunning())
        {
            try
            {
                Func<float, float> f = MakeFunction(submittedText);
                graph.UpdateFunction(f);
            }
            catch
            {
                Debug.Log("Invalid input.");
                SetTextColor(Color.red);
            }
        }
    }

    private void OnDestroy()
    {
        inputField.onValueChanged.RemoveListener(OnValueChanged);
        inputField.onEndEdit.RemoveListener(OnSubmit);
    }

    private void SetTextColor(Color newColor)
    {
        if (textComponent != null)
        {
            textComponent.color = newColor;
        }
    }

    public void DeleteText()
    {
        inputField.text = string.Empty;
    }
}
