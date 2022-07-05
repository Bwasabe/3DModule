using UnityEditor;
using UnityEngine;
using System.Collections;

public class Example : EditorWindow
{
    [MenuItem("Example/Mouse Move Example")]
    static void InitWindow()
    {
        Example window = (Example)GetWindowWithRect(typeof(Example), new Rect(0, 0, 300, 100));
        window.Show();
    }

    void OnGUI()
    {
        wantsMouseMove = EditorGUILayout.Toggle("Receive Movement: ", wantsMouseMove);
        EditorGUILayout.LabelField("Mouse Position: ", Event.current.mousePosition.ToString());

        // Repaint the window as wantsMouseMove doesnt trigger a repaint automatically
        if (Event.current.type == EventType.MouseDown){
            Debug.Log("와우");
            Repaint();
        }
    }
}