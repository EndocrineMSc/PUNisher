using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunManager : MonoBehaviour
{
    #region Fields and Properties

    public static PunManager Instance { get; private set; }
    public int PunsFound { get; private set; }  = 0;

    #endregion

    #region Methods

    private void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);    
    }

    public void PunFound() {
        PunsFound += 1;
    }

    #endregion

}
