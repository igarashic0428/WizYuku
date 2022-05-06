using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Audio;

public class InstructionCanvas : MonoBehaviour
{
    [SerializeField] GameObject TextChapter;
    [SerializeField] GameObject PrevButton;
    [SerializeField] GameObject NextButton;
    [SerializeField] GameObject Instruction1;
    [SerializeField] GameObject Instruction2;
    [SerializeField] GameObject Instruction3;

    int page;

    // Start is called before the first frame update
    void OnEnable()
    {
        Init();
    }

    void Init()
    {
        TextChapter.GetComponent<Text>().text = "‘€ìà–¾‡@";
        PrevButton.SetActive(false);
        NextButton.SetActive(true);
        Instruction1.SetActive(true);
        Instruction2.SetActive(false);
        Instruction3.SetActive(false);


        page = 1;
    }


    /// <summary>
    /// PrevButton‚Ìinspector‚ÌOnClick‚ÉŠ„‚è“–‚Ä‚é‚±‚Æ
    /// </summary>
    public void PresssPrev()
    {
        page--;
        PageChenge();
    }


    /// <summary>
    /// NextButton‚Ìinspector‚ÌOnClick‚ÉŠ„‚è“–‚Ä‚é‚±‚Æ
    /// </summary>
    public void PresssNext()
    {
        page++;
        PageChenge();
    }

    void PageChenge()
    {
        switch (page)
        {
            case 1:
                Init();
                break;
            case 2:
                TextChapter.GetComponent<Text>().text = "‘€ìà–¾‡A";
                PrevButton.SetActive(true);
                NextButton.SetActive(true);
                Instruction1.SetActive(false);
                Instruction2.SetActive(true);
                Instruction3.SetActive(false);

                break;
            case 3:
                TextChapter.GetComponent<Text>().text = "‘€ìà–¾‡B";
                PrevButton.SetActive(true);
                NextButton.SetActive(false);
                Instruction1.SetActive(false);
                Instruction2.SetActive(false);
                Instruction3.SetActive(true);

                break;
            default:
                Init();
                break;
        }

        //ƒ{ƒ^ƒ“‚ÌSE‚ğ–Â‚ç‚·
        AudioManager.instance.PlaySE((int)AudioManager.SeNum.Button);
    }

}
