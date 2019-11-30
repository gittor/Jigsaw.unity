using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 滑动条控制类
/// </summary>
public class LabelSlider : MonoBehaviour {

    public Text txt;
    public Slider slider;
    void Start() {
        this.slider.onValueChanged.AddListener(delegate { OnValueChanged(); });
        OnValueChanged();
    }
    public void OnValueChanged()
    {
        txt.text = this.slider.value.ToString();
    }
}
