using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class TimerBoardController : MonoBehaviour
{
    float startTime;
    int   secTimer; //�^�C�}�[(�b)
    bool  isTimerStop;

    //�C���X�y�N�^�[��"Text"��ݒ肵�Ă�������
    [Header("TimeText")] public GameObject Text;


    /*==============================================================*/
    /*====���J�֐�==================================================*/
    /*==============================================================*/
    /// <summary>
    /// �^�C�}�[���~�߂�
    /// </summary>
    public void SetStop()
    {
        isTimerStop = true;
    }

    /*=======================================================================*/
    /*====Unity���璼�ڌĂ΂��֐�==========================================*/
    /*=======================================================================*/
    // Start is called before the first frame update
    void Start()
    {
        secTimer = 0;
        startTime = Time.time;
    }

    // Update is called once per frame
    void Update()
    {
        if (isTimerStop == false) {
            secTimer = (int)(Time.time - startTime);
            DrawTimer(secTimer);
        }
    }

    /*==============================================================*/
    /*====�����֐�==================================================*/
    /*==============================================================*/
    void DrawTimer(int t)
    {
        int Minutes = (int)(t / 60f);
        int Seconds = (int)(t % 60f);
        //�e�L�X�g�̓��e��ύX����
        if (Seconds<10) {
            Text.GetComponent<Text>().text = Minutes + ":0" + Seconds;
        }
        else
        {
            Text.GetComponent<Text>().text = Minutes + ":" + Seconds;
        }
    }
}
