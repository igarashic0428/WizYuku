using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class MissionBoardController : MonoBehaviour
{
    //�C���X�y�N�^�[��"Text"��ݒ肵�Ă�������
    [Header("MissionText")] public GameObject Text;

    /*==============================================================*/
    /*====���J�֐�==================================================*/
    /*==============================================================*/
    public void DrawMission(int Nokori)
    {
        string strMission = null;
        if (Nokori > 0)
        {
            strMission =
                "�K�C�R�c��|��\n" +
                "�c��F" + Nokori + "��";
        }
        else
        {
            strMission = "����n�ʂ�\n������";
        }

        //�e�L�X�g�̓��e��ύX����
        Text.GetComponent<Text>().text = strMission;
    }
}
