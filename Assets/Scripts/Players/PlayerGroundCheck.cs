
namespace Assets.Scripts.Players
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class PlayerGroundCheck : MonoBehaviour
    {
        private string TagGround = "TagGround";
        private string TagTree = "TagTree";
        private string TagRock = "TagRock";
        private string TagObjFence = "TagObjFence";
        private string TagSpriteFence = "TagSpriteFence";
        private string TagEnemy = "TagEnemy";

        private bool isGround = false;
        private bool isGroundEnter, isGroundStay, isGroundExit;

        /*=======================================================================*/
        /*====Unity���璼�ڌĂ΂��֐�==========================================*/
        /*=======================================================================*/

        private void OnTriggerEnter(Collider collision)
        {
            Debug.Log("OnTriggerEnter���R�[������܂����B");
            if ((collision.tag == TagGround) ||
                (collision.tag == TagTree) ||
                (collision.tag == TagRock) ||
                (collision.tag == TagObjFence) ||
                (collision.tag == TagSpriteFence) ||
                (collision.tag == TagEnemy)   )

            {
                Debug.Log(collision.tag + "�ɓ�����܂����B");
                isGroundEnter = true;
            }
        }

        private void OnTriggerStay(Collider collision)
        {
            Debug.Log("OnTriggerStay���R�[������܂����B");
            if ((collision.tag == TagGround) ||
                (collision.tag == TagTree) ||
                (collision.tag == TagRock) ||
                (collision.tag == TagObjFence) ||
                (collision.tag == TagSpriteFence) ||
                (collision.tag == TagEnemy))
            {
                Debug.Log(collision.tag + "�ɓ����Ă��܂��B");
                isGroundStay = true;
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            Debug.Log("OnTriggerExit���R�[������܂����B");
            if ((collision.tag == TagGround) ||
                (collision.tag == TagTree) ||
                (collision.tag == TagRock) ||
                (collision.tag == TagObjFence) ||
                (collision.tag == TagSpriteFence) ||
                (collision.tag == TagEnemy))
            {
                Debug.Log(collision.tag + "���痣��܂���.");
                isGroundExit = true;
            }
        }

        void FixedUpdate()
        {
            if (isGroundEnter || isGroundStay)
            {
                isGround = true;
                Debug.Log("GroundCheck�������ɏՓ˂��܂���");
            }
            else if (isGroundExit)
            {
                isGround = false;
            }
            else
            {
                isGround = false;
            }

            isGroundEnter = false;
            isGroundStay = false;
            isGroundExit = false;
        }

        /*==============================================================*/
        /*====���J�֐�==========================================*/
        /*==============================================================*/

        //�ڒn�����Ԃ����\�b�h
        //��������̍X�V���ɌĂԕK�v������
        public bool GetIsGround()
        {
            return isGround;
        }
    }
}