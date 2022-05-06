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
        TextChapter.GetComponent<Text>().text = "クレジット";

        string str =
            "使用素材\n"+
            "\n"+
            "  効果音、BGM　：　魔王魂\n" +
            "\n" +
            "  ガイコツ、矢印等のイラスト　：いらすとや\n" +
            "\n" +
            "  フォント　：　JK丸ゴシック\n";
        TextCredit.GetComponent<Text>().text = str;
    }
}
