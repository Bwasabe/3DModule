using static Define;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static GrabScreenSwatch;

public class ColorHierarchyWindow : EditorWindow, IWindowDrawable
{
    private float _fadeValue = 1f;

    private int _count = 0;

    private static ColorHierarchyWindow _window;

    private static Texture _backgroundTexture;

    private static Blur _blur;

    private static IWindowDrawable _this;

    private void Awake() {
        //BlurWindow.RegisterWindow(this);
        //BlurWindow.RegisterWindow(DrawWindow);
    }


    [MenuItem("GameObject/ColorHierarchyasdf %H")]
    private static void CreateColorHierarchy()
    {
        GameObject[] obj = Selection.gameObjects;

        _blur = new Blur(500, 300);
        _backgroundTexture = _blur.BlurTexture(GrabScreenSwatchTexture(MainCam.pixelRect));


        _window = GetWindow<ColorHierarchyWindow>(false, "ColorHierarchy Setting", false);
        _window.Show();


        _window.wantsMouseEnterLeaveWindow = true;
        _window.wantsMouseMove = true;
        for (int i = 0; i < obj.Length; i++)
        {
            Undo.AddComponent<ColorHierarchy>(obj[i]);
        }
    }

    private void OnDisable() {
        //BlurWindow.RemoveWindow(this);
        //BlurWindow.RemoveWindow(DrawWindow);
    }


    private static bool _isFocus = false;


    private void OnLostFocus()
    {
        RestoreBlurBackground();
        _isFocus = false;
    }


    private Vector2 _tempWindowPos;
    private void OnGUI()
    {

        EditorGUI.BeginChangeCheck();
        {
            DrawWindow();
        }
        EditorGUI.EndChangeCheck();

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

    public void DrawWindow()
    {
        GUI.DrawTexture(new Rect(-_window.position.position.x, -_window.position.position.y -20, MainCam.pixelWidth, MainCam.pixelHeight), _backgroundTexture);
    }

    [MenuItem("GameObject/asfd %k")]
    private static void Tearfds()
    {
        RestoreBlurBackground();
    }

    private static void RestoreBlurBackground()
    {
        //if(_isFocus)return;
        if (_window == null) return;

        _window.Close();
        _backgroundTexture = _blur.BlurTexture(GrabScreenSwatchTexture(MainCam.pixelRect));
        _window = EditorWindow.GetWindow<ColorHierarchyWindow>(false, "ColorHierarchy Setting", false);

        _isFocus = true;
    }

}
