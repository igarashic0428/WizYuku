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
        //HpBarFrontImage��Image�擾
        HpBarFrontImage = this.transform.Find("HpBarFrontImage").GetComponent<Image>();

        //Enemy01Controller�擾
        Enemy01Controller = this.transform.parent.gameObject.GetComponent<Enemy01Controller>();
    }

    // Update is called once per frame
    void Update()
    {
        if (0 < (float)Enemy01Controller.GetHp()) //�v���C���[��Hp��0��葽���Ȃ��
        {
            HpBarFrontImage.fillAmount = ((float)Enemy01Controller.GetHp()) / 100.0f; //Hp�̒l�����A�}�E���g����(�h��ׂ�)
        }
        else //0�ȉ��Ȃ��
        {
            HpBarFrontImage.fillAmount = 0f; //0�ɃA�}�E���g����(�S���h��ׂ��Ȃ�)
        }
    }
}
