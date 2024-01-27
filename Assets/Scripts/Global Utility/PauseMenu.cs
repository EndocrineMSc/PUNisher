using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// A simple pause menu for volume control and pausing the game.
/// Put into every scene you want to have the menu in.
/// Default button set to "P", because of WebGL "Esc" incompability.
/// </summary>
public class PauseMenu : MonoBehaviour
{
    #region Fields and Properties

    private Canvas _settingsCanvas;

    #endregion

    #region Methods

    private void Start()
    {
        _settingsCanvas = GetComponent<Canvas>();
        _settingsCanvas.enabled = false;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (PauseControl.GameIsPaused)
            {
                PauseControl.ResumeGame();
                _settingsCanvas.enabled = false;
            }
            else
            {
                PauseControl.PauseGame();
                _settingsCanvas.enabled = true;
            }
        }
    }
    
    public void ResumeGameButton()
    {
        PauseControl.ResumeGame();
        _settingsCanvas.enabled = false;
    }

    #endregion
}
