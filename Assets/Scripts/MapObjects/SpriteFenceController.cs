using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Players;

public class SpriteFenceController : MonoBehaviour
{
    //Player取得用
    PlayerController PlayerController;

    //自身のSpriteRenderer取得用
    SpriteRenderer spriteRend;

    // Start is called before the first frame update
    void Start()
    {
        //Player取得
        PlayerController = GameObject.Find("Player").GetComponent<PlayerController>();

        //自身のSpriteRenderer取得
        spriteRend = this.GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {
        if (((this.transform.position.x - 2.5f) < PlayerController.GetPos().x) &&
            (PlayerController.GetPos().x < (this.transform.position.x + 2.5f)) && //x軸でプレイヤーがフェンスと重なっている
            (this.transform.position.z < PlayerController.GetPos().z) &&
            (PlayerController.GetPos().z < (this.transform.position.z + 10f))) //プレイヤーがフェンスより後ろにいる
        {
            Debug.Log("フェンスを半透明にします");
            //フェンスを半透明にする
            Color32 t_colr = spriteRend.color;
            spriteRend.color = new Color32(t_colr.r, t_colr.g, t_colr.b, 200);
        }
        else
        {
            //フェンスを半透明にしない
            Color32 t_colr = spriteRend.color;
            spriteRend.color = new Color32(t_colr.r, t_colr.g, t_colr.b, 255);
        }
    }
}
