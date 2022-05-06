using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//Slider

[RequireComponent(typeof(Slider))]

public class BgmSlider : MonoBehaviour
{
    Slider Slider;              //Slider�i�[�p
    AudioSource AudioSource;    //AudioSource�i�[�p

    void Start()
    {
        Slider = this.GetComponent<Slider>();
        AudioSource = GameObject.Find("AudioManager/BGM").GetComponent<AudioSource>();
        Slider.value = AudioSource.volume;
    }

    void Update()
    {
        //���ʂ��X���C�h�o�[�̒l�ɕύX
        AudioSource.volume = Slider.value;
    }

}
