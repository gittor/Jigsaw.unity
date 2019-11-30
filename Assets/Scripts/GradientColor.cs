using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


[RequireComponent(typeof(Image))]
public class GradientColor : BaseMeshEffect
{
    public Color32 topColor = Color.white;
    public Color32 bottomColor = Color.black;

    //后面自己添加的控制中心移动属性，有时候看着渐变不顺眼，中心偏离高或者低了，就可以通过这个去调整
    [RangeAttribute(0, 1)]
    public float center = 0.5f;

	public float ShrinkDiff = 0.2f;

    public override void ModifyMesh(VertexHelper vh)
    {
        if (!IsActive())
        {
            return;
        }

        var count = vh.currentVertCount;
        if (count == 0)
            return;

        var vertexs = new List<UIVertex>();
        for (var i = 0; i < count; i++)
        {
            var vertex = new UIVertex();
            vh.PopulateUIVertex(ref vertex, i);
            vertexs.Add(vertex);
        }

        var topY = vertexs[0].position.y;
        var bottomY = vertexs[0].position.y;

        for (var i = 1; i < count; i++)
        {
            var y = vertexs[i].position.y;
            if (y > topY)
            {
                topY = y;
            }
            else if (y < bottomY)
            {
                bottomY = y;
            }
        }

		
		var height = topY - bottomY;
        for (var i = 0; i < count; i++)
        {
            var vertex = vertexs[i];

            //使用处理过后的颜色
            // var color = Color32.Lerp(bottomColor, topColor, (vertex.position.y - bottomY) / height);
            var color = CenterColor(bottomColor, topColor, (vertex.position.y - bottomY) / height);
            vertex.color = color;

			//缩小底边两个顶点的距离，模拟梯形
			if (i == 0)
			{
				vertex.position += new Vector3(ShrinkDiff, 0, 0);
			}
			else if (i == 3)
			{
                vertex.position -= new Vector3(ShrinkDiff, 0, 0);
			}

            vh.SetUIVertex(vertex, i);
        }
	}
    //加了一个对颜色处理的函数，主要调整中心的位置
    private Color32 CenterColor(Color32 bc, Color32 tc, float time)
    {
        if (center == 0)
        {
            return bc;
        }
        else if (center == 1)
        {
            return tc;
        }
        else
        {
            var centerColor = Color32.Lerp(bottomColor, topColor, 0.5f);
            var resultColor = tc;
            if (time < center)
            {
                resultColor = Color32.Lerp(bottomColor, centerColor, time / center);
            }
            else
            {
                resultColor = Color32.Lerp(centerColor, topColor, (time - center) / (1 - center));
            }
            return resultColor;
        }
    }
}
