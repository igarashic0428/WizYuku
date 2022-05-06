using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;//Slider

using Assets.Scripts.Audio;

[RequireComponent(typeof(Slider))]

public class SeSlider : MonoBehaviour
{
    Slider Slider;              //Slider�i�[�p
    AudioSource AudioSource;    //AudioSource�i�[�p

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
        //���ʂ��X���C�h�o�[�̒l�ɕύX
        AudioSource.volume = Slider.value;

        if ((Input.GetMouseButtonUp(0)) &&
            (AudioSource.volume != volumeOld))
        {
            //SE��炷
            AudioManager.instance.PlaySE((int)AudioManager.SeNum.Goal);

            volumeOld = AudioSource.volume;
        }
    }

}
