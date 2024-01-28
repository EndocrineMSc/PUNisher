using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MousePointerInitialization : MonoBehaviour
{
    [SerializeField] Texture2D _cursorCrosshairs;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.SetCursor(_cursorCrosshairs, Vector2.zero, CursorMode.Auto);
        Destroy(gameObject);
    }
}
