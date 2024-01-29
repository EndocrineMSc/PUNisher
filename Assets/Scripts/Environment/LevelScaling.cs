using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScaling : MonoBehaviour
{
    #region Fields and Properties

    public static LevelScaling Instance {get; private set;}

    [field: SerializeField] public LevelScalingData SecondDojoData {get; private set;} 
    [field: SerializeField] public LevelScalingData ThirdDojoData {get; private set;} 
    [field: SerializeField] public LevelScalingData FourthDojoData {get; private set;} 

    #endregion

    #region Methods

    private void Awake() {
        if (Instance == null) {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else {
            Destroy(gameObject);
        }
    }

    #endregion
}
