using UnityEngine;


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
            else{
                return;
            }
        }
    }

    private static Camera _uiCam;
}
