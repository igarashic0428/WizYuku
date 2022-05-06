namespace Assets.Scripts.Enemy01s.Enemy01Attacks
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class Enemy01Attack01Controller : MonoBehaviour
    {
        float ATTACK_TIME = 0.25f; //攻撃時間(秒)
        float startTime; //開始時間

        private int power; //攻撃力

        /*======================================================*/
        /*====公開関数==========================================*/
        /*======================================================*/
        //攻撃力セット
        public void SetPower(int setPower)
        {
            power = setPower;
        }

        //タグをセット
        public void SetTag(string tagName)
        {
            this.gameObject.tag = tagName;
        }

        /*==============================================================*/
        /*====取得用====================================================*/
        //攻撃力取得
        public int GetPower()
        {
            return power;
        }

        /*==============================================================*/
        /*====Unityから直接呼ばれる関数=================================*/
        /*==============================================================*/
        // Start is called before the first frame update
        void Start()
        {
            startTime = Time.time; //生成時開始時間を保存する
        }

        //更新処置
        void FixedUpdate()
        {
            //所定時間経った後に自分を破棄する
            float t = Time.time;
            if (ATTACK_TIME < (t - startTime))
            {
                //自分を破棄
                Destroy(this.gameObject);
            }
        }

        /*======================================================*/
        /*====内部関数==========================================*/
        /*======================================================*/

    }

}