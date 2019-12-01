using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimImage : MonoBehaviour
{
    public Texture[] Textures;
    public int CurFrame;

    public float FrameDiff = 0.15f;

    public delegate void OnTextureChangedHandle(Texture texture);
    public OnTextureChangedHandle OnTextureChanged;

    void Start()
    {
        
    }

    public void SetActive()
	{
		InvokeRepeating("ChangeTexture", 0.0f, FrameDiff);
	}
    

    void ChangeTexture()
    {
        if (Textures.Length == 0)
        {
            return;
        }

        var tex = Textures[CurFrame];
        OnTextureChanged(tex);
        
        ++CurFrame;
        if (CurFrame == Textures.Length)
        {
            CurFrame = 0;
        }
    }
}
