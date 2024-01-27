using UnityEngine;

public class FMODEvents : MonoBehaviour
{
    #region Fields and Properties

    public static FMODEvents Instance { get; private set; }

    #endregion

    #region Methods

    void Awake() {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    #endregion
}
