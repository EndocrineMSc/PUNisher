using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EagleEyeMarkerVisualizer : MonoBehaviour
{
    #region Fields and Properties

    [SerializeField] private Image _eagleEyeMarker;

    #endregion

    #region Methods

    void Start() {
        _eagleEyeMarker.enabled = false;
    }

    void Update() {
        if (Input.GetKeyDown(KeyCode.Space)) {
            _eagleEyeMarker.enabled = true;
        }

        if (Input.GetKeyUp(KeyCode.Space)) {
            _eagleEyeMarker.enabled = false;
        }
    }

    #endregion
}
