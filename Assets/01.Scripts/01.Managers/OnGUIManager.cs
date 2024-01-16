using System.Collections.Generic;
using UnityEngine;

public class OnGUIManager : MonoBehaviour
{
    private static readonly Dictionary<string, string> GUIDict = new();

    [SerializeField]
    private int fontSize = 30;
    
    public static void SetGUI(string key, object value)
    {
        GUIDict[key] = value.ToString();
    }
    
    private void Awake()
    {
#if UNITY_EDITOR

        DontDestroyOnLoad(gameObject);
#else
        Destroy(gameObject);
#endif
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