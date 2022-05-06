namespace Assets.Scripts.Players
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    using Assets.Scripts.Enemy01s.Enemy01Attacks;

    public class PlayerReceiveDamage : MonoBehaviour
    {
        private string TagEnemyAttack = "TagEnemyAttack";
        private bool isReceiveDamage = false;
        private bool isReceiveDamageEnter;
        private Vector3 enemyAttackPos;

        private int damage; //ダメージ

        /*==============================================================*/
        /*====公開関数==========================================*/
        /*==============================================================*/
        //被攻撃判定を返すメソッド
        public bool GetIsReceiveDamage()
        {
            return isReceiveDamage;
        }
        public Vector3 GetEnemyAttackPos()
        {
            return enemyAttackPos;
        }

        //ダメージを返すメソッド
        public int GetDamege()
        {
            return damage;
        }

        /*=======================================================================*/
        /*====Unityから直接呼ばれる関数==========================================*/
        /*=======================================================================*/

        private void OnTriggerEnter(Collider collision)
        {
            Debug.Log("OnTriggerEnterがコールされました。");
            if (collision.tag == TagEnemyAttack)
            {
                Debug.Log(collision.tag + "に当たりました。");
                isReceiveDamageEnter = true;
                enemyAttackPos = collision.gameObject.transform.position;

                //衝突したEnemyAttackの攻撃力を取得
                Enemy01Attack01Controller Enemy01Attack01Controller = collision.gameObject.GetComponent<Enemy01Attack01Controller>();
                if (Enemy01Attack01Controller != null)
                {
                    damage = Enemy01Attack01Controller.GetPower();
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