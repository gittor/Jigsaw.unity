using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

/// <summary>
/// 用此类控制关卡以何种参数启动
/// 挂接到JigsawScene的Canvas上，并设置好需要导入的变量
/// </summary>
public class LevelController : MonoBehaviour {

    //加载哪一关，需要在关卡开始前配置好
    static public int curLevel = 0;
    //把大图分割成几乘几的小图
    static public int curNXN = 3;
    //打乱小图时是否同时旋转小图
    static public bool RotateMode = false;


    public Button pause;
    public Button finish;
    public Text title;
    public RawImage TipImage;
    public Toggle ShowTip;

    public JigsawController jc;

    //没什么具体作用，为了调试用
    public VideoPlayer video;

    private void Start()
    {
        video.url = Path.Combine(Application.streamingAssetsPath, string.Format("movie{0}.mov", (curLevel + 1)));
        print(video.url);
        video.Play();

        jc.Cleanup();

        pause.enabled = false;
        finish.enabled = false;

        Invoke("GameStart", 1.4f*2);

        jc.Split(curNXN);
        jc.Shuffle(RotateMode);
        jc.Restart(video.url);
    }

    //开始洗牌动画结束后才让用户可以操作
    void GameStart()
    {
        pause.enabled = true;
        finish.enabled = true;
        TipImage.enabled = false;
        ShowTip.isOn = false;
    }

    //暂时没有实现暂停功能
    public void OnPauseClick()
    {
        print("别点");
    }

    public void OnBackClick()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("ChooseLevel");
    }

    public void OnFinishClick()
    {
        if (jc.IsAllFinished())
        {
            UserData.Instance.NotifyFinishedLevel(curLevel);

            UnityEngine.SceneManagement.SceneManager.LoadScene("ChooseLevel");
        }
        else
        {
            print("un");
        }
    }

    public void OnShowTipChanged()
    {
        TipImage.enabled = ShowTip.isOn;
    }
}
