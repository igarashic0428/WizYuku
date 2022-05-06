using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//Slider

using Assets.Scripts.Audio;

[RequireComponent(typeof(Slider))]

public class SeSlider : MonoBehaviour
{
    Slider Slider;              //Slider格納用
    AudioSource AudioSource;    //AudioSource格納用

    float volumeOld;

    void Start()
    {
        Slider = this.GetComponent<Slider>();
        AudioSource = GameObject.Find("AudioManager/SE").GetComponent<AudioSource>();
        Slider.value = AudioSource.volume;
        volumeOld = AudioSource.volume;
    }

    void Update()
    {
        //音量をスライドバーの値に変更
        AudioSource.volume = Slider.value;

        if ((Input.GetMouseButtonUp(0)) &&
            (AudioSource.volume != volumeOld))
        {
            //SEを鳴らす
            AudioManager.instance.PlaySE((int)AudioManager.SeNum.Goal);

            volumeOld = AudioSource.volume;
        }
    }

}
