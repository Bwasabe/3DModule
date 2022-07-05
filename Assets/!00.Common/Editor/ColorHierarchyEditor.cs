using static Define;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static GrabScreenSwatch;

public class ColorHierarchyEditor : EditorWindow
{
    private float _fadeValue = 1f;

    private int _count = 0;

    private static ColorHierarchyEditor _window;

    private static Texture _backgroundTexture;

    private static Blur _blur;

    [MenuItem("GameObject/ColorHierarchyasdf %H")]
    private static void CreateColorHierarchy()
    {
        GameObject[] obj = Selection.gameObjects;

        _blur = new Blur(500, 500);
        RestoreBlurBackground();

        _window = GetWindow<ColorHierarchyEditor>(false, "ColorHierarchy Setting", true);
        _window.Show();

        _window.wantsMouseEnterLeaveWindow = true;
        _window.wantsMouseMove = true;
        // for (int i = 0; i < obj.Length; i++)
        // {
        //     Undo.AddComponent<ColorHierarchy>(obj[i]);
        // }
    }

    private void OnFocus()
    {
        if (_window == null) return;
        _window.Close();
        RestoreBlurBackground();
        _window = EditorWindow.GetWindow<ColorHierarchyEditor>(false, "ColorHierarchy Setting", true);

    }


    private Vector2 _tempWindowPos;
    private void OnGUI()
    {
        
        DrawBlur();

        var style = new GUIStyle();
        style.fontSize = 50;
        style.normal.textColor = Color.red;
        GUILayout.Label($"{Event.current.type}", style);


        _fadeValue = EditorGUILayout.Slider(_fadeValue, 0f, 1f);
        GUILayout.BeginArea(new Rect(50, 50, 500f, 500f), new GUIStyle("Box"));
        {
            // EditorGUILayout.EnumPopup
        }
        GUILayout.EndArea();
    }

    private void DrawBlur()
    {
        // if(isStart)return;

        GUI.DrawTexture(new Rect(-_window.position.position.x, -_window.position.position.y, MainCam.pixelWidth, MainCam.pixelHeight), _backgroundTexture);
    }

    [MenuItem("GameObject/asfd %k")]
    private static void Tearfds()
    {
    }

    private static void RestoreBlurBackground()
    {
        _backgroundTexture = _blur.BlurTexture(GrabScreenSwatchTexture(MainCam.pixelRect));
    }

}
