using System;
using UnityEngine;

public static class InputCheck
{
    public static KeyCode GetInput()
    {
        try
        {
            Event e = Event.KeyboardEvent(Input.inputString[0].ToString());
            return e.keyCode;
        }
        catch
        {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    Event e = Event.KeyboardEvent(key.ToString());
                    return e.keyCode;
                }
            }
        }
        return KeyCode.None;
    }

}
