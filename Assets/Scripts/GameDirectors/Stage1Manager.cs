namespace Assets.Scripts.GameDirectors
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;
    using UnityEngine.SceneManagement;

    using Assets.Scripts.Goals;
    using Assets.Scripts.Players;

    using Assets.Scripts.Audio;

    public class Stage1Manager : MonoBehaviour
    {
        //==============================================定数定義==============================================
        const int ENEMY_MAX = (int)6; //倒す敵の最大数

        //==============================================変数定義==============================================
        string nowScene = null;

        //UI関連
        ReStartMenuCanvasController ReStartMenuCanvasController;
        TimerBoardController TimerBoardController;
        MissionBoardController MissionBoardController;

        //Goal関連
        GoalController GoalController;
        bool isGoalOld = false;

        //GameOver関連
        bool isGameOverOld = false;

        //Playerのインスタンス生成
        PlayerController PlayerController;

        //エネミーが死んだ回数
        int enemyDeadCount = 0;
        int enemyDeadCountOld = 0;

        /*==============================================================*/
        /*====公開関数==================================================*/
        /*==============================================================*/
        /// <summary>
        /// エネミーが死んだ回数を1プラスする
        /// </summary>
        public void SetEnemyDeadCountInc()
        {
            enemyDeadCount++;
        }

        /*=======================================================================*/
        /*====Unityから直接呼ばれる関数==========================================*/
        /*=======================================================================*/

        // Start is called before the first frame update
        void Start()
        {
            ReStartMenuCanvasController = GameObject.Find("ReStartMenuCanvas").GetComponent<ReStartMenuCanvasController>();
            TimerBoardController = GameObject.Find("UICanvas/TimerBoard").GetComponent<TimerBoardController>();

            MissionBoardController = GameObject.Find("UICanvas/MissionBoard").GetComponent<MissionBoardController>();
            MissionBoardController.DrawMission(ENEMY_MAX);

            GoalController = GameObject.Find("Goal").GetComponent<GoalController>();
            GoalController.SetInActiveGoal();

            PlayerController = GameObject.Find("Player").GetComponent<PlayerController>();

            //BGMを生成する====================================
            nowScene = SceneManager.GetActiveScene().name;
            if (nowScene == "Stage1")
            {
                //BGMを再生する
                AudioManager.instance.PlayBGM((int)AudioManager.BgmNum.Stage1);
            }
        }

        /*=======================================================================*/
        /*====Unityから直接呼ばれる関数==========================================*/
        /*=======================================================================*/
        // Update is called once per frame
        void Update()
        {
            if(enemyDeadCount != enemyDeadCountOld) //カウントが変わったら
            {
                //残りを算出
                int Nokori = ENEMY_MAX - enemyDeadCount;

                //MissionBoardを更新
                MissionBoardController.DrawMission( Nokori );
            }

            //エネミーが6体とも倒されたらゴールを出現させる===========
            if (enemyDeadCount >= ENEMY_MAX)
            {
                //ゴールを出現させる
                GoalController.SetActiveGoal();
            }

            //ゴールしたか判定
            if (GoalController.GetIsGoal()==true)
            {
                if (isGoalOld == false) //1回しか処理されないようにする
                {
                    //ゴールメニューを表示する
                    ReStartMenuCanvasController.DrawGoalMenu();

                    //タイマーを止める
                    TimerBoardController.SetStop();

                }
            }

            if (PlayerController.GetIsGameOver() == true) //GameOverならば
            {
                if (isGameOverOld == false) //1回しか処理されないようにする
                {
                    //ゲームオーバーメニューを表示する
                    ReStartMenuCanvasController.DrawGameOverMenu();
                }
            }

            //GameOver関係========================================================================================

        }
    }
}
