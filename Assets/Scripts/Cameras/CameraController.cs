namespace Assets.Scripts.Cameras
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class CameraController : MonoBehaviour
    {
        GameObject Player;      //Player�i�[�p
        Vector3 diff;           //Player�Ƃ̋���

        Camera Cam;             //Camera Component �i�[�p

        RaycastHit rayHitInfo; //���C�̃q�b�g���i�[�p

        /*==============================================================*/
        /*====���J�֐�==================================================*/
        /*==============================================================*/

        //=======================================================================
        //====Raycas�֘A=========================================================

        /// <summary>
        /// �}�E�X���W����3D���W�ɕϊ��\������
        /// ����F
        ///   �C���X�y�N�^�[���Ray���Ǝ˂���I�u�W�F�N�g��Layer��ݒ肷�邱��
        ///   Layer7�FEnemy
        ///   Layer8�FGround
        /// </summary>
        /// <param name="vec2">2D���W</param>
        public bool IsRaycastHitVec2To3d(Vector2 vec2)
        {
            //�}�E�X���W��3D�̃I�u�W�F�N�g��hit����������
            Ray ray = Camera.main.ScreenPointToRay(vec2);

            // Layer8��Layer7�ɏƎ�(Layer��8bit�ڂ�7bit��)
            int mask = 0b1_1000_0000;

            bool hasHit = Physics.Raycast(ray, out rayHitInfo, Mathf.Infinity, mask);

            if (hasHit == true)
            {
                Debug.DrawRay(ray.origin, ray.direction * 100f, Color.blue, 1f); //�f�o�b�O�p
                Debug.Log(rayHitInfo.collider.tag + "�Ƀ��C��������܂���"); //�f�o�b�O�p
            }

            return hasHit;
        }

        /// <summary>
        /// ���C���q�b�g����3D���W���擾
        /// ����F���IsRaycastHitVec2To3d���R�[�����鎖
        /// </summary>
        public Vector3 GetRayHitPosVec3()
        {
            return rayHitInfo.point;
        }

        /// <summary>
        /// �q�b�g�����I�u�W�F�N�g��Tag���擾
        /// ����F���IsRaycastHitVec2To3d���R�[�����鎖
        /// </summary>
        public string GetRayHitObjTag()
        {
            string result = null;
            if (rayHitInfo.collider != null) //�q�b�g�����I�u�W�F�N�g���j������Ă��܂��Ă������̑΍�
            {
                result = rayHitInfo.collider.tag;
            }
            return result;
        }

        /// <summary>
        /// �q�b�g�����Q�[���I�u�W�F�N�g���擾
        /// ����F���IsRaycastHitVec2To3d���R�[�����鎖
        /// </summary>
        public GameObject GetRayHitGameObject()
        {
            return rayHitInfo.collider.gameObject;
        }

        //=======================================================================
        //====Camera�֘A=========================================================

        /// <summary>
        /// �J�����ʒu�擾
        /// </summary>
        public Vector3 GetPos()
        {
            return this.transform.position;
        }

        /// <summary>
        /// �J�����`��T�C�Y�擾
        /// </summary>
        public float GetOrthographicSize()
        {
            return Cam.orthographicSize;
        }

        /*=======================================================================*/
        /*====Unity���璼�ڌĂ΂��֐�==========================================*/
        /*=======================================================================*/

        // Start is called before the first frame update
        void Start()
        {
            Player = GameObject.Find("Player");
            diff = Player.transform.position - transform.position;

            //�J�����R���|�[�l���g�擾�p
            Cam = gameObject.GetComponent<Camera>();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (Player != null) //target(Player)���j������ĂȂ����
            {
                Vector3 l = Vector3.Lerp(transform.position, Player.transform.position - diff, Time.deltaTime * 5.0f);
                transform.position = new Vector3(l.x, transform.position.y, transform.position.z);
            }
        }
    }
}