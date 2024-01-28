using UnityEngine;
using UnityEngine.UI;
using System;
using FMODUnity;

/// <summary>
/// Attach this script to a MainMenu Canvas with the buttons as children.
/// Event-based system for switching between different main menu canvases.
/// </summary>
public class MainMenu : MonoBehaviour
{
    #region Fields and Properties

    private Canvas _menuCanvas;

    [SerializeField] private GameObject _startButton;
    [SerializeField] private GameObject _settingsButton;
    [SerializeField] private GameObject _creditsButton;

    public static event Action OnMainMenuOpened;
    public static event Action OnSettingsMenuOpened;
    public static event Action OnCreditsMenuOpened;

    [SerializeField] EventReference _menuMusic;

    #endregion

    #region Functions

    private void Awake()
    {
        _menuCanvas = GetComponent<Canvas>();
    }

    private void OnEnable()
    {
        OnMainMenuOpened += OnThisMenuOpened;
        OnSettingsMenuOpened += OnOtherMenuOpened;
        OnCreditsMenuOpened += OnOtherMenuOpened;
    }

    private void OnDisable()
    {
        OnMainMenuOpened -= OnThisMenuOpened;
        OnSettingsMenuOpened -= OnOtherMenuOpened;
        OnCreditsMenuOpened -= OnOtherMenuOpened;
    }

    private void Start()
    {
        _startButton.GetComponent<Button>().onClick.AddListener(NewGame);
        _settingsButton.GetComponent<Button>().onClick.AddListener(OpenSettings);
        _creditsButton.GetComponent<Button>().onClick.AddListener(OpenCredits);

        var eventInstance = AudioManager.Instance.CreateInstance(_menuMusic);
        eventInstance.start();
    }

    public static void RaiseMainMenuOpened()
    {
        OnMainMenuOpened?.Invoke();
    }

    public static void RaiseSettingsMenuOpened()
    {
        OnSettingsMenuOpened?.Invoke();
    }

    public static void RaiseCreditsOpened()
    {
        OnCreditsMenuOpened?.Invoke();
    }

    private void OnThisMenuOpened()
    {
        _menuCanvas.enabled = true;
    }

    private void OnOtherMenuOpened()
    {
        _menuCanvas.enabled = false;
    }

    public void OpenSettings()
    {
        PlayButtonClick();
        RaiseSettingsMenuOpened();
    }

    public void OpenCredits()
    {
        PlayButtonClick();
        RaiseCreditsOpened();
    }

    public void NewGame()
    {
        Debug.Log("Click");
        PlayButtonClick();
        AudioManager.Instance.CleanUp(true);
        SceneLoader.Instance.LoadScene(SceneName.FirstDojo);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private void PlayButtonClick()
    {
        //AudioManager.Instance.PlaySoundEffectOnce(SFX._0001_ButtonClick);
    }

    #endregion
}

