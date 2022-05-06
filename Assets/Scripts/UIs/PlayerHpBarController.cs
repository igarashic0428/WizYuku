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
        //HpBarFrontImage��Image�擾
        HpBarFrontImage = transform.Find("HpBarFrontImage").GetComponent<Image>();

        //PlayerController�擾
        PlayerController = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        if (0 < (float)PlayerController.GetHp()) //�v���C���[��Hp��0��葽���Ȃ��
        {
            HpBarFrontImage.fillAmount = ((float)PlayerController.GetHp())/100.0f; //Hp�̒l�����A�}�E���g����(�h��ׂ�)
        }
        else //0�ȉ��Ȃ��
        {
            HpBarFrontImage.fillAmount = 0f; //0�ɃA�}�E���g����(�S���h��ׂ��Ȃ�)
        }
    }
}
