using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface TouchListener {
	void onTouchBegan(TouchController con);
	void onTouchMoved(TouchController con);
	void onTouchEnded(TouchController con);
}

public class TouchController : MonoBehaviour {

	public List<TouchListener> listeners = new List<TouchListener>();
	private TouchPhase m_phase;
	private bool m_phasevalid = false;
	private Vector2 m_startTouch;
	void Start () {
	}
	void Update () {
		if(Input.GetMouseButtonDown(0)){
			RectTransform t = this.gameObject.transform as RectTransform;
			if(!t.rect.Contains(pos())){
				return;
			}
			m_phase = TouchPhase.Began;
			m_phasevalid = true;
		}
		else if(Input.GetMouseButtonUp(0)){
			m_phase = TouchPhase.Ended;
		}
		else if(Input.GetMouseButton(0)){
			m_phase = TouchPhase.Moved;
		}
	}
	public Vector2 pos() {
		Vector2 pos = Vector2.zero;
		RectTransformUtility.ScreenPointToLocalPointInRectangle(gameObject.transform as RectTransform, Input.mousePosition, null, out pos);
		return pos;
	}
	public Vector2 diffpos(){
		return pos()-m_startTouch;
	}
	void FixedUpdate(){
		if(!m_phasevalid)
			return;
		for(int i=0; i<listeners.Count; ++i)
		{
			switch(m_phase)
			{
				case TouchPhase.Began:
					m_startTouch = pos();
					listeners[i].onTouchBegan(this);
				break;
				case TouchPhase.Moved:
					listeners[i].onTouchMoved(this);
					m_startTouch = pos();
				break;
				case TouchPhase.Ended:
					listeners[i].onTouchEnded(this);
					m_startTouch = pos();
					m_phasevalid = false;
				break;
			}
		}
	}
}
