
namespace Assets.Scripts.Inputs
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    using Assets.Scripts.Cameras;

    public class InputController : MonoBehaviour
    {
        //==============================================定数定義==============================================
        const float TAP_CONFIRM_TIME = 0.3f;            //タップ確定時間の閾値
        const float TAP_LIMIT_POS_DIFF = 30.0f;         //タップ限界座標差の閾値
        const float FLICK_LIMIT_TIME = 0.5f;            //プリック限界時間の閾値

        const float UP_FLICK_CONFIRM_ANGLE_MIN = 5.0f;  //上フリック確定角度

        //==============================================外部クラス呼び出し==============================================

        //クラスのインスタンス
        CameraController CameraController;

        //==============================================変数定義==============================================
        public bool isTap; //タップ状態

        bool isFlick; //フリック状態
        public bool isUpFlick; //上フリック状態

        bool isJudgePermit; //判定許可

        //タップとフリックの分別用
        Vector2 tapDownPos2d = new Vector2(0, 0); //タップ座標Down
        Vector2 tapNowPos2d = new Vector2(0, 0); //タップ座標Now
        Vector2 tapUpPos2d = new Vector2(0, 0); //タップ座標Up

        float tapDownTime; //指が画面に触れた瞬間の時間
        float tapNowTime;  //現在の時間
        float tapUpTime;   //指が画面から離れた瞬間の時間

        public float flickAngle; //フリック角度

        //============================================================
        //右クリック関連==============================================
        bool isRightClick; //右クリック状態

        //==============================================関数定義==============================================

        /*==============================================================*/
        /*====Unityから直接呼ばれる関数==========================================*/
        /*==============================================================*/

        // Start is called before the first frame update
        void Start()
        {
            //クラスのインスタンスにクラスを格納
            CameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();
        }

        /*==============================================================*/
        /*====公開関数==================================================*/
        /*==============================================================*/

        /*==============================================================*/
        /*====取得用====================================================*/

        //タップされたことを判定
        public bool GetIsTap()
        {
            return isTap;
        }

        //3D空間上のタップ座標を取得
        public Vector3 GetTapPos3d()
        {
            Vector3 result;

            bool isHit = CameraController.IsRaycastHitVec2To3d(Input.mousePosition);
            if (isHit == true) //3D座標に変換可能ならば
            {
                result = CameraController.GetRayHitPosVec3();
            }
            else
            {
                result = new Vector3(0,0,0);
            }

            return result;
        }

        //タップされたオブジェクトのタグを取得
        public string GetTapObjTag()
        {
            string result;

            bool isHit = CameraController.IsRaycastHitVec2To3d(Input.mousePosition);
            if (isHit == true) //3D座標に変換可能ならば
            {
                result = CameraController.GetRayHitObjTag();
            }
            else
            {
                result = null;
            }

            return result;
        }

        //タップされたゲームオブジェクトを取得
        public GameObject GetTapGameObject()
        {
            GameObject result;

            bool isHit = CameraController.IsRaycastHitVec2To3d(Input.mousePosition);
            if (isHit == true) //3D座標に変換可能ならば
            {
                result = CameraController.GetRayHitGameObject();
            }
            else
            {
                result = null;
            }

            return result;
        }

        //上フリックされたことを判定
        public bool GetIsUpFlick()
        {
            return isUpFlick;
        }

        //右クリック状態を取得
        public bool GetInputisRightClick()
        {
            return isRightClick;
        }

        /*==============================================================*/
        /*====更新用====================================================*/

        //更新処理
        public void UpdateInput()
        {
            //===========================================
            //左クリック(タップ)と上フリックの2パターンを出力する
            //===========================================

            if (isTap) { isTap = false; } //trueは1周期のみ
            if (isFlick) { isFlick = false; } //trueは1周期のみ
            if (isUpFlick) { isUpFlick = false; } //trueは1周期のみ

            if (Input.GetMouseButtonDown(0)) //押された瞬間
            {
                tapDownPos2d = Input.mousePosition;
                tapDownTime = Time.time;
                isJudgePermit = true;
            }

            if (isJudgePermit)
            {
                if (Input.GetMouseButton(0)) //押されている間
                {
                    //押しっぱなしの時 優先順位:(1)>(2)
                    //(1)「動いていない かつ 一定時間経過」ならタップ確定する
                    //(2)何もしない

                    tapNowPos2d = Input.mousePosition;
                    tapNowTime = Time.time;

                    if ((Vector2.Distance(tapNowPos2d, tapDownPos2d) <= TAP_LIMIT_POS_DIFF) &&  //タップ限界座標差以内 かつ
                        (TAP_CONFIRM_TIME < (tapNowTime - tapDownTime))) //タップ確定時間を経過
                    {
                        //タップ確定する
                        isTap = true;
                        isFlick = false;
                        isJudgePermit = false;

                        Debug.Log("タップされました(押しっぱなし)");

                    }
                }

                if (Input.GetMouseButtonUp(0)) //離れた瞬間
                {
                    //離れた瞬間の時 優先順位:(1)>(2)>(3)
                    //(1)「動いていない かつ 一定時間以内」ならタップ確定する
                    //(2)「動いている かつ 一定時間以内」ならフリック確定する
                    //(3)未入力確定する

                    tapUpPos2d = Input.mousePosition;
                    tapUpTime = Time.time;

                    if ((Vector2.Distance(tapUpPos2d, tapDownPos2d) <= TAP_LIMIT_POS_DIFF) &&  //タップ限界座標差以内 かつ
                        ((tapUpTime - tapDownTime) <= TAP_CONFIRM_TIME)) //タップ確定時間以内
                    {
                        //タップ確定する
                        isTap = true;
                        isFlick = false;
                        isJudgePermit = false;

                        Debug.Log("タップされました");

                    }
                    else if ((TAP_LIMIT_POS_DIFF < Vector2.Distance(tapUpPos2d, tapDownPos2d)) &&  //タップ限界座標差より上 かつ
                             ((tapUpTime - tapDownTime) <= FLICK_LIMIT_TIME)) //フリック限界時間以内
                    {
                        //フリック確定する
                        isTap = false;
                        isFlick = true;
                        isJudgePermit = false;

                        Debug.Log("フリックされました");
                    }
                    else
                    {
                        //未入力確定する
                        isTap = false;
                        isFlick = false;
                        isJudgePermit = false;

                        Debug.Log("未入力です");
                    }
                }
            }

            //上フリック関連の算出
            if (isFlick)
            {
                //フリック角度算出
                flickAngle = GetAngle(tapDownPos2d, tapUpPos2d);

                //上フリック判定する
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
            //右クリックを出力する
            //===========================================
            if (isRightClick) { isRightClick = false; } //trueは1周期のみ
            if (Input.GetMouseButtonDown(1)) //右クリックが押された瞬間
            {
                isRightClick = true;
            }

        }

        /*==============================================================*/
        /*====内部関数==========================================*/
        /*==============================================================*/

        // p2からp1への角度を求める
        // @param p1 自分の座標
        // @param p2 相手の座標
        // @return 2点の角度(Degree)
        float GetAngle(Vector2 p1, Vector2 p2)
        {
            float dx = p2.x - p1.x;
            float dy = p2.y - p1.y;
            float rad = Mathf.Atan2(dy, dx);
            return rad * Mathf.Rad2Deg;
        }



    }
}
