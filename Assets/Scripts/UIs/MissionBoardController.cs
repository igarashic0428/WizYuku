using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class MissionBoardController : MonoBehaviour
{
    //インスペクターで"Text"を設定しておくこと
    [Header("MissionText")] public GameObject Text;

    /*==============================================================*/
    /*====公開関数==================================================*/
    /*==============================================================*/
    public void DrawMission(int Nokori)
    {
        string strMission = null;
        if (Nokori > 0)
        {
            strMission =
                "ガイコツを倒せ\n" +
                "残り：" + Nokori + "体";
        }
        else
        {
            strMission = "光る地面へ\n向かえ";
        }

        //テキストの内容を変更する
        Text.GetComponent<Text>().text = strMission;
    }
}
