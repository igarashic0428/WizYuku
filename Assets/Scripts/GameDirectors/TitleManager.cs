using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Assets.Scripts.Audio;

public class TitleManager : MonoBehaviour
{
    private FadeImageController FadeImageController;//[Header("フェード")] 
    private bool isFirstPushed = false; //1回しか処理しない用
    private bool isGoNextScene = false; //1回しか処理しない用

    [SerializeField] GameObject OptionCanvas;
    [SerializeField] GameObject InstructionCanvas;
    [SerializeField] GameObject CreditCanvas;

    // Start is called before the first frame update
    void Start()
    {
        FadeImageController = GameObject.Find("FadeImage").GetComponent<FadeImageController>();
    }

    /// <summary>
    /// StartButtonのinspectorのOnClickに割り当てること
    /// </summary>
    public void PresssStart()
    {
        Debug.Log("Press Start!");
        if (isFirstPushed == false) //1回しか処理しない
        {
            Debug.Log("Go Next Scene!");

            FadeImageController.StartFadeOut(); //フェードアウト開始

            isFirstPushed = true;

            //ボタンのSEを鳴らす
            AudioManager.instance.PlaySE((int)AudioManager.SeNum.Button);
        }
    }

    /// <summary>
    /// InfomationButtonのinspectorのOnClickに割り当てること
    /// </summary>
    public void PresssInfomation()
    {
        InstructionCanvas.SetActive(true);

        //ボタンのSEを鳴らす
        AudioManager.instance.PlaySE((int)AudioManager.SeNum.Button);
    }

    /// <summary>
    /// OptionButtonのinspectorのOnClickに割り当てること
    /// </summary>
    public void PresssOption()
    {
        OptionCanvas.SetActive(true);

        //ボタンのSEを鳴らす
        AudioManager.instance.PlaySE((int)AudioManager.SeNum.Button);

        //BGMを再生する
        AudioManager.instance.PlayBGM((int)AudioManager.BgmNum.Stage1);
    }

    /// <summary>
    /// CreditButtonのinspectorのOnClickに割り当てること
    /// </summary>
    public void PresssCredit()
    {
        CreditCanvas.SetActive(true);

        //ボタンのSEを鳴らす
        AudioManager.instance.PlaySE((int)AudioManager.SeNum.Button);
    }

    /// <summary>
    /// OkButtonのinspectorのOnClickに割り当てること
    /// </summary>
    public void PresssOk()
    {
        OptionCanvas.SetActive(false);
        InstructionCanvas.SetActive(false);
        CreditCanvas.SetActive(false);

        //ボタンのSEを鳴らす
        AudioManager.instance.PlaySE((int)AudioManager.SeNum.Button);

        //BGMを止める
        AudioManager.instance.StopBGM();
    }

    private void Update()
    {
        //フェードアウト完了していたら
        if ((isGoNextScene == false) && (FadeImageController.IsFadeOutComplete() == true))
        {
            //次のシーンへ行く
            SceneManager.LoadScene("Stage1");
            isGoNextScene = true;
        }
    }

}
