using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ChooseLevel : MonoBehaviour
{
    public RectTransform ScrollContent;

    // Start is called before the first frame update
    void Start()
    {
        int itemCount = 6;
        float itemHeight = 120;

        float contentHeight = itemCount * itemHeight + 40;
        ScrollContent.sizeDelta = new Vector2(ScrollContent.sizeDelta.x, contentHeight);

        for (int i = 0; i < itemCount; i++)
        {
            GameObject prefab = Resources.Load<GameObject>("ChooseLevelItem");

            GameObject inst = Instantiate(prefab);

            inst.transform.SetParent(ScrollContent, false);

            RectTransform rt = inst.transform as RectTransform;

            float itemWidth = rt.sizeDelta.x;

            //

            rt.anchoredPosition = new Vector2(-itemWidth / 2, -i * itemHeight - 20 + contentHeight / 2);

            Button button = inst.GetComponentInChildren(typeof(Button)) as Button;
            button.name = i.ToString();

            button.GetComponentInChildren<Text>().text = GetButtonText(i);

            button.onClick.AddListener(delegate() {
                OnItemSelected(button);
            });

            button.enabled = i <= UserData.Instance.CurrentLevel;
        }

        //ScrollContent.anchoredPosition = new Vector2(0, 0);
    }

    public void OnItemSelected(Button sender)
    {
        LevelController.curLevel = int.Parse(sender.name);
        LevelController.curNXN = GetNxNByLevel(LevelController.curLevel);
        LevelController.RotateMode = GetUseRotate(LevelController.curLevel);

        UnityEngine.SceneManagement.SceneManager.LoadScene("JigsawScene");
    }

    private string GetButtonText(int index)
    {
        string[] vs =
        {
            "初入江湖"
            ,"小有名气"
            ,"名动一方"
            ,"天下闻名"
            ,"一代宗师"
            ,"超凡入圣"
            ,"天外飞仙"
        };

        if (index<vs.Length)
        {
            return vs[index];
        }
        return string.Format("未命名{0}", index);
    }

    private int GetNxNByLevel(int level)
    {
        //return 2;
        switch(level)
        {
            case 0: return 2;
            case 1: return 3;
            case 2: return 3;
            case 3: return 4;
            case 4: return 4;
            case 5: return 5;
            case 6: return 5;
            default: return 3;
        }
    }

    private bool GetUseRotate(int level)
    {
        switch (level)
        {
            case 0: return false;
            case 1: return false;
            case 2: return false;
            case 3: return false;
            case 4: return false;
            case 5: return false;
            default: return false;
        }
    }
}
