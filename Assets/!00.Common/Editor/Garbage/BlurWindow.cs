// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEditor;
// using System;

// [InitializeOnLoad]
// public class BlurWindow : Editor
// {
//     // public static BlurWindow Instance{
//     //     get{
//     //         if(_instance == null){
//     //             return null;
//     //         }
//     //         return _instance;
//     //     }
//     // }

//     // private static BlurWindow _instance;
//     private static List<IWindowDrawable> _windows = new List<IWindowDrawable>();

//     private static Action _windowAction;
//     static BlurWindow(){
//         Debug.Log("생성자");
//         // _instance = new BlurWindow();
//         EditorApplication.update -= UpdateWindow;
//         EditorApplication.update += UpdateWindow;
//     }

//     private static void UpdateWindow(){
//         Debug.Log("업데이트 돌아가는 중");
//         _windows.ForEach(x => x.DrawWindow());
//     }

//     // public static void RegisterWindow(IWindowDrawable window){
//     //     _windows.Add(window);
//     // }

//     // public static void RemoveWindow(IWindowDrawable window){
//     //     _windows.Remove(window);
//     // }

//     public static void RegisterWindow(Action window){
//         _windowAction += window;
//     }

//     public static void RemoveWindow(Action window){
//         _windowAction -= window;
//     }
// }
