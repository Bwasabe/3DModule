using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class ColorHierarchyEditor : Editor
{

    [MenuItem("GameObject/ColorHierarchy %H")]
    private static void OnClickMenu()
    {
        GameObject[] obj = Selection.gameObjects;

        for (int i = 0; i < obj.Length; i++)
        {
            Undo.AddComponent<ColorHierarchy>(obj[i]);
        }
    }


}
