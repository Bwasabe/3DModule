using static Define;

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using static GrabScreenSwatch;

public class ColorHierarchyWindow : EditorWindow
{
    private static ColorHierarchyWindow _window;


    private static Texture _backgroundTexture;
    private static Blur _blur;

    private Vector2 _scrollView = Vector2.up * 50;

    private Color _color = Color.white;

    private int _selection = 0;

    private string[] _texts = { "이히히", "우히히" };



    [MenuItem("GameObject/ColorHierarchyasdf %H")]
    private static void CreateColorHierarchy()
    {
        Debug.Log("눌림");
        GameObject[] obj = Selection.gameObjects;

        _blur = new Blur(500, 300);
        _backgroundTexture = _blur.BlurTexture(GrabScreenSwatchTexture(MainCam.pixelRect));


        _window = GetWindow<ColorHierarchyWindow>(false, "ColorHierarchy Setting", true);
        _window.Show();

        // _window.wantsMouseEnterLeaveWindow = true;
        // _window.wantsMouseMove = true;
        // for (int i = 0; i < obj.Length; i++)
        // {
        //     Undo.AddComponent<ColorHierarchy>(obj[i]);
        // }
    }


    private void OnGUI()
    {
        DrawWindow();



        var verticalStyle = new GUIStyle();

        verticalStyle.padding = new RectOffset(0, 0, 40, 0);
        EditorGUILayout.BeginVertical(verticalStyle);
        {

            DrawRestoreLabel();
            
            EditorGUILayout.BeginHorizontal();
            {
                _scrollView = EditorGUILayout.BeginScrollView(_scrollView);
                {
                    _selection = GUILayout.SelectionGrid(_selection, _texts, 1);

                    _color = EditorGUILayout.ColorField(_color);
                }
                EditorGUILayout.EndScrollView();
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndVertical();

    }

    private void DrawWindow()
    {
        EditorGUI.BeginChangeCheck();
        {
            GUI.DrawTexture(new Rect(-_window.position.position.x, -_window.position.position.y - 20, MainCam.pixelWidth, MainCam.pixelHeight), _backgroundTexture);
        }
        EditorGUI.EndChangeCheck();
    }

    private void DrawRestoreLabel()
    {
        var areaStyle = new GUIStyle("Box");

        areaStyle.normal.background = MakeTex(2, 2, new Color(0f, 0f, 0f, 0.5f));
        GUILayout.BeginArea(new Rect(0, 0, _window.position.width, 30f), areaStyle);
        {
            var style = new GUIStyle();
            style.alignment = TextAnchor.MiddleCenter;
            style.fontSize = 15;
            style.normal.textColor = Color.green;

            GUILayout.Label($"만약 뒷배경이 안맞을경우 Ctrl + k를 눌러주세요!", style);
        }
        GUILayout.EndArea();
    }

    private static Texture2D MakeTex(int width, int height, Color col)
    {
        Color[] pix = new Color[width * height];
        for (int i = 0; i < pix.Length; ++i)
        {
            pix[i] = col;
        }
        Texture2D result = new Texture2D(width, height);
        result.SetPixels(pix);
        result.Apply();
        return result;
    }



    [MenuItem("GameObject/asfd %k")]
    private static void Tearfds()
    {
        RestoreBlurBackground();
    }

    private static void RestoreBlurBackground()
    {
        bool isError = false;
        try
        {
            _window.Close();
        }
        catch
        {
            isError = true;
        }
        if (!isError)
        {
            _backgroundTexture = _blur.BlurTexture(GrabScreenSwatchTexture(MainCam.pixelRect));
            _window = EditorWindow.GetWindow<ColorHierarchyWindow>(false, "ColorHierarchy Setting", false);
        }
    }


}
