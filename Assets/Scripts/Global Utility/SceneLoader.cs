using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using FMODUnity;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SceneLoader : MonoBehaviour
{
    #region Fields and Properties

    public static SceneLoader Instance {get; private set;}

    [SerializeField] private Image _blackScreen;
    [SerializeField] private float _fadeDuration = 2f;
    private bool _isFading = false;

    [SerializeField] EventReference _menuMusic;
    [SerializeField] EventReference _dojo1;
    [SerializeField] EventReference _dojo2;
    [SerializeField] EventReference _dojo3;
    [SerializeField] EventReference _dojo4;

    #endregion

    #region Methods

    private void Awake() {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        _blackScreen.enabled = true;
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += FadeInScene;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= FadeInScene;
    }

    public void LoadScene(SceneName sceneName) {
        if (!_isFading) {
            AudioManager.Instance.CleanUp(true);
            StartCoroutine(FadeOutScene(sceneName));

            switch(sceneName) {
                case SceneName.MainMenu:
                    //if (AudioManager.Instance.bussesInitialized)
                    //{
                    //    AudioManager.Instance.CreateInstance(_menuMusic).start();
                        
                    //}Debug.Log("SceneLoader started _menuMusic"); //never happens
                    break;
                case SceneName.FirstDojo:
                    AudioManager.Instance.CreateInstance(_dojo1).start();
                    break;
                case SceneName.SecondDojo:
                    AudioManager.Instance.CreateInstance(_dojo2).start();
                    break;
                case SceneName.ThirdDojo:
                    AudioManager.Instance.CreateInstance(_dojo3).start();
                    break;
                case SceneName.FourthDojo:
                    AudioManager.Instance.CreateInstance(_dojo4).start();
                    break;
            }
        }
    }

    private void FadeInScene(Scene scene, LoadSceneMode mode) {
        //parameters not needed
        _blackScreen.DOFade(0, _fadeDuration);
    }

    private IEnumerator FadeOutScene(SceneName sceneName) {
        _isFading = true;
        _blackScreen.DOFade(1, _fadeDuration);
        yield return new WaitForSeconds(_fadeDuration);
        _isFading = false;
        SceneManager.LoadSceneAsync(sceneName.ToString());
    }

    #endregion
}

public enum SceneName {
    MainMenu,
    FirstDojo,
    SecondDojo,
    ThirdDojo,
    FourthDojo,
    EndScene
}
