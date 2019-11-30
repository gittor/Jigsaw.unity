using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class MoveTo : MonoBehaviour {

    public Vector2 target;
    public float totalTime = 0.2f;
    public float delayTime = 0;
    private Vector2 movestep;
    private int movecount;
    private const float rate = 0.05f;
	void Start () {
        Vector2 cur = (this.transform as RectTransform).anchoredPosition;
        movestep = target - cur;
        movestep /= (totalTime/ rate);
        movecount = (int)(totalTime / rate);
        InvokeRepeating("Execute", delayTime, rate);
	}
	void Execute()
    {
        (this.transform as RectTransform).anchoredPosition += movestep;
        //this.transform.Translate(movestep);
        movecount--;
        if(movecount==0)
        {
            //CancelInvoke("Execute");
            Destroy(this);
        }
    }
}
