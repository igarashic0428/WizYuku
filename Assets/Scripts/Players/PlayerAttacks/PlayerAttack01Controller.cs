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

        Vector3 moveNVec; //攻撃目標への正規ベクトル

        private int power; //攻撃力

        /*==============================================================*/
        /*====セット用====================================================*/
        //位置セット
        public void SetPos(Vector3 Pos)
        {
            this.transform.position = Pos;
        }

        //攻撃力セット
        public void SetPower(int setPower)
        {
            power = setPower;
        }

        //攻撃目標のオブジェクトをセット
        public void SetTargetObj(GameObject Obj)
        {
            if (Obj!=null) {
                targetObj = Obj;

                //攻撃目標への正規ベクトルを算出
                Vector3 v = targetObj.transform.position;
                v.y += 3f; //デフォルトだと足元なので高さを補正する
                moveNVec = (v - this.transform.position).normalized;
            }
        }

        //タグをセット
        public void SetTag(string TagName)
        {
            this.gameObject.tag = TagName;
        }

        /*==============================================================*/
        /*====取得用====================================================*/
        //攻撃力取得
        public int GetPower()
        {
            return power;
        }

        /*=======================================================================*/
        /*====Unityから直接呼ばれる関数==========================================*/
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
            //ステージ外に出ていったら自身を破棄する
            if ((this.transform.position.x < -40f) ||
                (this.transform.position.x > 200f) ||
                (this.transform.position.y < -20f) ||
                (this.transform.position.y > 20f) ||
                (this.transform.position.z < -20f) ||
                (this.transform.position.z > 20f))//ステージ外ならば
            {
                //自身を破棄する
                Destroy(this.gameObject);
            }
        }

        void FixedUpdate()
        {
            rb.velocity = moveNVec * 15f;
        }

        private void OnTriggerEnter(Collider collision)
        {
            //Debug.Log("OnTriggerEnterがコールされました。");

            //爆発エフェクトを生成する
            //ファイアボールを生成
            GameObject ExplosionPrefab = (GameObject)Resources.Load("ParticleExplosion"); //プレハブを取得 ※必ずResourcesフォルダに格納すること
            GameObject Explosion = Instantiate(ExplosionPrefab); //プレハブからオブジェクト生成する
            Explosion.transform.position = this.transform.position;
            Explosion.tag = this.tag;

            //爆発のSEを鳴らす
            AudioManager.instance.PlaySE((int)AudioManager.SeNum.Explosion);

            //衝突時にEnemy01ReceiveDamageがこのクラスのGetPowerをコールする為、1フレーム待ってから自身を破棄する
            StartCoroutine(OneFrameLateDestroy()); //コルーチンの関数をコールする
        }

        //コルーチンの関数定義
        IEnumerator OneFrameLateDestroy()
        {
            //1フレーム処理を待ちます。
            yield return null;

            //自身を破棄する
            Destroy(this.gameObject);
        }
    }
}