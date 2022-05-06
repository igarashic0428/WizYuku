using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Players;

public class SpriteFenceController : MonoBehaviour
{
    //Player�擾�p
    PlayerController PlayerController;

    //���g��SpriteRenderer�擾�p
    SpriteRenderer spriteRend;

    // Start is called before the first frame update
    void Start()
    {
        //Player�擾
        PlayerController = GameObject.Find("Player").GetComponent<PlayerController>();

        //���g��SpriteRenderer�擾
        spriteRend = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (((this.transform.position.x - 2.5f) < PlayerController.GetPos().x) &&
            (PlayerController.GetPos().x < (this.transform.position.x + 2.5f)) && //x���Ńv���C���[���t�F���X�Əd�Ȃ��Ă���
            (this.transform.position.z < PlayerController.GetPos().z) &&
            (PlayerController.GetPos().z < (this.transform.position.z + 10f))) //�v���C���[���t�F���X�����ɂ���
        {
            Debug.Log("�t�F���X�𔼓����ɂ��܂�");
            //�t�F���X�𔼓����ɂ���
            Color32 t_colr = spriteRend.color;
            spriteRend.color = new Color32(t_colr.r, t_colr.g, t_colr.b, 200);
        }
        else
        {
            //�t�F���X�𔼓����ɂ��Ȃ�
            Color32 t_colr = spriteRend.color;
            spriteRend.color = new Color32(t_colr.r, t_colr.g, t_colr.b, 255);
        }
    }
}
