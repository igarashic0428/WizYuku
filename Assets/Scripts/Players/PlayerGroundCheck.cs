
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
        /*====Unityから直接呼ばれる関数==========================================*/
        /*=======================================================================*/

        private void OnTriggerEnter(Collider collision)
        {
            Debug.Log("OnTriggerEnterがコールされました。");
            if ((collision.tag == TagGround) ||
                (collision.tag == TagTree) ||
                (collision.tag == TagRock) ||
                (collision.tag == TagObjFence) ||
                (collision.tag == TagSpriteFence) ||
                (collision.tag == TagEnemy)   )

            {
                Debug.Log(collision.tag + "に当たりました。");
                isGroundEnter = true;
            }
        }

        private void OnTriggerStay(Collider collision)
        {
            Debug.Log("OnTriggerStayがコールされました。");
            if ((collision.tag == TagGround) ||
                (collision.tag == TagTree) ||
                (collision.tag == TagRock) ||
                (collision.tag == TagObjFence) ||
                (collision.tag == TagSpriteFence) ||
                (collision.tag == TagEnemy))
            {
                Debug.Log(collision.tag + "に当っています。");
                isGroundStay = true;
            }
        }

        private void OnTriggerExit(Collider collision)
        {
            Debug.Log("OnTriggerExitがコールされました。");
            if ((collision.tag == TagGround) ||
                (collision.tag == TagTree) ||
                (collision.tag == TagRock) ||
                (collision.tag == TagObjFence) ||
                (collision.tag == TagSpriteFence) ||
                (collision.tag == TagEnemy))
            {
                Debug.Log(collision.tag + "から離れました.");
                isGroundExit = true;
            }
        }

        void FixedUpdate()
        {
            if (isGroundEnter || isGroundStay)
            {
                isGround = true;
                Debug.Log("GroundCheckが何かに衝突しました");
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
        /*====公開関数==========================================*/
        /*==============================================================*/

        //接地判定を返すメソッド
        //物理判定の更新毎に呼ぶ必要がある
        public bool GetIsGround()
        {
            return isGround;
        }
    }
}