
namespace Assets.Scripts.Inputs
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    using Assets.Scripts.Cameras;

    public class InputController : MonoBehaviour
    {
        //==============================================�萔��`==============================================
        const float TAP_CONFIRM_TIME = 0.3f;            //�^�b�v�m�莞�Ԃ�臒l
        const float TAP_LIMIT_POS_DIFF = 30.0f;         //�^�b�v���E���W����臒l
        const float FLICK_LIMIT_TIME = 0.5f;            //�v���b�N���E���Ԃ�臒l

        const float UP_FLICK_CONFIRM_ANGLE_MIN = 5.0f;  //��t���b�N�m��p�x

        //==============================================�O���N���X�Ăяo��==============================================

        //�N���X�̃C���X�^���X
        CameraController CameraController;

        //==============================================�ϐ���`==============================================
        public bool isTap; //�^�b�v���

        bool isFlick; //�t���b�N���
        public bool isUpFlick; //��t���b�N���

        bool isJudgePermit; //���苖��

        //�^�b�v�ƃt���b�N�̕��ʗp
        Vector2 tapDownPos2d = new Vector2(0, 0); //�^�b�v���WDown
        Vector2 tapNowPos2d = new Vector2(0, 0); //�^�b�v���WNow
        Vector2 tapUpPos2d = new Vector2(0, 0); //�^�b�v���WUp

        float tapDownTime; //�w����ʂɐG�ꂽ�u�Ԃ̎���
        float tapNowTime;  //���݂̎���
        float tapUpTime;   //�w����ʂ��痣�ꂽ�u�Ԃ̎���

        public float flickAngle; //�t���b�N�p�x

        //============================================================
        //�E�N���b�N�֘A==============================================
        bool isRightClick; //�E�N���b�N���

        //==============================================�֐���`==============================================

        /*==============================================================*/
        /*====Unity���璼�ڌĂ΂��֐�==========================================*/
        /*==============================================================*/

        // Start is called before the first frame update
        void Start()
        {
            //�N���X�̃C���X�^���X�ɃN���X���i�[
            CameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
        }

        /*==============================================================*/
        /*====���J�֐�==================================================*/
        /*==============================================================*/

        /*==============================================================*/
        /*====�擾�p====================================================*/

        //�^�b�v���ꂽ���Ƃ𔻒�
        public bool GetIsTap()
        {
            return isTap;
        }

        //3D��ԏ�̃^�b�v���W���擾
        public Vector3 GetTapPos3d()
        {
            Vector3 result;

            bool isHit = CameraController.IsRaycastHitVec2To3d(Input.mousePosition);
            if (isHit == true) //3D���W�ɕϊ��\�Ȃ��
            {
                result = CameraController.GetRayHitPosVec3();
            }
            else
            {
                result = new Vector3(0,0,0);
            }

            return result;
        }

        //�^�b�v���ꂽ�I�u�W�F�N�g�̃^�O���擾
        public string GetTapObjTag()
        {
            string result;

            bool isHit = CameraController.IsRaycastHitVec2To3d(Input.mousePosition);
            if (isHit == true) //3D���W�ɕϊ��\�Ȃ��
            {
                result = CameraController.GetRayHitObjTag();
            }
            else
            {
                result = null;
            }

            return result;
        }

        //�^�b�v���ꂽ�Q�[���I�u�W�F�N�g���擾
        public GameObject GetTapGameObject()
        {
            GameObject result;

            bool isHit = CameraController.IsRaycastHitVec2To3d(Input.mousePosition);
            if (isHit == true) //3D���W�ɕϊ��\�Ȃ��
            {
                result = CameraController.GetRayHitGameObject();
            }
            else
            {
                result = null;
            }

            return result;
        }

        //��t���b�N���ꂽ���Ƃ𔻒�
        public bool GetIsUpFlick()
        {
            return isUpFlick;
        }

        //�E�N���b�N��Ԃ��擾
        public bool GetInputisRightClick()
        {
            return isRightClick;
        }

        /*==============================================================*/
        /*====�X�V�p====================================================*/

        //�X�V����
        public void UpdateInput()
        {
            //===========================================
            //���N���b�N(�^�b�v)�Ə�t���b�N��2�p�^�[�����o�͂���
            //===========================================

            if (isTap) { isTap = false; } //true��1�����̂�
            if (isFlick) { isFlick = false; } //true��1�����̂�
            if (isUpFlick) { isUpFlick = false; } //true��1�����̂�

            if (Input.GetMouseButtonDown(0)) //�����ꂽ�u��
            {
                tapDownPos2d = Input.mousePosition;
                tapDownTime = Time.time;
                isJudgePermit = true;
            }

            if (isJudgePermit)
            {
                if (Input.GetMouseButton(0)) //������Ă����
                {
                    //�������ςȂ��̎� �D�揇��:(1)>(2)
                    //(1)�u�����Ă��Ȃ� ���� ��莞�Ԍo�߁v�Ȃ�^�b�v�m�肷��
                    //(2)�������Ȃ�

                    tapNowPos2d = Input.mousePosition;
                    tapNowTime = Time.time;

                    if ((Vector2.Distance(tapNowPos2d, tapDownPos2d) <= TAP_LIMIT_POS_DIFF) &&  //�^�b�v���E���W���ȓ� ����
                        (TAP_CONFIRM_TIME < (tapNowTime - tapDownTime))) //�^�b�v�m�莞�Ԃ��o��
                    {
                        //�^�b�v�m�肷��
                        isTap = true;
                        isFlick = false;
                        isJudgePermit = false;

                        Debug.Log("�^�b�v����܂���(�������ςȂ�)");

                    }
                }

                if (Input.GetMouseButtonUp(0)) //���ꂽ�u��
                {
                    //���ꂽ�u�Ԃ̎� �D�揇��:(1)>(2)>(3)
                    //(1)�u�����Ă��Ȃ� ���� ��莞�Ԉȓ��v�Ȃ�^�b�v�m�肷��
                    //(2)�u�����Ă��� ���� ��莞�Ԉȓ��v�Ȃ�t���b�N�m�肷��
                    //(3)�����͊m�肷��

                    tapUpPos2d = Input.mousePosition;
                    tapUpTime = Time.time;

                    if ((Vector2.Distance(tapUpPos2d, tapDownPos2d) <= TAP_LIMIT_POS_DIFF) &&  //�^�b�v���E���W���ȓ� ����
                        ((tapUpTime - tapDownTime) <= TAP_CONFIRM_TIME)) //�^�b�v�m�莞�Ԉȓ�
                    {
                        //�^�b�v�m�肷��
                        isTap = true;
                        isFlick = false;
                        isJudgePermit = false;

                        Debug.Log("�^�b�v����܂���");

                    }
                    else if ((TAP_LIMIT_POS_DIFF < Vector2.Distance(tapUpPos2d, tapDownPos2d)) &&  //�^�b�v���E���W������ ����
                             ((tapUpTime - tapDownTime) <= FLICK_LIMIT_TIME)) //�t���b�N���E���Ԉȓ�
                    {
                        //�t���b�N�m�肷��
                        isTap = false;
                        isFlick = true;
                        isJudgePermit = false;

                        Debug.Log("�t���b�N����܂���");
                    }
                    else
                    {
                        //�����͊m�肷��
                        isTap = false;
                        isFlick = false;
                        isJudgePermit = false;

                        Debug.Log("�����͂ł�");
                    }
                }
            }

            //��t���b�N�֘A�̎Z�o
            if (isFlick)
            {
                //�t���b�N�p�x�Z�o
                flickAngle = GetAngle(tapDownPos2d, tapUpPos2d);

                //��t���b�N���肷��
                if ((UP_FLICK_CONFIRM_ANGLE_MIN < flickAngle) && (flickAngle < (180f - UP_FLICK_CONFIRM_ANGLE_MIN)))
                {
                    isUpFlick = true;
                }
                else
                {
                    isUpFlick = false;
                }
            }

            //===========================================
            //�E�N���b�N���o�͂���
            //===========================================
            if (isRightClick) { isRightClick = false; } //true��1�����̂�
            if (Input.GetMouseButtonDown(1)) //�E�N���b�N�������ꂽ�u��
            {
                isRightClick = true;
            }

        }

        /*==============================================================*/
        /*====�����֐�==========================================*/
        /*==============================================================*/

        // p2����p1�ւ̊p�x�����߂�
        // @param p1 �����̍��W
        // @param p2 ����̍��W
        // @return 2�_�̊p�x(Degree)
        float GetAngle(Vector2 p1, Vector2 p2)
        {
            float dx = p2.x - p1.x;
            float dy = p2.y - p1.y;
            float rad = Mathf.Atan2(dy, dx);
            return rad * Mathf.Rad2Deg;
        }



    }
}
