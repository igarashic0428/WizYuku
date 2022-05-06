using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class TimerBoardController : MonoBehaviour
{
    float startTime;
    int   secTimer; //タイマー(秒)
    bool  isTimerStop;

    //インスペクターで"Text"を設定しておくこと
    [Header("TimeText")] public GameObject Text;


    /*==============================================================*/
    /*====公開関数==================================================*/
    /*==============================================================*/
    /// <summary>
    /// タイマーを止める
    /// </summary>
    public void SetStop()
    {
        isTimerStop = true;
    }

    /*=======================================================================*/
    /*====Unityから直接呼ばれる関数==========================================*/
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
    /*====内部関数==================================================*/
    /*==============================================================*/
    void DrawTimer(int t)
    {
        int Minutes = (int)(t / 60f);
        int Seconds = (int)(t % 60f);
        //テキストの内容を変更する
        if (Seconds<10) {
            Text.GetComponent<Text>().text = Minutes + ":0" + Seconds;
        }
        else
        {
            Text.GetComponent<Text>().text = Minutes + ":" + Seconds;
        }
    }
}
