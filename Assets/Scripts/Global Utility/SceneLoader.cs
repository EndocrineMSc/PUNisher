using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
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
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += FadeInScene;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= FadeInScene;
    }

    public void LoadScene(SceneName sceneName) {
        if (!_isFading)
            StartCoroutine(FadeOutScene(sceneName));
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
    FourthDojo
}
