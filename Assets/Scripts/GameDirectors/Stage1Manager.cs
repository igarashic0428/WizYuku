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
        //==============================================�萔��`==============================================
        const int ENEMY_MAX = (int)6; //�|���G�̍ő吔

        //==============================================�ϐ���`==============================================
        string nowScene = null;

        //UI�֘A
        ReStartMenuCanvasController ReStartMenuCanvasController;
        TimerBoardController TimerBoardController;
        MissionBoardController MissionBoardController;

        //Goal�֘A
        GoalController GoalController;
        bool isGoalOld = false;

        //GameOver�֘A
        bool isGameOverOld = false;

        //Player�̃C���X�^���X����
        PlayerController PlayerController;

        //�G�l�~�[�����񂾉�
        int enemyDeadCount = 0;
        int enemyDeadCountOld = 0;

        /*==============================================================*/
        /*====���J�֐�==================================================*/
        /*==============================================================*/
        /// <summary>
        /// �G�l�~�[�����񂾉񐔂�1�v���X����
        /// </summary>
        public void SetEnemyDeadCountInc()
        {
            enemyDeadCount++;
        }

        /*=======================================================================*/
        /*====Unity���璼�ڌĂ΂��֐�==========================================*/
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

            //BGM�𐶐�����====================================
            nowScene = SceneManager.GetActiveScene().name;
            if (nowScene == "Stage1")
            {
                //BGM���Đ�����
                AudioManager.instance.PlayBGM((int)AudioManager.BgmNum.Stage1);
            }
        }

        /*=======================================================================*/
        /*====Unity���璼�ڌĂ΂��֐�==========================================*/
        /*=======================================================================*/
        // Update is called once per frame
        void Update()
        {
            if(enemyDeadCount != enemyDeadCountOld) //�J�E���g���ς������
            {
                //�c����Z�o
                int Nokori = ENEMY_MAX - enemyDeadCount;

                //MissionBoard���X�V
                MissionBoardController.DrawMission( Nokori );
            }

            //�G�l�~�[��6�̂Ƃ��|���ꂽ��S�[�����o��������===========
            if (enemyDeadCount >= ENEMY_MAX)
            {
                //�S�[�����o��������
                GoalController.SetActiveGoal();
            }

            //�S�[������������
            if (GoalController.GetIsGoal()==true)
            {
                if (isGoalOld == false) //1�񂵂���������Ȃ��悤�ɂ���
                {
                    //�S�[�����j���[��\������
                    ReStartMenuCanvasController.DrawGoalMenu();

                    //�^�C�}�[���~�߂�
                    TimerBoardController.SetStop();

                }
            }

            if (PlayerController.GetIsGameOver() == true) //GameOver�Ȃ��
            {
                if (isGameOverOld == false) //1�񂵂���������Ȃ��悤�ɂ���
                {
                    //�Q�[���I�[�o�[���j���[��\������
                    ReStartMenuCanvasController.DrawGameOverMenu();
                }
            }

            //GameOver�֌W========================================================================================

        }
    }
}
