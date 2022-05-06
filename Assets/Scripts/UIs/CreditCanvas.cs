using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditCanvas : MonoBehaviour
{
    [SerializeField] GameObject TextChapter;
    [SerializeField] GameObject TextCredit;

    // Start is called before the first frame update
    void Start()
    {
        TextChapter.GetComponent<Text>().text = "�N���W�b�g";

        string str =
            "�g�p�f��\n"+
            "\n"+
            "  ���ʉ��ABGM�@�F�@������\n" +
            "\n" +
            "  �K�C�R�c�A��󓙂̃C���X�g�@�F���炷�Ƃ�\n" +
            "\n" +
            "  �t�H���g�@�F�@JK�ۃS�V�b�N\n";
        TextCredit.GetComponent<Text>().text = str;
    }
}
