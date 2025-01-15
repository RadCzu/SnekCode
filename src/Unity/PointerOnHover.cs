using UnityEngine;
using UnityEngine.EventSystems;

public class PointerOnHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Texture2D handCursor; 

    public void OnPointerEnter(PointerEventData eventData)
    {
        Cursor.SetCursor(handCursor, Vector2.zero, CursorMode.Auto);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
