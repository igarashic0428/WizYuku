namespace Assets.Scripts.Enemy01s.Enemy01Attacks
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Enemy01Attack01Controller : MonoBehaviour
    {
        float ATTACK_TIME = 0.25f; //�U������(�b)
        float startTime; //�J�n����

        private int power; //�U����

        /*======================================================*/
        /*====���J�֐�==========================================*/
        /*======================================================*/
        //�U���̓Z�b�g
        public void SetPower(int setPower)
        {
            power = setPower;
        }

        //�^�O���Z�b�g
        public void SetTag(string tagName)
        {
            this.gameObject.tag = tagName;
        }

        /*==============================================================*/
        /*====�擾�p====================================================*/
        //�U���͎擾
        public int GetPower()
        {
            return power;
        }

        /*==============================================================*/
        /*====Unity���璼�ڌĂ΂��֐�=================================*/
        /*==============================================================*/
        // Start is called before the first frame update
        void Start()
        {
            startTime = Time.time; //�������J�n���Ԃ�ۑ�����
        }

        //�X�V���u
        void FixedUpdate()
        {
            //���莞�Ԍo������Ɏ�����j������
            float t = Time.time;
            if (ATTACK_TIME < (t - startTime))
            {
                //������j��
                Destroy(this.gameObject);
            }
        }

        /*======================================================*/
        /*====�����֐�==========================================*/
        /*======================================================*/

    }

}