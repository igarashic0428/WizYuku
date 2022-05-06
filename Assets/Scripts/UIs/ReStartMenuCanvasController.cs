using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using UnityEngine.UI;
using Assets.Scripts.Audio;

public class ReStartMenuCanvasController : MonoBehaviour
{
    private FadeImageController FadeImageController;//[Header("フェード")] 
    private bool isFirstPushed = false; //1回しか処理しない用
    private bool isGoTitleScene = false; //1回しか処理しない用

    //Start時に非アクティブにしておくオブジェトについて
    //非アクティブなオブジェクトは捕まえられないのでインスペクターで設定する
    [Header("ReStartMenuCanvasのText")] public GameObject Text; //インスペクターで"Text"を設定しておくこと
    [Header("ReStartMenuCanvasのTextのMenu")] public GameObject Menu; //インスペクターで"Menu"を設定しておくこと

    /*==============================================================*/
    /*====公開関数==================================================*/
    /*==============================================================*/
    /// <summary>
    /// ゴールメニューを表示する
    /// </summary>
    public void DrawGoalMenu()
    {
        //テキストの内容を変更する
        Text.GetComponent<Text>().text = "ゴール！";
        //テキストをオンにして非表示→表示にする
        Text.SetActive(true);
        //ボタンをアクティブにする
        Menu.SetActive(true);
    }

    /// <summary>
    /// ゲームオーバーメニューを表示する
    /// </summary>
    public void DrawGameOverMenu()
    {
        //テキストの内容を変更する
        Text.GetComponent<Text>().text = "ゲームオーバー・・";
        //テキストをオンにして非表示→表示にする
        Text.SetActive(true);

        //ボタンをアクティブにする
        Menu.SetActive(true);

        //ImageFilterを黒にする
        GameObject ImageFilter = GameObject.Find("ReStartMenuCanvas/Menu/ImageFilter");
        ImageFilter.GetComponent<Image>().color = new Color(0, 0, 0, 0.5f);//半透明の黒
    }

    /*=======================================================================*/
    /*====Unityから直接呼ばれる関数==========================================*/
    /*=======================================================================*/
    // Start is called before the first frame update
    void Start()
    {
        FadeImageController = GameObject.Find("FadeImage").GetComponent<FadeImageController>();
    }

    //ボタンクリック時に呼ばれる関数
    //"ReStartMenuCanvas/Menu/Bottom"オブジェクトの「On Click ()」にこの関数を設定する事
    public void PressReStart() 
    {
        Debug.Log("Press ReStart!");
        if (isFirstPushed == false) //1回しか処理しない
        {

            Debug.Log("Go ReStart!");

            FadeImageController.StartFadeOut(); //フェードアウト開始

            isFirstPushed = true;

            //ボタンのSEを鳴らす
            AudioManager.instance.PlaySE((int)AudioManager.SeNum.Button);
        }
    }

    private void Update()
    {
        //フェードアウト完了していたら==============================================
        if ((isGoTitleScene == false) && (FadeImageController.IsFadeOutComplete() == true))
        {
            //タイトルシーンへ行く
            SceneManager.LoadScene("TitleScene");

            isGoTitleScene = true;

            //BGMを止める
            AudioManager.instance.StopBGM();
        }
    }

}
