using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;
using Assets.Scripts.Audio;

public class ReStartMenuCanvasController : MonoBehaviour
{
    private FadeImageController FadeImageController;//[Header("�t�F�[�h")] 
    private bool isFirstPushed = false; //1�񂵂��������Ȃ��p
    private bool isGoTitleScene = false; //1�񂵂��������Ȃ��p

    //Start���ɔ�A�N�e�B�u�ɂ��Ă����I�u�W�F�g�ɂ���
    //��A�N�e�B�u�ȃI�u�W�F�N�g�͕߂܂����Ȃ��̂ŃC���X�y�N�^�[�Őݒ肷��
    [Header("ReStartMenuCanvas��Text")] public GameObject Text; //�C���X�y�N�^�[��"Text"��ݒ肵�Ă�������
    [Header("ReStartMenuCanvas��Text��Menu")] public GameObject Menu; //�C���X�y�N�^�[��"Menu"��ݒ肵�Ă�������

    /*==============================================================*/
    /*====���J�֐�==================================================*/
    /*==============================================================*/
    /// <summary>
    /// �S�[�����j���[��\������
    /// </summary>
    public void DrawGoalMenu()
    {
        //�e�L�X�g�̓��e��ύX����
        Text.GetComponent<Text>().text = "�S�[���I";
        //�e�L�X�g���I���ɂ��Ĕ�\�����\���ɂ���
        Text.SetActive(true);
        //�{�^�����A�N�e�B�u�ɂ���
        Menu.SetActive(true);
    }

    /// <summary>
    /// �Q�[���I�[�o�[���j���[��\������
    /// </summary>
    public void DrawGameOverMenu()
    {
        //�e�L�X�g�̓��e��ύX����
        Text.GetComponent<Text>().text = "�Q�[���I�[�o�[�E�E";
        //�e�L�X�g���I���ɂ��Ĕ�\�����\���ɂ���
        Text.SetActive(true);

        //�{�^�����A�N�e�B�u�ɂ���
        Menu.SetActive(true);

        //ImageFilter�����ɂ���
        GameObject ImageFilter = GameObject.Find("ReStartMenuCanvas/Menu/ImageFilter");
        ImageFilter.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);//�������̍�
    }

    /*=======================================================================*/
    /*====Unity���璼�ڌĂ΂��֐�==========================================*/
    /*=======================================================================*/
    // Start is called before the first frame update
    void Start()
    {
        FadeImageController = GameObject.Find("FadeImage").GetComponent<FadeImageController>();
    }

    //�{�^���N���b�N���ɌĂ΂��֐�
    //"ReStartMenuCanvas/Menu/Bottom"�I�u�W�F�N�g�́uOn Click ()�v�ɂ��̊֐���ݒ肷�鎖
    public void PressReStart() 
    {
        Debug.Log("Press ReStart!");
        if (isFirstPushed == false) //1�񂵂��������Ȃ�
        {

            Debug.Log("Go ReStart!");

            FadeImageController.StartFadeOut(); //�t�F�[�h�A�E�g�J�n

            isFirstPushed = true;

            //�{�^����SE��炷
            AudioManager.instance.PlaySE((int)AudioManager.SeNum.Button);
        }
    }

    private void Update()
    {
        //�t�F�[�h�A�E�g�������Ă�����==============================================
        if ((isGoTitleScene == false) && (FadeImageController.IsFadeOutComplete() == true))
        {
            //�^�C�g���V�[���֍s��
            SceneManager.LoadScene("TitleScene");

            isGoTitleScene = true;

            //BGM���~�߂�
            AudioManager.instance.StopBGM();
        }
    }

}
