using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Animations;

/// <summary>
/// 一个操作掩码的辅助类
/// </summary>
public static class JigtileMaskMode
{
    //反转把edge的形状
	static public char reverse(char edge)
    {
		switch(edge)
		{
			case 'f': return 'f';
			case 'v': return 'a';
			case 'a': return 'v';
			default: return 'o';
		}
	}

    //随机产生一条形状的边
	static public char random()
    {
		int rand = Random.Range(0, 2);
		switch(rand)
		{
			case 0: return 'a';
			case 1: return 'v';
			default: return 'o';
		}
	}

    //返回mode顺时针旋转90度后的样子
	static public string deform(string mode)
    {
		return string.Format("{0}{1}{2}{3}", mode[3], mode[0], mode[1], mode[2]);
	}

    //mode可能是被旋转过的，用format可以得到存储了图片的mode
	static public string format(string mode)
    {
		HashSet<string> candy = new HashSet<string>();
		candy.Add("aaaa");
		candy.Add("aaaf");
		candy.Add("faaf");
		candy.Add("ffva");
		candy.Add("vaaa");
		candy.Add("vafa");
		candy.Add("vava");
		candy.Add("vfaa");
		candy.Add("vfav");
		candy.Add("vffa");
		candy.Add("vfva");
		candy.Add("vvaa");
		candy.Add("vvav");
		candy.Add("vvfa");
		candy.Add("vvff");
		candy.Add("vvva");
		candy.Add("vvvf");
		candy.Add("vvvv");

		for(int i=0; i<4; ++i)
        {
			if(candy.Contains(mode)){
				return mode;
			}
			else{
				// Debug.Log("format");
				mode = JigtileMaskMode.deform(mode);
			}
		}

		// print(string.Format("Can not found:{0}", mode));
		jt.log("can not find ", mode);
		Debug.Assert(false);

		return null;
	}
}