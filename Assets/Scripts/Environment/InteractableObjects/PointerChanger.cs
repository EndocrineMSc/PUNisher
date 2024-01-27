using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Fields and Properties

    [SerializeField] private Texture2D _cursorTexture;

    #endregion

    #region Methods

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(_cursorTexture, Vector2.zero, CursorMode.Auto);    
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);    
    }

    #endregion
}
