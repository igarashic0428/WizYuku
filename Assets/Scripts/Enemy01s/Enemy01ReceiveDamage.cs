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

        private int Damage; //�_���[�W

        /*==============================================================*/
        /*====���J�֐�==========================================*/
        /*==============================================================*/
        //��U�������Ԃ����\�b�h
        public bool GetIsReceiveDamage()
        {
            return isReceiveDamage;
        }

        //�_���[�W��Ԃ����\�b�h
        public int GetDamege()
        {
            return Damage;
        }

        /*=======================================================================*/
        /*====Unity���璼�ڌĂ΂��֐�==========================================*/
        /*=======================================================================*/

        private void OnTriggerEnter(Collider collision)
        {
            Debug.Log("OnTriggerEnter���R�[������܂����B");
            if (collision.tag == TagPlayerAttack)//�Փ˂����I�u�W�F�N�g��PlayerAttack�Ȃ��
            {
                Debug.Log(collision.tag + "�ɓ�����܂����B");
                isReceiveDamageEnter = true;

                //�Փ˂���PlayerAttack�̍U���͂��擾
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