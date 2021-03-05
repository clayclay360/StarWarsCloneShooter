using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorController : MonoBehaviour
{
    [SerializeField]
    private Texture2D[] cursorAim;

    public int cursorTextureNumber;

    // Update is called once per frame
    void Update()
    {
        Cursor.SetCursor(cursorAim[cursorTextureNumber], new Vector2(250, 250), CursorMode.Auto);
    }
}
