using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

using Assets.Scripts.Enemy01s;


public class EnemyHpCanvasCntroller : MonoBehaviour
{
    Image HpBarFrontImage = null;
    Enemy01Controller Enemy01Controller = null;

    // Start is called before the first frame update
    void Start()
    {
        //HpBarFrontImageのImage取得
        HpBarFrontImage = this.transform.Find("HpBarFrontImage").GetComponent<Image>();

        //Enemy01Controller取得
        Enemy01Controller = this.transform.parent.gameObject.GetComponent<Enemy01Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        if (0 < (float)Enemy01Controller.GetHp()) //プレイヤーのHpが0より多いならば
        {
            HpBarFrontImage.fillAmount = ((float)Enemy01Controller.GetHp()) / 100.0f; //Hpの値分をアマウントする(塗り潰す)
        }
        else //0以下ならば
        {
            HpBarFrontImage.fillAmount = 0f; //0にアマウントする(全く塗り潰さない)
        }
    }
}
