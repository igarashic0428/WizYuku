namespace Assets.Scripts.Goals
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    using Assets.Scripts.Players;

    public class GoalController : MonoBehaviour
    {
        //Playerのインスタンス生成
        PlayerController PlayerController;

        //公開変数
        public bool isGoal;

        /*==============================================================*/
        /*====公開関数==================================================*/
        /*==============================================================*/
        //ゴール状態を取得
        public bool GetIsGoal()
        {
            return isGoal;
        }

        //ゴールを非アクティブにする
        public void SetInActiveGoal()
        {
            this.gameObject.SetActive(false);
        }

        //ゴール状態を取得
        public void SetActiveGoal()
        {
            this.gameObject.SetActive(true);
        }

        /*=======================================================================*/
        /*====Unityから直接呼ばれる関数==========================================*/
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

        //当たり判定関数
        private void OnTriggerEnter(Collider other)
        {
            //当たってきたオブジェクトがPlayerのとき
            if (other.name == PlayerController.GetName())
            {
                //一旦ログに出力
                Debug.Log("Playerと接触しました！");

                isGoal = true;
            }
        }
    }
}