using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputCheck : MonoBehaviour
{
    private Event e;

    public KeyCode InputKeyCheck()
    {
        try
        {
            e = Event.KeyboardEvent(Input.inputString[0].ToString());
        }
        catch
        {
            foreach (KeyCode key in Enum.GetValues(typeof(KeyCode)))
            {
                if (Input.GetKeyDown(key))
                {
                    e = Event.KeyboardEvent(key.ToString());
                }
            }
        }
        return e.keyCode;
    }

    // Example

    // private void Update()
    // {
    //     if(Input.anyKeyDown){
    //         Debug.Log(InputKeyCheck());
    //     }

    // }
}
