// Create a Horizontal Compound Button

using UnityEngine;
using UnityEditor;

namespace Garbage
{

    public class BeginEndHorizontalExample : EditorWindow
    {
        [MenuItem("Examples/Begin-End Horizontal usage")]
        static void Init()
        {
            BeginEndHorizontalExample window = (BeginEndHorizontalExample)GetWindow(typeof(BeginEndHorizontalExample));
            window.Show();
        }

        void OnGUI()
        {
            EditorGUILayout.BeginVertical();
            {
                GUILayout.BeginArea(new Rect(0, 0, 500, 100));
                GUILayout.EndArea();

                Rect r = EditorGUILayout.BeginHorizontal("Button");
                if (GUI.Button(r, GUIContent.none))
                    Debug.Log("Go here");
                GUILayout.Label("I'm inside the button");
                GUILayout.Label("So am I");
                EditorGUILayout.EndHorizontal();

                Rect re = EditorGUILayout.BeginHorizontal("Button");
                if (GUI.Button(r, GUIContent.none))
                    Debug.Log("Go here");
                GUILayout.Label("I'm inside the button");
                GUILayout.Label("So am I");
                EditorGUILayout.EndHorizontal();
            }
            EditorGUILayout.EndVertical();
        }
    }
}