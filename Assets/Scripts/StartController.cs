using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.IO;

/// <summary>
/// 用于开始场景的控制类，挂接到Canvas上
/// </summary>
public class StartController : MonoBehaviour {

    private VideoPlayer video;

	void Start () {
    }
	
	public void OnClickStart()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("ChooseLevel");
    }
}
