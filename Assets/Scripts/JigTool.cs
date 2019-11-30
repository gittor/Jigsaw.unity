using UnityEngine;

class jt
{
	static public void log(params string[] logs){
		string txt = "";
		// for(int i=0; i<logs.Length; ++i)
		foreach(string o in logs){
			txt += o;
			txt += " ";
		}
		Debug.Log(txt);
	}
}