using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GrabScreenSwatch {

  public static Texture GrabScreenSwatchTexture(Rect rect) {
    int width = (int)rect.width;
    int height = (int)rect.height;
    int x = (int)rect.x;
    int y = (int)rect.y;
    Vector2 position = new Vector2(x, y);
  
    Color[] pixels = UnityEditorInternal.InternalEditorUtility.ReadScreenPixel(position, width, height);
  
    Texture2D texture = new Texture2D(width, height);
    texture.SetPixels(pixels);
    texture.Apply();
  
    return texture;
  }  
}
