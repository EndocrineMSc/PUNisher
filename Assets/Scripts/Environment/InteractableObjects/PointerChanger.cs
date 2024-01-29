using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerChanger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    #region Fields and Properties

    [SerializeField] private Texture2D _cursorTextureMagnify;

    #endregion

    #region Methods

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(_cursorTextureMagnify, Vector2.zero, CursorMode.Auto);    
    } 
    
    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);    
    }

    private void OnDestroy() {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);   
    }

    #endregion
}
