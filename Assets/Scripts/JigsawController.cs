using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using UnityEngine.Animations;

/// <summary>
/// 控制大图如何切割成小图等一系列操作
/// 需要挂接到JigsawScene上的clipBg上
/// </summary>
public class JigsawController : MonoBehaviour, TouchListener
{

    public RawImage test;

    //开始的时候要打乱各个小拼图块，打乱的方式是将小拼图块随机发送到startRect的矩形区域里
    public RectTransform startRect;

    //建立每个小块时使用的模板
    public GameObject jigtile_mod;

    //运行时创建的所有拼图小块
    private List<JigTile> m_jigtiles = new List<JigTile>();

    //用户当前操作的小块
    private JigTile m_active;

	//当前被分成了几行几列
	private int RC;

    private float TileSize
    {
        get
        {
            return (this.transform as RectTransform).rect.width / RC;
        }
    }

    void Start()
    {
    }

    public void Restart()
    {
        StartCoroutine(StartAnimation());
    }

    //设置使用的大图
    public void SetTexture(Texture texture)
    {
        for (int i = 0; i < m_jigtiles.Count; i++)
        {
            m_jigtiles[i].SetTexture(texture);
        }
    }

    //本关卡是否所有小块都已经拼好了
    public bool IsAllFinished()
    {
        for (int i = 0; i < m_jigtiles.Count; ++i)
        {
            if (m_jigtiles[i].perfectPos != m_jigtiles[i].GetPosition())
            {
                return false;
            }
        }
        return true;
    }

    //开局时随机打乱小块的动画
    IEnumerator StartAnimation()
    {
        //print(startRect.position);
        yield return new WaitForSeconds(1);
        for (int i = 0; i < m_jigtiles.Count; ++i)
        {
            yield return new WaitForSeconds(0.05f);

            Vector2 pos = new Vector2();

            float[] xcandy =
            {
                -100
                , 500 + 100
            };

            pos.x = xcandy[Random.Range(0, xcandy.Length)];
            pos.y = Random.Range(189, 300);

            MoveTo moveto = m_jigtiles[i].gameObject.AddComponent<MoveTo>();
            moveto.target = pos;
        }
    }

    //将video分割成rc*rc个小块，并保存到m_jigtiles里
    public void Split(int rc)
    {
        RC = rc;

        for (int r = 0; r < rc; ++r)
        {
            for (int c = 0; c < rc; ++c)
            {
                GameObject obj = Instantiate(jigtile_mod);

                obj.name = string.Format("tile{0}{1}", r, c);

                obj.GetComponent<TouchController>().listeners.Add(this);

                JigTile tile = obj.GetComponent<JigTile>();

                m_jigtiles.Add(tile);

                obj.transform.SetParent(this.gameObject.transform, false);
            }
        }
    }

    public void Shuffle(bool rotmode)
	{
		shuffle_mode(RC);

		if (rotmode)
		{
			shuffle_rot();

		}
	}

    //m_jigtiles里的小块是没有边界变换的，用这个方法给每个小块都分配合理的边界
    void shuffle_mode(int rc)
    {
        float tilesize = (this.transform as RectTransform).rect.width / rc;

        //坐下角是(0,0)
        for (int r = 0; r < rc; ++r)
        {
            for (int c = 0; c < rc; ++c)
            {
                m_jigtiles[r * rc + c].SetSize(tilesize);

                char up = getmode(rc, r, c, r + 1, c, 2);
                char left = getmode(rc, r, c, r, c - 1, 1);
                char right = getmode(rc, r, c, r, c + 1, 3);
                char down = getmode(rc, r, c, r - 1, c, 0);

                string mode = string.Format("{0}{1}{2}{3}", up, right, down, left);

                m_jigtiles[r * rc + c].SetMask(mode);


                Vector2 perfect = new Vector2(c * tilesize + tilesize / 2, r * tilesize + tilesize / 2);

                m_jigtiles[r * rc + c].SetPosition(perfect);
                m_jigtiles[r * rc + c].perfectPos = perfect;
            }
        }
    }

    void shuffle_rot()
	{
		for (int i = 0; i < m_jigtiles.Count; i++)
		{
			Vector3 axis = new Vector3(0, 0, 1);
			float radian = Random.Range(0, 4) * 90 * Mathf.Deg2Rad;
			m_jigtiles[i].gameObject.transform.Rotate(axis, radian);

			print(string.Format("{0}.rot={1}", m_jigtiles[i].gameObject.name, radian*Mathf.Rad2Deg));
		}
	}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="rc">一共有几行几列</param>
    /// <param name="r">要生成的是哪个图块</param>
    /// <param name="c">要生成的是哪个图块</param>
    /// <param name="r2">相邻的是哪个图块</param>
    /// <param name="c2">相邻的是哪个图块</param>
    /// <param name="rc2idx">(r2,c2)与(r,c)相邻时，(r2,c2)边的索引，上右下左分别用0123代替</param>
    /// <returns></returns>
	char getmode(int rc, int r, int c, int r2, int c2, int rc2idx)
    {
        if (r2 < 0 || r2 >= rc)
            return 'f';
        if (c2 < 0 || c2 >= rc)
            return 'f';

        int rc2 = r2 * rc + c2;

        string candy = m_jigtiles[rc2].GetMask();

        //如果相邻的图块还没设置过边界，就给(r,c)随机设置一个边界
        //如果相邻的图块已经设置了边界，就给(r,c)设置成相反的
        if (string.IsNullOrEmpty(candy))
        {
            return JigtileMaskMode.random();
        }
        else
        {
            char mode = candy[rc2idx];
            mode = JigtileMaskMode.reverse(mode);
            return mode;
        }
    }

    //清除所有生成的内容，以便于再次开始
    public void Cleanup()
    {
        for (int i = 0; i < m_jigtiles.Count; ++i)
        {
            Destroy(m_jigtiles[i].gameObject);
        }
        m_jigtiles.Clear();
    }

    public void onTouchBegan(TouchController con)
    {
        m_active = con.gameObject.GetComponent<JigTile>();
        Debug.Assert(m_active);
        //print(m_active);
    }
    public void onTouchMoved(TouchController con)
    {
        if (!m_active)
            return;

        Vector2 pos = m_active.GetPosition();
        pos += con.diffpos();
        m_active.SetPosition(pos);
    }
    public void onTouchEnded(TouchController con)
    {
        if (!m_active)
            return;

        //限制用户不能把小图块移出屏幕
        Vector3 diff = m_active.transform.position;
        diff.x = Mathf.Min(diff.x, 0);
        diff.y = Mathf.Min(diff.y, 0);
        m_active.SetPosition(m_active.GetPosition() - (Vector2)diff);

        diff = m_active.transform.position;
        diff.x = Mathf.Max(diff.x - Screen.width, 0);
        diff.y = Mathf.Max(diff.y - Screen.height, 0);
        m_active.SetPosition(m_active.GetPosition() - (Vector2)diff);

        //如果用户把小图块移动到正确位置附近，就产生一个吸附效果
        diff = m_active.GetPosition() - m_active.perfectPos;
        //print(diff);
        if (Mathf.Abs(diff.x) < 10 && Mathf.Abs(diff.y) < 10)
        {
            m_active.SetPosition(m_active.perfectPos);
        }

        m_active = null;
    }
}
