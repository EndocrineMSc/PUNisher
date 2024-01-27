using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class PunCounter : MonoBehaviour
{
    #region Fields and Properties

    [SerializeField] TextMeshProUGUI _counter;
    int _maxPunCount;

    #endregion

    #region Methods

    private void Start() {
        var punObjects = GameObject.FindGameObjectsWithTag("PunObject");
        _maxPunCount = punObjects.Length;
    }

    private void Update() {
        _counter.text = PunManager.Instance.PunsFound + " / " + _maxPunCount;    
    }

    #endregion
}
