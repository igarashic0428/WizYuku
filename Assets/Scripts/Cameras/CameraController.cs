namespace Assets.Scripts.Cameras
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class CameraController : MonoBehaviour
    {
        GameObject Player;      //Player格納用
        Vector3 diff;           //Playerとの距離

        Camera Cam;             //Camera Component 格納用

        RaycastHit rayHitInfo; //レイのヒット情報格納用

        /*==============================================================*/
        /*====公開関数==================================================*/
        /*==============================================================*/

        //=======================================================================
        //====Raycas関連=========================================================

        /// <summary>
        /// マウス座標から3D座標に変換可能か判定
        /// 制約：
        ///   インスペクター上でRayを照射するオブジェクトにLayerを設定すること
        ///   Layer7：Enemy
        ///   Layer8：Ground
        /// </summary>
        /// <param name="vec2">2D座標</param>
        public bool IsRaycastHitVec2To3d(Vector2 vec2)
        {
            //マウス座標が3Dのオブジェクトにhitしたか判定
            Ray ray = Camera.main.ScreenPointToRay(vec2);

            // Layer8とLayer7に照射(Layerの8bit目と7bit目)
            int mask = 0b1_1000_0000;

            bool hasHit = Physics.Raycast(ray, out rayHitInfo, Mathf.Infinity, mask);

            if (hasHit == true)
            {
                Debug.DrawRay(ray.origin, ray.direction * 100f, Color.blue, 1f); //デバッグ用
                Debug.Log(rayHitInfo.collider.tag + "にレイが当たりました"); //デバッグ用
            }

            return hasHit;
        }

        /// <summary>
        /// レイがヒットした3D座標を取得
        /// 制約：先にIsRaycastHitVec2To3dをコールする事
        /// </summary>
        public Vector3 GetRayHitPosVec3()
        {
            return rayHitInfo.point;
        }

        /// <summary>
        /// ヒットしたオブジェクトのTagを取得
        /// 制約：先にIsRaycastHitVec2To3dをコールする事
        /// </summary>
        public string GetRayHitObjTag()
        {
            string result = null;
            if (rayHitInfo.collider != null) //ヒットしたオブジェクトが破棄されてしまっていた時の対策
            {
                result = rayHitInfo.collider.tag;
            }
            return result;
        }

        /// <summary>
        /// ヒットしたゲームオブジェクトを取得
        /// 制約：先にIsRaycastHitVec2To3dをコールする事
        /// </summary>
        public GameObject GetRayHitGameObject()
        {
            return rayHitInfo.collider.gameObject;
        }

        //=======================================================================
        //====Camera関連=========================================================

        /// <summary>
        /// カメラ位置取得
        /// </summary>
        public Vector3 GetPos()
        {
            return this.transform.position;
        }

        /// <summary>
        /// カメラ描画サイズ取得
        /// </summary>
        public float GetOrthographicSize()
        {
            return Cam.orthographicSize;
        }

        /*=======================================================================*/
        /*====Unityから直接呼ばれる関数==========================================*/
        /*=======================================================================*/

        // Start is called before the first frame update
        void Start()
        {
            Player = GameObject.Find("Player");
            diff = Player.transform.position - transform.position;

            //カメラコンポーネント取得用
            Cam = gameObject.GetComponent<Camera>();
        }

        // Update is called once per frame
        void LateUpdate()
        {
            if (Player != null) //target(Player)が破棄されてなければ
            {
                Vector3 l = Vector3.Lerp(transform.position, Player.transform.position - diff, Time.deltaTime * 5.0f);
                transform.position = new Vector3(l.x, transform.position.y, transform.position.z);
            }
        }
    }
}