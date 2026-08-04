using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    [SerializeField] private InputActionReference menuToggleAction;
    [SerializeField] private MenuPanel defaultPanel;
    [SerializeField] private float fadeDuration;

    private MenuPanel currentMenu;
    private MenuPanel lastOpenedMenu;

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
        if (currentMenu != null)
            CloseAll();
        else
        {
            MenuPanel menuToOpen = lastOpenedMenu != null? lastOpenedMenu : defaultPanel;
            StartCoroutine(SwitchRoutine(menuToOpen));
        }
    }

    public void ToggleMenu(MenuPanel panel)
    {
        if (currentMenu == panel)
        {
            StartCoroutine(SwitchRoutine(null));
            return;
        }

        StartCoroutine(SwitchRoutine(panel));
    }

    private void ApplyTimeState(MenuPanel panel)
    {
        if (panel != null && panel.pausesGame)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

    public void CloseAll() => StartCoroutine(SwitchRoutine(null));

    private IEnumerator SwitchRoutine(MenuPanel panel)
    {
        //Close Old Menu
        if (currentMenu != null)
        {
            yield return Fade(currentMenu.canvasGroup, currentMenu.canvasGroup.alpha, 0);
            currentMenu.canvasGroup.interactable = false;
            currentMenu.canvasGroup.blocksRaycasts = false;
            currentMenu.Close();
        }

        //Open New Menu
        currentMenu = panel;

        if (currentMenu != null)
        {
            currentMenu.Open();
            lastOpenedMenu = currentMenu;
            CanvasGroup newGroup = currentMenu.canvasGroup;
            currentMenu.canvasGroup.interactable = true;
            currentMenu.canvasGroup.blocksRaycasts = true;
            yield return Fade(newGroup, newGroup.alpha, 1);
        }

        ApplyTimeState(currentMenu);
    }

    private IEnumerator Fade(CanvasGroup canvasGroup, float from, float to)
    {
        float time = 0f;
        while (time < fadeDuration)
        {
            time += Time.unscaledDeltaTime;
            canvasGroup.alpha = Mathf.Lerp(from, to, time / fadeDuration);
            yield return null;
        }
        canvasGroup.alpha = to;
    }
}
