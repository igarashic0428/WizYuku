using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Players;

public class PlayerHpBarController : MonoBehaviour
{
    Image HpBarFrontImage;
    PlayerController PlayerController;

    // Start is called before the first frame update
    void Start()
    {
        //HpBarFrontImageのImage取得
        HpBarFrontImage = transform.Find("HpBarFrontImage").GetComponent<Image>();

        //PlayerController取得
        PlayerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (0 < (float)PlayerController.GetHp()) //プレイヤーのHpが0より多いならば
        {
            HpBarFrontImage.fillAmount = ((float)PlayerController.GetHp())/100.0f; //Hpの値分をアマウントする(塗り潰す)
        }
        else //0以下ならば
        {
            HpBarFrontImage.fillAmount = 0f; //0にアマウントする(全く塗り潰さない)
        }
    }
}
