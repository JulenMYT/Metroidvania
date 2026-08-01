using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MiniMapToggle : MonoBehaviour
{
    [SerializeField] private GameObject miniMap;
    [SerializeField] private InputActionReference menuToggleAction;

    private void Awake()
    {
        miniMap.SetActive(false);
    }

    private void OnEnable()
    {
        menuToggleAction.action.performed += OnCancel;
        menuToggleAction.action.Enable();
    }

    private void OnDisable()
    {
        menuToggleAction.action.performed -= OnCancel;
        menuToggleAction.action.Disable();
    }

    private void OnCancel(InputAction.CallbackContext context)
    {
        miniMap.SetActive(!miniMap.activeSelf);
    }
}
