using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEditor;
using UnityEngine.UI;

/// <summary>
/// 一个测试类，无实际作用
/// </summary>
public class MyVideo : MonoBehaviour {

    public VideoPlayer vPlayer;
    
	void Start () {
        RenderTexture tex = new RenderTexture(200, 200, 24);
        vPlayer.targetTexture = tex;
        
    }
	
	// Update is called once per frame
	void Update () {
        Debug.Log("Frame " + vPlayer.frame);
    }
    private void OnGUI()
    {
        if(GUILayout.Button("开始/暂停"))
        {
            if (vPlayer.isPlaying)
            {
                vPlayer.Pause();
            }
            else
            {
                vPlayer.Play();
            }
        }
        RectTransform rc = this.transform as RectTransform;
        GUI.DrawTexture(rc.rect, vPlayer.targetTexture);
    }
}
