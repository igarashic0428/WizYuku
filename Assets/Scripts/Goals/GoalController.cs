namespace Assets.Scripts.Goals
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    using Assets.Scripts.Players;

    public class GoalController : MonoBehaviour
    {
        //Player�̃C���X�^���X����
        PlayerController PlayerController;

        //���J�ϐ�
        public bool isGoal;

        /*==============================================================*/
        /*====���J�֐�==================================================*/
        /*==============================================================*/
        //�S�[����Ԃ��擾
        public bool GetIsGoal()
        {
            return isGoal;
        }

        //�S�[�����A�N�e�B�u�ɂ���
        public void SetInActiveGoal()
        {
            this.gameObject.SetActive(false);
        }

        //�S�[����Ԃ��擾
        public void SetActiveGoal()
        {
            this.gameObject.SetActive(true);
        }

        /*=======================================================================*/
        /*====Unity���璼�ڌĂ΂��֐�==========================================*/
        /*=======================================================================*/
        // Start is called before the first frame update
        void Start()
        {
            PlayerController = GameObject.Find("Player").GetComponent<PlayerController>();
        }

        // Update is called once per frame
        void Update()
        {
        }

        //�����蔻��֐�
        private void OnTriggerEnter(Collider other)
        {
            //�������Ă����I�u�W�F�N�g��Player�̂Ƃ�
            if (other.name == PlayerController.GetName())
            {
                //��U���O�ɏo��
                Debug.Log("Player�ƐڐG���܂����I");

                isGoal = true;
            }
        }
    }
}