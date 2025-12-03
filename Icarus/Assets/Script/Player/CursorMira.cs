using UnityEngine;

public class CursorMira : MonoBehaviour
{
    [SerializeField] Texture2D Cursorimg;

    public Vector2 hotSpot = Vector2.zero;
    void Start()
    {
        Cursor.SetCursor(Cursorimg, hotSpot, CursorMode.Auto);

        
        Cursor.visible = true;
    }

   
    void Update()
    {
        
    }
}
