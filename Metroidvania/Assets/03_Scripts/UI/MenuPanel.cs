using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class MenuPanel : MonoBehaviour
{
    [SerializeField] private Button toggleButton;
    [SerializeField] private InputActionReference menuToggleAction;

    public CanvasGroup canvasGroup;

    public bool pausesGame;

    protected virtual void OnEnable()
    {
        menuToggleAction.action.performed += ToggleMenu;
    }

    protected virtual void OnDisable()
    {
        menuToggleAction.action.performed -= ToggleMenu;
    }

    public virtual void Open() { }
    public virtual void Close() { }

    protected virtual void ToggleMenu(InputAction.CallbackContext context)
    {
        toggleButton.onClick.Invoke();
    }
}
