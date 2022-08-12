using System;
using UnityEngine;
using Random = UnityEngine.Random;

public enum FADECHILDS
{
    FADEOBJECT,
    TOPBAR,
    BOTTOMBAR,
}

public static class Define
{

    public static Camera MainCam
    {
        get
        {
            if (_mainCam == null)
            {
                _mainCam = Camera.main;
            }
            return _mainCam;
        }

    }

    private static Camera _mainCam;

    public static Camera UICam
    {
        get
        {
            return _uiCam;
        }
        set
        {
            if (_uiCam == null)
            {
                _uiCam = value;
            }
            else
            {
                return;
            }
        }
    }

    private static Camera _uiCam;

    public static Transform FadeParent
    {
        get
        {
            if (_fadeParent == null)
            {
                _fadeParent = GameObject.Find("FadeParent").transform;
            }
            return _fadeParent;
        }
    }

    private static Transform _fadeParent;



    public static Vector2 MousePos => MainCam.ScreenToWorldPoint(Input.mousePosition);

    public static T GetRandomEnum<T>(bool isNone = false, bool isLength = false)
    {
        Array values = Enum.GetValues(typeof(T));
        return (T)values.GetValue(Random.Range((isNone) ? 1 : 0, (isLength) ? values.Length - 1 : values.Length));
    }
}
