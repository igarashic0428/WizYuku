using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//Slider

[RequireComponent(typeof(Slider))]

public class BgmSlider : MonoBehaviour
{
    Slider Slider;              //Slider格納用
    AudioSource AudioSource;    //AudioSource格納用

    void Start()
    {
        Slider = this.GetComponent<Slider>();
        AudioSource = GameObject.Find("AudioManager/BGM").GetComponent<AudioSource>();
        Slider.value = AudioSource.volume;
    }

    void Update()
    {
        //音量をスライドバーの値に変更
        AudioSource.volume = Slider.value;
    }

}
