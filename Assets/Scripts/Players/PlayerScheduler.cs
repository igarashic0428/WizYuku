namespace Assets.Scripts.GameDirectors
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    using Assets.Scripts.Inputs;
    using Assets.Scripts.Players;

    public class PlayerScheduler : MonoBehaviour
    {
        //�C���^�[�t�F�[�X�錾
        InputController InputController;
        PlayerController PlayerController;

        //===================================================
        //Unity����Ă΂��֐�==============================

        // Start is called before the first frame update
        void Start()
        {
            InputController = GameObject.Find("InputController").GetComponent<InputController>();
            PlayerController = GameObject.Find("Player").GetComponent<PlayerController>();
        }

        // Update is called once per frame
        void Update()
        {
            Scheduler();
        }

        void Scheduler()
        {
            //���������V�r�A�Ȃ��̂͂����ɃX�P�W���[�����O
            InputController.UpdateInput();
            PlayerController.UpdatePlayer();
        }
    }
}
