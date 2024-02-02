using UnityEngine;
using UnityEngine.UI;
using System;
using FMODUnity;
using FMOD.Studio;

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
    [SerializeField] private GameObject button;

    public static event Action OnMainMenuOpened;
    public static event Action OnSettingsMenuOpened;
    public static event Action OnCreditsMenuOpened;

    [SerializeField] EventReference _menuMusic;
    public bool userActive = false;

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
        button.GetComponent<Button>().onClick.AddListener(StartSound);
    }
    //private void Update()
    //{
    //    if (_menuCanvas.enabled && !userActive) {
    //        if (Input.GetMouseButtonDown(0))
    //        {
    //            var eventInstance = AudioManager.Instance.CreateInstance(_menuMusic);
    //            eventInstance.start();
    //            userActive = true;
    //        }
    //    }
        
    //}
    public void StartSound()
    {
        if (!userActive) { 
            var eventInstance = AudioManager.Instance.CreateInstance(_menuMusic);
            eventInstance.start();
            userActive = true;
            Debug.Log($"user active {userActive}");
            Destroy(button);
        }
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

