namespace Assets.Scripts.Enemy01s
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    using Assets.Scripts.Players.PlayerAttacks;

    public class Enemy01ReceiveDamage : MonoBehaviour
    {
        private string TagPlayerAttack = "TagPlayerAttack";
        private bool isReceiveDamage = false;
        private bool isReceiveDamageEnter;
        //private Vector3 PlayerAttackPos;

        private int Damage; //ダメージ

        /*==============================================================*/
        /*====公開関数==========================================*/
        /*==============================================================*/
        //被攻撃判定を返すメソッド
        public bool GetIsReceiveDamage()
        {
            return isReceiveDamage;
        }

        //ダメージを返すメソッド
        public int GetDamege()
        {
            return Damage;
        }

        /*=======================================================================*/
        /*====Unityから直接呼ばれる関数==========================================*/
        /*=======================================================================*/

        private void OnTriggerEnter(Collider collision)
        {
            Debug.Log("OnTriggerEnterがコールされました。");
            if (collision.tag == TagPlayerAttack)//衝突したオブジェクトがPlayerAttackならば
            {
                Debug.Log(collision.tag + "に当たりました。");
                isReceiveDamageEnter = true;

                //衝突したPlayerAttackの攻撃力を取得
                PlayerAttack01Controller PlayerAttack01Controller = collision.gameObject.GetComponent<PlayerAttack01Controller>();
                if(PlayerAttack01Controller != null)
                {
                    Damage = PlayerAttack01Controller.GetPower();
                }
            }
        }

        void FixedUpdate()
        {
            if (isReceiveDamageEnter)
            {
                isReceiveDamage = true;
            }
            else
            {
                isReceiveDamage = false;
            }

            isReceiveDamageEnter = false;
        }

    }
}