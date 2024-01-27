using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Settings Menu containing volume settings.
/// Attach to a SettingsMenuCanvas and add a return button (to main menu) as child.
/// </summary>
public class SettingMenu : MonoBehaviour
{
    #region Fields and Properties

    private Canvas _settingsCanvas;
    private Button _backButton;

    #endregion

    #region Methods

    private void Awake()
    {
        _settingsCanvas = GetComponent<Canvas>();
        _settingsCanvas.enabled = false;
    }

    private void OnEnable()
    {
        MainMenu.OnSettingsMenuOpened += OnSettingsOpened;
        MainMenu.OnMainMenuOpened += OnOtherMenuOpened;
        MainMenu.OnCreditsMenuOpened += OnOtherMenuOpened;
    }

    private void Start()
    {
        _backButton = GetComponentInChildren<Button>();
        _backButton.onClick.AddListener(OpenMainMenu);
    }

    private void OnDisable()
    {
        MainMenu.OnSettingsMenuOpened -= OnSettingsOpened;
        MainMenu.OnMainMenuOpened -= OnOtherMenuOpened;
        MainMenu.OnCreditsMenuOpened -= OnOtherMenuOpened;
    }

    private void OnSettingsOpened()
    {
        _settingsCanvas.enabled = true;
    }

    private void OnOtherMenuOpened()
    {
        _settingsCanvas.enabled = false;
    }

    private void OpenMainMenu()
    {
        MainMenu.RaiseMainMenuOpened();
    }

    public void ResumeGameButton()
    {
        PauseControl.ResumeGame();
        _settingsCanvas.enabled = false;
    }

    #endregion
}

