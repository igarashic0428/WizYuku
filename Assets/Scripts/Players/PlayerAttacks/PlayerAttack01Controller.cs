namespace Assets.Scripts.Players.PlayerAttacks
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    using Assets.Scripts.Audio;

    public class PlayerAttack01Controller : MonoBehaviour
    {
        GameObject targetObj;

        private Rigidbody rb;

        Vector3 moveNVec; //�U���ڕW�ւ̐��K�x�N�g��

        private int power; //�U����

        /*==============================================================*/
        /*====�Z�b�g�p====================================================*/
        //�ʒu�Z�b�g
        public void SetPos(Vector3 Pos)
        {
            this.transform.position = Pos;
        }

        //�U���̓Z�b�g
        public void SetPower(int setPower)
        {
            power = setPower;
        }

        //�U���ڕW�̃I�u�W�F�N�g���Z�b�g
        public void SetTargetObj(GameObject Obj)
        {
            if (Obj!=null) {
                targetObj = Obj;

                //�U���ڕW�ւ̐��K�x�N�g�����Z�o
                Vector3 v = targetObj.transform.position;
                v.y += 3f; //�f�t�H���g���Ƒ����Ȃ̂ō�����␳����
                moveNVec = (v - this.transform.position).normalized;
            }
        }

        //�^�O���Z�b�g
        public void SetTag(string TagName)
        {
            this.gameObject.tag = TagName;
        }

        /*==============================================================*/
        /*====�擾�p====================================================*/
        //�U���͎擾
        public int GetPower()
        {
            return power;
        }

        /*=======================================================================*/
        /*====Unity���璼�ڌĂ΂��֐�==========================================*/
        /*=======================================================================*/

        // Start is called before the first frame update
        void Start()
        {
            rb = GetComponent<Rigidbody>();
        }

        //Update is called once per frame
        void Update()
        {
            //StageOutChack
            //�X�e�[�W�O�ɏo�Ă������玩�g��j������
            if ((this.transform.position.x < -40f) ||
                (this.transform.position.x > 200f) ||
                (this.transform.position.y < -20f) ||
                (this.transform.position.y > 20f) ||
                (this.transform.position.z < -20f) ||
                (this.transform.position.z > 20f))//�X�e�[�W�O�Ȃ��
            {
                //���g��j������
                Destroy(this.gameObject);
            }
        }

        void FixedUpdate()
        {
            rb.velocity = moveNVec * 15f;
        }

        private void OnTriggerEnter(Collider collision)
        {
            //Debug.Log("OnTriggerEnter���R�[������܂����B");

            //�����G�t�F�N�g�𐶐�����
            //�t�@�C�A�{�[���𐶐�
            GameObject ExplosionPrefab = (GameObject)Resources.Load("ParticleExplosion"); //�v���n�u���擾 ���K��Resources�t�H���_�Ɋi�[���邱��
            GameObject Explosion = Instantiate(ExplosionPrefab); //�v���n�u����I�u�W�F�N�g��������
            Explosion.transform.position = this.transform.position;
            Explosion.tag = this.tag;

            //������SE��炷
            AudioManager.instance.PlaySE((int)AudioManager.SeNum.Explosion);

            //�Փˎ���Enemy01ReceiveDamage�����̃N���X��GetPower���R�[������ׁA1�t���[���҂��Ă��玩�g��j������
            StartCoroutine(OneFrameLateDestroy()); //�R���[�`���̊֐����R�[������
        }

        //�R���[�`���̊֐���`
        IEnumerator OneFrameLateDestroy()
        {
            //1�t���[��������҂��܂��B
            yield return null;

            //���g��j������
            Destroy(this.gameObject);
        }
    }
}