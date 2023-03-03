using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace ColorHierarchyNameSpace
{


    public class ColorHierarchyEditor
    {
        [MenuItem("Editor/ColorHierarchy/AddColorHierarchy %H")]
        [MenuItem("GameObject/ColorHierarchy/AddColorHierarchy %H")]
        private static void CreateColorHierarchy()
        {
            GameObject[] obj = Selection.gameObjects;


            for (int i = 0; i < obj.Length; i++)
            {
                Undo.AddComponent<ColorHierarchy>(obj[i]);
            }
        }

        [MenuItem("Editor/ColorHierarchy/RemoveColorHierarchy %#H")]
        [MenuItem("GameObject/ColorHierarchy/RemoveColorHierarchy %#H")]
        private static void RemoveColorHierarchy()
        {
            GameObject[] obj = Selection.gameObjects;


            for (int i = 0; i < obj.Length; i++)
            {
                ColorHierarchy ch = obj[i].GetComponent<ColorHierarchy>();
                if (ch != null)
                {
                    Undo.DestroyObjectImmediate(ch);
                }
            }
        }
    }

}