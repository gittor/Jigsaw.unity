using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 每个小图块上的脚本
/// 挂接在jigtile_mod上
/// </summary>
public class JigTile : MonoBehaviour {

    //使用的掩码图
	public Image mask;

    //完整的大图
	public RawImage jigimg;

    //所使用掩码图的模式，上右下左依次排列，可选项为f/v/a
	[SerializeField] private string m_mode;

    //此图块拼好后的位置，也是初始位置
    public Vector2 perfectPos;

    void Start()
    {
        float jigmapsize = (this.transform.parent as RectTransform).rect.width;
        
        (this.jigimg.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, jigmapsize);
        (this.jigimg.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, jigmapsize);

        float scale = Screen.height / 640.0f;
        this.jigimg.transform.Translate((new Vector2(jigmapsize / 2, jigmapsize / 2) - GetPosition())*scale, Space.Self);
    }

    public void SetPosition(Vector2 pos)
    {
		RectTransform rt = this.transform as RectTransform;
		rt.anchoredPosition = pos;
	}

	public Vector2 GetPosition()
    {
		RectTransform rt = this.transform as RectTransform;
		return rt.anchoredPosition;
	}

	public void SetSize(float tilesize)
    {
        (transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, tilesize);
		(transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, tilesize);
		float masksize = 150*tilesize/100;
		(mask.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, masksize);
		(mask.transform as RectTransform).SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, masksize);
	}

    //设置使用哪张碰撞掩码图，用vfa代表，上右下左4个字符
	public void SetMask(string maskmode)
    {
		m_mode = maskmode;

        //如果有旋转的情况，需要通过旋转掩码图片得到正确的掩码形状
		for(int i=0; i<4; ++i)
		{
			string file = string.Format("sds-{0}", maskmode);
			this.mask.sprite = Resources.Load<Sprite>(file);

			if(this.mask.sprite)
            {
				this.mask.transform.Rotate(Vector3.forward*90*i);
				this.jigimg.transform.Rotate(-Vector3.forward*90*i);
				break;
			}
			else
            {
				maskmode = JigtileMaskMode.deform(maskmode);
			}
		}

        Debug.Assert(this.mask.sprite, string.Format("找不到sds-{0}", maskmode));		
	}

	public string GetMask()
    {
		return m_mode;
	}

    public void SetTexture(Texture texture)
    {
        jigimg.texture = texture;
    }
}
