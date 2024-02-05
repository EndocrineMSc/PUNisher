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
    [SerializeField] private GameObject overlay;

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
        overlay.SetActive(true);
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
       
    }

    bool audioInitStarted = false;
    public void StartAudioInitialization()
    {
        audioInitStarted = true;
    }

    private void Update()
    {
        if (audioInitStarted)
        {

            Debug.Log("Banks loaded: " + FMODUnity.RuntimeManager.HaveAllBanksLoaded);
            if (!FMODUnity.RuntimeManager.HaveAllBanksLoaded)
            {
                AudioManager.Instance.LoadBanks();
            }
            else
            {
                if(!audioResumed)
                    ResumeAudio();
            }           
        }   

    }

    bool audioResumed = false;

    public void ResumeAudio()
    {
        if (!audioResumed)
        {
            AudioManager.Instance.SetBusses();
            var result = FMODUnity.RuntimeManager.CoreSystem.mixerSuspend();
            Debug.Log(result);
            result = FMODUnity.RuntimeManager.CoreSystem.mixerResume();
            Debug.Log(result);
            audioResumed = true;
            StartSound();
        }
    }
    public void StartSound()
    {
        if (!userActive) { 
            userActive = true;
            overlay.SetActive(false);
            var eventInstance = AudioManager.Instance.CreateInstance(_menuMusic);
            eventInstance.start();
            Debug.Log($"user active {userActive}");
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
        FindObjectOfType<SceneLoader>().LoadScene(SceneName.FirstDojo);
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

