using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeImageController : MonoBehaviour
{
    [Header("最初からフェードインが完了しているかどうか")] public bool isFirstFadeInComp;

    private Image fadeImg = null;

    //フェードイン関連
    private int frameCount;
    private float fadeTimer;
    private bool isFadeIn;
    private bool isCompFadeIn = false;

    //フェードアウト関連
    private bool isFadeOut;
    private bool isCompFadeOut = false;

    /// <summary>
    /// フェードインを開始する
    /// </summary>
    public void StartFadeIn()
    {
        if (isFadeIn || isFadeOut)
        {
            return; //フェード中は新たにフェードを開始しない
        }

        //フェードインを開始する場合の初期値設定
        isFadeIn = true;
        isCompFadeIn = false;
        fadeTimer = 0.0f;

        fadeImg.color = new Color(1, 1, 1, 1);
        fadeImg.fillAmount = 1;
        fadeImg.raycastTarget = true; //フェード画像の当たり判定オンにすることでボタン等を押せないようにする
    }

    /// <summary>
    /// フェードインが完了したかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsFadeInComplete()
    {
        return isCompFadeIn;
    }

    /// <summary>
    /// フェードアウトを開始する
    /// </summary>
    public void StartFadeOut()
    {
        if (isFadeIn || isFadeOut)
        {
            return; //フェード中は新たにフェードを開始しない
        }

        //フェードアウト開始する場合の初期値設定
        isFadeOut = true;
        isCompFadeOut = false;
        fadeTimer = 0.0f;

        fadeImg.color = new Color(1, 1, 1, 0);
        fadeImg.fillAmount = 0;
        fadeImg.raycastTarget = true; //フェード画像の当たり判定オンにすることでボタン等を押せないようにする
    }

    /// <summary>
    /// フェードアウトが完了したかどうか
    /// </summary>
    /// <returns></returns>
    public bool IsFadeOutComplete()
    {
        return isCompFadeOut;
    }


    // Start is called before the first frame update
    void Start()
    {
        fadeImg = GetComponent<Image>();

        if (isFirstFadeInComp == true)
        {
            FadeInComplete();
        }
        else
        {
            StartFadeIn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (frameCount >2) //シーン開始時、読み込みのラグ対策に2フレーム待つ
        {
            if (isFadeIn) //フェードインする
            {
                FadeInUpdate();
            }
            else if (isFadeOut)
            {
                FadeOutUpdate();
            }
        }
        frameCount++;
    }

    //フェードイン中の更新
    private void FadeInUpdate()
    {
        if (fadeTimer < 1f) //シーン開始時に1秒欠けてフェードインする
        {
            fadeImg.color = new Color(1, 1, 1, 1 - fadeTimer);//乗算：1秒かけて透明になる
            fadeImg.fillAmount = 1f - fadeTimer; //1秒かけて額を塗りつぶす
        }
        else
        {
            FadeInComplete();
        }
        fadeTimer += Time.deltaTime;
    }

    //フェードイン完了処理
    private void FadeInComplete()
    {
        //フェードイン完了時に各種パラメータをセット
        fadeImg.color = new Color(1, 1, 1, 0);//乗算
        fadeImg.fillAmount = 0; //額は塗りつぶさない
        fadeImg.raycastTarget = false; //Imageの当たり判定をオフ

        fadeTimer = 0.0f;
        isFadeIn = false;
        isCompFadeIn = true;
    }

    //フェードアウト中の更新
    private void FadeOutUpdate()
    {
        if (fadeTimer < 1f) //シーン開始時に1秒欠けてフェードアウトする
        {
            fadeImg.color = new Color(1, 1, 1, fadeTimer);//色を乗算：1秒かけて不透明になる
            fadeImg.fillAmount = fadeTimer; //1秒かけて額を剥がす
        }
        else
        {
            FadeOutComplete();
        }
        fadeTimer += Time.deltaTime;
    }

    //フェードアウト完了処理
    private void FadeOutComplete()
    {
        //フェードアウト完了時に各種パラメータをセット
        fadeImg.color = new Color(1, 1, 1, 1);//色を乗算
        fadeImg.fillAmount = 1; //額を塗りつぶす
        fadeImg.raycastTarget = true; //フェード画像の当たり判定オンにすることでボタン等を押せないようにする

        fadeTimer = 0.0f;
        isFadeOut = false;
        isCompFadeOut = true;
    }


}
