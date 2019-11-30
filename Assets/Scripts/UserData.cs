using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UserData
{
    public int CurrentLevel { get; private set; }
    public void NotifyFinishedLevel(int level)
    {
        if (CurrentLevel==level)
        {
            CurrentLevel = level+1;
        }
    }

    private static UserData mInstance;
    public static UserData Instance
    {
        get
        {
            if (mInstance == null)
            {
                mInstance = new UserData();
            }
            return mInstance;
        }
    }

    public float ScreenScale
    {
        get
        {
            return Screen.height / 640.0f;
        }
    }
}
