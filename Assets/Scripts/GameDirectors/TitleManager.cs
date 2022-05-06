using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Assets.Scripts.Audio;

public class TitleManager : MonoBehaviour
{
    private FadeImageController FadeImageController;//[Header("�t�F�[�h")] 
    private bool isFirstPushed = false; //1�񂵂��������Ȃ��p
    private bool isGoNextScene = false; //1�񂵂��������Ȃ��p

    [SerializeField] GameObject OptionCanvas;
    [SerializeField] GameObject InstructionCanvas;
    [SerializeField] GameObject CreditCanvas;

    // Start is called before the first frame update
    void Start()
    {
        FadeImageController = GameObject.Find("FadeImage").GetComponent<FadeImageController>();
    }

    /// <summary>
    /// StartButton��inspector��OnClick�Ɋ��蓖�Ă邱��
    /// </summary>
    public void PresssStart()
    {
        Debug.Log("Press Start!");
        if (isFirstPushed == false) //1�񂵂��������Ȃ�
        {
            Debug.Log("Go Next Scene!");

            FadeImageController.StartFadeOut(); //�t�F�[�h�A�E�g�J�n

            isFirstPushed = true;

            //�{�^����SE��炷
            AudioManager.instance.PlaySE((int)AudioManager.SeNum.Button);
        }
    }

    /// <summary>
    /// InfomationButton��inspector��OnClick�Ɋ��蓖�Ă邱��
    /// </summary>
    public void PresssInfomation()
    {
        InstructionCanvas.SetActive(true);

        //�{�^����SE��炷
        AudioManager.instance.PlaySE((int)AudioManager.SeNum.Button);
    }

    /// <summary>
    /// OptionButton��inspector��OnClick�Ɋ��蓖�Ă邱��
    /// </summary>
    public void PresssOption()
    {
        OptionCanvas.SetActive(true);

        //�{�^����SE��炷
        AudioManager.instance.PlaySE((int)AudioManager.SeNum.Button);

        //BGM���Đ�����
        AudioManager.instance.PlayBGM((int)AudioManager.BgmNum.Stage1);
    }

    /// <summary>
    /// CreditButton��inspector��OnClick�Ɋ��蓖�Ă邱��
    /// </summary>
    public void PresssCredit()
    {
        CreditCanvas.SetActive(true);

        //�{�^����SE��炷
        AudioManager.instance.PlaySE((int)AudioManager.SeNum.Button);
    }

    /// <summary>
    /// OkButton��inspector��OnClick�Ɋ��蓖�Ă邱��
    /// </summary>
    public void PresssOk()
    {
        OptionCanvas.SetActive(false);
        InstructionCanvas.SetActive(false);
        CreditCanvas.SetActive(false);

        //�{�^����SE��炷
        AudioManager.instance.PlaySE((int)AudioManager.SeNum.Button);

        //BGM���~�߂�
        AudioManager.instance.StopBGM();
    }

    private void Update()
    {
        //�t�F�[�h�A�E�g�������Ă�����
        if ((isGoNextScene == false) && (FadeImageController.IsFadeOutComplete() == true))
        {
            //���̃V�[���֍s��
            SceneManager.LoadScene("Stage1");
            isGoNextScene = true;
        }
    }

}
