using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScaling : MonoBehaviour
{
    #region Fields and Properties

    public static LevelScaling Instance {get; private set;}

    [field: SerializeField] public Vector3 MaxScale {get; private set;}
    [field: SerializeField] public Vector3 MinScale {get; private set;}

    #endregion

    #region Methods

    private void Awake() {
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(gameObject);
        }
    }

    #endregion

}
