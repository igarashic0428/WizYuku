namespace Assets.Scripts.GameDirectors
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    using Assets.Scripts.Inputs;
    using Assets.Scripts.Players;

    public class PlayerScheduler : MonoBehaviour
    {
        //インターフェース宣言
        InputController InputController;
        PlayerController PlayerController;

        //===================================================
        //Unityから呼ばれる関数==============================

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
            //処理順がシビアなものはここにスケジューリング
            InputController.UpdateInput();
            PlayerController.UpdatePlayer();
        }
    }
}
