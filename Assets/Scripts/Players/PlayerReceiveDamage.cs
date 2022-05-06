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

        private int damage; //�_���[�W

        /*==============================================================*/
        /*====���J�֐�==========================================*/
        /*==============================================================*/
        //��U�������Ԃ����\�b�h
        public bool GetIsReceiveDamage()
        {
            return isReceiveDamage;
        }
        public Vector3 GetEnemyAttackPos()
        {
            return enemyAttackPos;
        }

        //�_���[�W��Ԃ����\�b�h
        public int GetDamege()
        {
            return damage;
        }

        /*=======================================================================*/
        /*====Unity���璼�ڌĂ΂��֐�==========================================*/
        /*=======================================================================*/

        private void OnTriggerEnter(Collider collision)
        {
            Debug.Log("OnTriggerEnter���R�[������܂����B");
            if (collision.tag == TagEnemyAttack)
            {
                Debug.Log(collision.tag + "�ɓ�����܂����B");
                isReceiveDamageEnter = true;
                enemyAttackPos = collision.gameObject.transform.position;

                //�Փ˂���EnemyAttack�̍U���͂��擾
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