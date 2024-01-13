using System.Collections.Generic;
using UnityEngine;

[SingletonLifeTime(SingletonLifeTime.Application)]
public class OnGUIManager : MonoSingleton<OnGUIManager>
{
    private static readonly Dictionary<string, string> GUIDict = new();

    [SerializeField]
    private int fontSize = 30;
    
    public static void SetGUI(string key, object value)
    {
        GUIDict[key] = value.ToString();
    }


    private void OnGUI()
    {
#if UNITY_EDITOR
        if(GUIDict.Count <= 0) return;
        
        GUIStyle label = new();
        label.normal.textColor = Color.red;
        label.fontSize = fontSize;

        foreach (KeyValuePair<string, string> dict in GUIDict)
        {
            GUILayout.Label($"{dict.Key} : {dict.Value}", label);
        }
#endif
    }
}