namespace Assets.Scripts.Enemy01s
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    using Assets.Scripts.Players;
    //using Assets.Scripts.Goals;

    using Assets.Scripts.Enemy01s.Enemy01Attacks;

    using Assets.Scripts.GameDirectors;

    public class Enemy01Controller : MonoBehaviour
    {
        //==============================================struct==============================================
        struct Parameters
        {
            public int hp;
            public int power;
        }

        Parameters Enemy01Para;

        //==============================================enum==============================================
        //SkeletonオブジェクトのAnimatorコンポーネントに設定したAnimStateの数値と合わせること
        enum State
        {
            NotFindSoIdle,   //0
            NotFindSoWalk,   //1
            Find,            //2
            Chase,           //3
            Attack01Charge,  //4
            Attack01,        //5
            Recoil,          //6
            NotFound,        //7
            Die,             //8
            Max
        }

        //==============================================定数定義==============================================

        //NotFindSoIdle,NotFindSoWalk関連
        float NOT_FIND_SO_IDLE_COUNT_TIME = 3f; //立ち止まる時間
        float NOT_FIND_SO_WALK_COUNT_TIME = 3f; //歩く時間
        float WALK_SPEED_X = 3f;

        //FindingPlayer関連
        float ENEMY_HEAD_POS_Y = 4.5f; //ENEMYの頭に位置
        float PLAYER_HEAD_POS_Y = 3f; //Playerの頭に位置

        float MAX_DISTANCE = 7f; //Playerを探すレイ照射の最大距離

        float FIND_TIME = 1.5f; //Playerを見つけた時の制御時間

        float CHASE_SPEED = 6f; //プレイヤーを追いかける時の移動速度

        //Chase関連
        float CHASE_DESTINATION_CORRECTION_X = 2f; //追いかける時の目的地補正値(X軸)
        float ATTACK01_ABLE_DISTANCE_X = 2f; //攻撃に入る間合い(X軸)
        float ATTACK01_ABLE_DISTANCE_Z = 0.5f; //攻撃に入る間合い(Z軸)

        //Attack01関連
        float ATTACK01_CHARGE_TIME = 0.5f; //溜めアニメーションの時間
        float ATTACK01_TIME = 0.5f; //攻撃アニメーションの時間

        //Recoil関連
        float RECOIL_TIME = 1f; //反動アニメーションの時間

        //ChaseToNotFoundCheck関連
        float CHASE_TO_NOT_FOUND_TIME = 3; //見失う確定時間

        //NotFound関連
        float NOT_FOUND_TIME = 3f; //プレイヤーを見失った時のアニメーションの時間

        //Die関連
        float DIE_TIME = 1f; //Dieアニメーションの時間

        //==============================================変数定義==============================================
        PlayerController PlayerController;

        public int animState; //アニメーション状態

        bool isNotFindSoIdle;
        bool isNotFindSoWalk;
        bool isFind;
        Vector3 ChaseDirection;
        bool isAttack01Charge;
        bool isAttack01;
        bool isRecoil;
        bool isChaseToNotFound;
        bool isNotFound;
        bool isDie;
        bool isDieParticle;
        float stateStartTime;
        Enemy01ReceiveDamage Enemy01ReceiveDamage;
        bool isReceiveDamage;

        //物理演算関連
        private Rigidbody rb; //物理演算用

        //インスタンス生成
        GameObject objSkeleton; //子オブジェクト「Skeleton」を取得

        private Animator Anim; //Animator Component 格納用

        /*======================================================*/
        /*====公開関数==========================================*/
        /*======================================================*/

        //位置取得
        public Vector3 GetPos()
        {
            return this.transform.position;
        }

        //Hp取得
        public float GetHp()
        {
            return Enemy01Para.hp;
        }

        /*==============================================================*/
        /*====Unityから直接呼ばれる関数=================================*/
        /*==============================================================*/

        // Start is called before the first frame update
        void Start()
        {
            PlayerController = GameObject.Find("Player").GetComponent<PlayerController>();

            Enemy01ReceiveDamage = this.gameObject.GetComponent<Enemy01ReceiveDamage>();

            animState = (int)State.NotFindSoIdle;

            rb = GetComponent<Rigidbody>(); //物理演算用

            objSkeleton = transform.Find("Skeleton").gameObject; //SkeletonのgameObject取得
            Anim = objSkeleton.GetComponent<Animator>();         //SkeletonのAnimatorコンポーネント取得

            Enemy01Para.hp = (int)100; //初期HP
            Enemy01Para.power = (int)20; //攻撃力
        }

        // Update is called once per frame
        //void Update() { }

        void FixedUpdate()
        {
            //====================================================
            //挙動の制御関連========================================

            //Idle, Walk
            NotFindSoIdle(); //Playerが見つからないため立ち止まる状態を制御
            NotFindSoWalk(); //Playerが見つからないため歩く状態を制御
            FindingPlayer(); //Playerを探し見つけたらFindに遷移する

            //Find
            Find(); //見つけた状態の制御

            //Chase
            ChasePlayer(); //Playerを追いかける制御
            ChaseToAttack01Charge(); //Playerの傍まで来たらAttack01Chargeに遷移する

            //Attack01Charge
            Attack01Charge(); //Attack01ChargeならばAttack01の溜め制御

            //Attack01
            Attack01(); //攻撃する制御

            //Recoil
            Recoil();//反動の制御

            //Chase
            ChaseToNotFoundCheck(); //Playerを見失ったらNotFindSoWalkに遷移する

            //NotFound
            NotFound(); //Playerを見失った時の制御

            //落下判定
            FallCheck();

            //プレイヤーがゴールしたか判定
            //PlayerGoalCheck();

            DieCheck(); //死亡判定

            //Die
            Die();

            //被攻撃判定
            ReceiveDamageCheck();

            //物理演算関連
            PhysicsUpdate();//物理挙動を更新

            //疑似的に重力を設定
            Gravity();

            //アニメーター関連
            Animator(); //アニメーターを更新

        }


        /*======================================================*/
        /*====内部関数==========================================*/
        /*======================================================*/

        //立ち止まる状態を制御
        void NotFindSoIdle()
        {
            //==========================================================================================
            //立ち止まる状態を制御
            if (animState == (int)State.NotFindSoIdle)
            {
                //歩く状態になった初回は開始時間を保存
                if (isNotFindSoIdle == false)
                {
                    isNotFindSoIdle = true;
                    stateStartTime = Time.time;
                }

                //しばらく立ち止まる状態を続けたら反対を向いて歩く状態に切り替える
                float t = Time.time;
                if (NOT_FIND_SO_IDLE_COUNT_TIME < (t - stateStartTime))
                {
                    objSkeleton.transform.localScale = new Vector3(
                        objSkeleton.transform.localScale.x * -1f,
                        objSkeleton.transform.localScale.y,
                        objSkeleton.transform.localScale.z);
                    animState = (int)State.NotFindSoWalk;
                }
            }
            else
            {
                isNotFindSoIdle = false;
            }
        }

        //歩く状態を制御
        void NotFindSoWalk()
        {
            //==========================================================================================
            //歩く状態を制御
            if (animState == (int)State.NotFindSoWalk)
            {
                //歩く状態になった初回は開始時間を保存
                if (isNotFindSoWalk == false)
                {
                    isNotFindSoWalk = true;
                    stateStartTime = Time.time;
                }

                //しばらく歩く状態を続けたら立ち止まる状態に切り替える
                float t = Time.time;
                if (NOT_FIND_SO_WALK_COUNT_TIME < (t - stateStartTime))
                {
                    animState = (int)State.NotFindSoIdle;
                }
            }
            else
            {
                isNotFindSoWalk = false;
            }
        }

        //Playerを探す制御
        void FindingPlayer()
        {
            //Playerを見つけた瞬間の制御
            if (animState == (int)State.NotFindSoIdle || animState == (int)State.NotFindSoWalk) {

                //====レイ用にエネミーからプレイヤーへの方向を算出====
                Vector3 PlayerPos;
                Vector3 EnemyPos;
                Vector3 Direction;

                //Enemyの位置を算出
                EnemyPos = this.transform.position;
                EnemyPos.y = EnemyPos.y + ENEMY_HEAD_POS_Y;

                //Playerの位置を算出
                PlayerPos = PlayerController.GetPos();
                PlayerPos.y = PlayerPos.y + PLAYER_HEAD_POS_Y;
                Direction = (PlayerPos - EnemyPos).normalized;

                //====EnemyPosからPlayerPosの方向へレイを飛ばす====
                RaycastHit hit;
                bool hasHit = Physics.Raycast(EnemyPos, Direction, out hit, MAX_DISTANCE); //Rayを生成

                Ray ray = new Ray(EnemyPos, Direction); //デバッグ用
                Debug.DrawRay(ray.origin, ray.direction * MAX_DISTANCE, Color.red); //デバッグ用

                //====レイがプレイヤーに衝突したらFindに遷移する====
                if (hasHit) //もしRayを投射して何らかのコライダーに衝突したら
                {
                    GameObject obj = hit.collider.gameObject; // 衝突した相手オブジェクトを取得

                    if (obj.tag == "TagPlayer") //レイが衝突した相手オブジェクトのタグがTagPlayerなら
                    {
                        if ( ( (0f <= objSkeleton.transform.localScale.x) &&
                               (0f <= (obj.transform.position.x - this.transform.position.x) ) ) || //右向きかつプレイヤーが右にいるならば
                             ( (0f > objSkeleton.transform.localScale.x) &&
                               (0f > (obj.transform.position.x - this.transform.position.x)  ) )) //左向きかつプレイヤーが左にいるならば
                        {
                            Debug.Log("Enemyが" + obj.name + "を見つけました"); //コンソールに表示
                            animState = (int)State.Find; //Findに遷移する
                        }
                    }
                }
            }
        }

        //見つけた状態の制御
        void Find()
        {
            //====見つけた時の制御====
            if (animState == (int)State.Find)
            {
                //見つけた状態になった初回は開始時間を保存
                if (isFind == false)
                {
                    isFind = true;
                    stateStartTime = Time.time;

                    //「eye L open」「eye R open」をアクティブにする
                    string str_eye_R_open = "Skeleton/PSB_Character_SkeletonB/eye_R";//自身の「eye R open」オブジェクトを取得
                    string str_eye_L_open = "Skeleton/PSB_Character_SkeletonB/eye_L";//自身の「eye L open」オブジェクトを取得
                    Transform EyeROpen = this.transform.Find(str_eye_R_open);//自身の「eye R open」オブジェクトを取得
                    Transform EyeLOpen = this.transform.Find(str_eye_L_open);//自身の「eye L open」オブジェクトを取得
                    EyeROpen.gameObject.SetActive(true);
                    EyeLOpen.gameObject.SetActive(true);
                }
                //見つけた状態でFIND_TIME秒経過したらChaseに遷移
                float t = Time.time;
                if (FIND_TIME < (t - stateStartTime))
                {
                    animState = (int)State.Chase;
                }
            }
            else
            {
                isFind = false;
            }
        }

        //プレイヤーを追いかける制御
        void ChasePlayer()
        {
            //Playerを見つけた瞬間の制御
            if (animState == (int)State.Chase)
            {
                //====エネミーからプレイヤーへの方向を算出====
                Vector3 PlayerPos;
                Vector3 EnemyPos;
                Vector3 Destination;
                Vector3 d;
                EnemyPos = this.transform.position; //Enemyの位置を取得
                PlayerPos = PlayerController.GetPos(); //Playerの位置を取得
                Destination = PlayerPos;
                if ((Destination.x - EnemyPos.x) <= 0f)//プレイヤーの手前を目指す。左方向ならば少し右へ補正、右方向ならば少し左へ補正
                {
                    Destination.x += CHASE_DESTINATION_CORRECTION_X;
                }
                else
                {
                    Destination.x -= CHASE_DESTINATION_CORRECTION_X;
                }
                d = (Destination - EnemyPos); //方向を算出
                d.y = 0f; //空中へは追いかけない
                ChaseDirection = d.normalized; //方向を算出

                //スプライトの向きを設定
                if (0f<= (PlayerPos.x - EnemyPos.x)) //プレイヤーが右にいるならば
                {
                    if (objSkeleton.transform.localScale.x < 0f) //左を向いていたら反転する
                    {
                        objSkeleton.transform.localScale = new Vector3(
                            objSkeleton.transform.localScale.x * -1f,
                            objSkeleton.transform.localScale.y,
                            objSkeleton.transform.localScale.z);
                    }
                }
                else //プレイヤーが左にいるならば
                {
                    if (0f < objSkeleton.transform.localScale.x) //右を向いていたら反転する
                    {
                        objSkeleton.transform.localScale = new Vector3(
                            objSkeleton.transform.localScale.x * -1f,
                            objSkeleton.transform.localScale.y,
                            objSkeleton.transform.localScale.z);
                    }
                }
            }
        }

        //Playerの傍まで来たらAttack01Chargeに遷移
        void ChaseToAttack01Charge()
        {
            if (animState == (int)State.Chase)
            {
                //エネミーとプレイヤーの距離を算出
                float DistanceX;
                float DistanceZ;
                DistanceX = Mathf.Abs(this.transform.position.x - PlayerController.GetPos().x);
                DistanceZ = Mathf.Abs(this.transform.position.z - PlayerController.GetPos().z);

                //傍まで来ていたら
                if ( (DistanceX < ATTACK01_ABLE_DISTANCE_X) && (DistanceZ < ATTACK01_ABLE_DISTANCE_Z) )
                {
                    //Attack01Chargeに遷移
                    animState = (int)State.Attack01Charge;
                }
            }
        }

        //Attack01ChargeならばAttack01の溜め制御
        void Attack01Charge()
        {
            //Attack01Charge状態ならば所定時間経った後にAttack01状態に移行する
            if (animState == (int)State.Attack01Charge)
            {
                //Attack01Charge状態になった初回は開始時間を保存
                if (isAttack01Charge == false)
                {
                    isAttack01Charge = true;
                    stateStartTime = Time.time;
                }

                //所定時間経った後にAttack01状態に移行する
                float t = Time.time;
                if (ATTACK01_CHARGE_TIME <(t- stateStartTime))
                {
                    animState = (int)State.Attack01;
                }
            }
            else
            {
                isAttack01Charge = false;
            }
        }

        //Attack01の制御
        void Attack01()
        {
            //Attack01状態ならば
            if (animState == (int)State.Attack01)
            {
                if (isAttack01 == false) //Attack01Charge状態になった初回ならば
                {
                    isAttack01 = true;
                    stateStartTime = Time.time; //開始時間を保存する

                    //Attack01のオブジェクトを生成する===========================================
                    const string str_bone_AttackPoint = "Skeleton/PSB_Character_SkeletonB/bone_1_base/bone_2_body/bone_5_ArmUpL/bone_6_ArmDownL/bone_7_HandL";
                    Transform ParentTransform = this.transform.Find(str_bone_AttackPoint); //自身の「bone_9」オブジェクトを取得
                    GameObject Enemy01Attack01Prefab = (GameObject)Resources.Load("Enemy01Attack01"); //Enemy01Attack01プレハブを取得 ※必ずResourcesフォルダに格納すること
                    GameObject AttackPoint = Instantiate(Enemy01Attack01Prefab, ParentTransform); //「bone_9」オブジェクトの子としてEnemy01Attack01プレハブからオブジェクト生成する

                    //生成したオブジェクトのコンポーネントを取得
                    Enemy01Attack01Controller Enemy01Attack01Controller = AttackPoint.GetComponent<Enemy01Attack01Controller>();
                    Enemy01Attack01Controller.SetPower(Enemy01Para.power);
                    Enemy01Attack01Controller.SetTag("TagEnemyAttack"); //タグをセットする
                }

                //所定時間経った後にAttack01状態に移行する
                float t = Time.time;
                if (ATTACK01_TIME < (t - stateStartTime))
                {
                    animState = (int)State.Recoil;
                }
            }
            else
            {
                isAttack01 = false;
            }
        }

        //反動の制御
        void Recoil()
        {
            //Recoil状態ならば所定時間経った後にChase状態に移行する
            if (animState == (int)State.Recoil)
            {
                //Attack01Charge状態になった初回は開始時間を保存
                if (isRecoil == false)
                {
                    isRecoil = true;
                    stateStartTime = Time.time;
                }

                //所定時間経った後にAttack01状態に移行する
                float t = Time.time;
                if (RECOIL_TIME < (t - stateStartTime))
                {
                    animState = (int)State.Chase;
                }
            }
            else
            {
                isRecoil = false;
            }
        }

        //プレイヤーを見失ったかチェック
        void ChaseToNotFoundCheck()
        {
            if (animState == (int)State.Chase)
            {
                bool isNotFoundPlayer=false;

                //プレイヤーを見失ったか確認
                //今向いている方向にプレイヤーがいない
                if (((0f <= (PlayerController.GetPos().x - this.transform.position.x)) &&
                      (objSkeleton.transform.localScale.x < 0)) || //プレイヤーが右にいる かつ エネミーは左を向いている
                     ((0f > (PlayerController.GetPos().x - this.transform.position.x)) &&
                      (objSkeleton.transform.localScale.x > 0)))   //プレイヤーが左にいる かつ エネミーは右を向いている
                {
                    isNotFoundPlayer = true; //プレイヤーを見失った
                }
                else
                {
                    //レイをプレイヤーに照射してプレイヤーに当たるか確認(FindingPlayerのロジック流用)
                    //====レイ用にエネミーからプレイヤーへの方向を算出====
                    Vector3 PlayerPos;
                    Vector3 EnemyPos;
                    Vector3 Direction;

                    //Enemyの位置を算出
                    EnemyPos = this.transform.position;
                    EnemyPos.y = EnemyPos.y + ENEMY_HEAD_POS_Y;

                    //Playerの位置を算出
                    PlayerPos = PlayerController.GetPos();
                    PlayerPos.y = PlayerPos.y + PLAYER_HEAD_POS_Y;
                    Direction = (PlayerPos - EnemyPos).normalized;

                    //====EnemyPosからPlayerPosの方向へレイを飛ばす====
                    RaycastHit hit;
                    bool hasHit = Physics.Raycast(EnemyPos, Direction, out hit, MAX_DISTANCE); //Rayを生成

                    Ray ray = new Ray(EnemyPos, Direction); //デバッグ用
                    Debug.DrawRay(ray.origin, ray.direction * MAX_DISTANCE, Color.red, 1f); //デバッグ用

                    //====レイがプレイヤーに衝突したらFindに遷移する====
                    if (hasHit) //もしRayを投射して何らかのコライダーに衝突したら
                    {
                        GameObject obj = hit.collider.gameObject; // 衝突した相手オブジェクトを取得

                        if (obj.tag != "TagPlayer") //レイが衝突した相手オブジェクトのタグがTagPlayerではないなら
                        {
                            isNotFoundPlayer = true; //プレイヤーを見失った
                        }
                    }
                }

                //プレイヤーを見失った状態が所定時間続いたならばNotFound状態に移行する
                if (isNotFoundPlayer == true)
                {
                    //Attack01Charge状態になった初回は開始時間を保存
                    if (isChaseToNotFound == false)
                    {
                        isChaseToNotFound = true;
                        stateStartTime = Time.time;
                    }

                    //所定時間経った後にAttack01状態に移行する
                    float t = Time.time;
                    if (CHASE_TO_NOT_FOUND_TIME < (t - stateStartTime))
                    {
                        animState = (int)State.NotFound; //NotFoundに遷移
                    }
                }
                else
                {
                    isChaseToNotFound = false;
                }
            }
        }

        //Playerを見失った時の制御
        void NotFound() {
            //NotFound状態ならば所定時間経った後にNotFindSoWalk状態に移行する
            if (animState == (int)State.NotFound)
            {
                //Attack01Charge状態になった初回は開始時間を保存
                if (isNotFound == false)
                {
                    isNotFound = true;
                    stateStartTime = Time.time;

                    //「eye L open」「eye R open」を非アクティブにする
                    string str_eye_R_open = "Skeleton/PSB_Character_SkeletonB/eye_R";//自身の「eye_R」オブジェクトを取得
                    string str_eye_L_open = "Skeleton/PSB_Character_SkeletonB/eye_L";//自身の「eye_L」オブジェクトを取得
                    Transform EyeROpen = transform.Find(str_eye_R_open);//自身の「eye R open」オブジェクトを取得
                    Transform EyeLOpen = transform.Find(str_eye_L_open);//自身の「eye L open」オブジェクトを取得
                    EyeROpen.gameObject.SetActive(false);
                    EyeLOpen.gameObject.SetActive(false);
                }

                //所定時間経った後にAttack01状態に移行する
                float t = Time.time;
                if (NOT_FOUND_TIME < (t - stateStartTime))
                {
                    animState = (int)State.NotFindSoWalk;
                }
            }
            else
            {
                isNotFound = false;
            }
        }

        //落下判定
        void FallCheck()
        {
            if (this.transform.position.y < -12.0f) //y軸が-12以下ならば落下したと判断する
            {
                //死亡用のパーティクル生成
                GameObject Prefab = (GameObject)Resources.Load("ParticleSoul"); //プレハブを取得 ※必ずResourcesフォルダに格納すること
                GameObject ParticleSoul = Instantiate(Prefab); //プレハブからオブジェクト生成する
                Vector3 pos = this.transform.position;
                pos.y += 3; //低すぎてカメラに写るまでにラグがあるのでパーティクルの開始位置を少し高く設定
                ParticleSoul.transform.position = pos;

                //Stage1Managerに死んだことを伝える
                Stage1Manager IStage1Manager = GameObject.Find("Stage1Manager").GetComponent<Stage1Manager>();
                IStage1Manager.SetEnemyDeadCountInc();

                //自身を破棄する
                Destroy(this.gameObject);
            }
        }

        //死亡判定
        void DieCheck()
        {
            if (animState != (int)State.Die) //Die状態ではないならば
            {
                    if (Enemy01Para.hp <= 0) //Hpが0以下ならば
                    {
                        animState = (int)State.Die; //Die状態にする
                    }
            }
        }

        //死亡時の挙動
        void Die()
        {
            //Die状態ならば
            if (animState == (int)State.Die)
            {
                //Die状態になった初回は開始時間を保存
                if (isDie == false)
                {
                    isDie = true;
                    stateStartTime = Time.time;

                    //「eye_L」「eye_R」を非アクティブにする
                    string str_eye_R_open = "Skeleton/PSB_Character_SkeletonB/eye_R";//自身の「eye R open」オブジェクトを取得
                    string str_eye_L_open = "Skeleton/PSB_Character_SkeletonB/eye_L";//自身の「eye L open」オブジェクトを取得
                    Transform EyeROpen = this.transform.Find(str_eye_R_open);//自身の「eye R open」オブジェクトを取得
                    Transform EyeLOpen = this.transform.Find(str_eye_L_open);//自身の「eye L open」オブジェクトを取得
                    EyeROpen.gameObject.SetActive(false);
                    EyeLOpen.gameObject.SetActive(false);

                    //Stage1Managerに死んだことを伝える
                    Stage1Manager IStage1Manager = GameObject.Find("Stage1Manager").GetComponent<Stage1Manager>();
                    IStage1Manager.SetEnemyDeadCountInc();
                }

                //所定時間経った後に自身を破棄する
                float t = Time.time;
                if (DIE_TIME < (t - stateStartTime))
                {
                    if (isDieParticle == false)
                    {
                        //死亡時の演出
                        GameObject Prefab = (GameObject)Resources.Load("ParticleSoul"); //プレハブを取得 ※必ずResourcesフォルダに格納すること
                        GameObject ParticleSoul = Instantiate(Prefab); //プレハブからオブジェクト生成する
                        ParticleSoul.transform.position = this.transform.position;

                        //1秒後自身を破棄する
                        StartCoroutine("OneSecLateDestroy"); //コルーチンの関数OneSecLateDestroy()をコールする

                        isDieParticle = true;
                    }
                }
            }
            else
            {
                isDie = false;
            }
        }

        //1秒後自身を破棄する
        IEnumerator OneSecLateDestroy()
        {
            yield return new WaitForSeconds(1.0f); //このコルーチン関数内でこの行から下の処理は1.0秒待つ
            GameObject.Destroy(this.gameObject); //自身を破棄する
        }

        //ダメージを受ける
        void ReceiveDamageCheck()
        {
            //被攻撃判定を得る
            isReceiveDamage = Enemy01ReceiveDamage.GetIsReceiveDamage();

            if (isReceiveDamage == true) //敵からの攻撃を受けているならば
            {
                //ダメージを受ける
                int d = Enemy01ReceiveDamage.GetDamege();
                Enemy01Para.hp -= d;
                
                //Debug.Log("攻撃を受けました");
            }
        }

        //物理挙動の更新
        void PhysicsUpdate()
        {
            //Playerが見つからないから立ち止まる状態を制御
            if (animState == (int)State.NotFindSoIdle)
            {
                //XZ平面移動の速度を0にする。
                rb.velocity = new Vector3(0.0f, rb.velocity.y, 0.0f);
            }

            //Playerが見つからないから歩く状態を制御
            if (animState == (int)State.NotFindSoWalk)
            {
                //XZ平面移動の速度を更新する。
                if (0 < objSkeleton.transform.localScale.x) //右向きならば
                {
                    //右へ移動
                    rb.velocity = new Vector3(
                        WALK_SPEED_X,
                        rb.velocity.y,
                        rb.velocity.z);
                }
                else //左向きならば
                {
                    //左へ移動
                    rb.velocity = new Vector3(
                        WALK_SPEED_X * (-1f),
                        rb.velocity.y,
                        rb.velocity.z);
                }
            }

            //Playerを追いかける
            if (animState == (int)State.Chase)
            {
                rb.velocity = new Vector3(
                    ChaseDirection.x * CHASE_SPEED,
                    rb.velocity.y,
                    ChaseDirection.z * CHASE_SPEED); 
            }

            //攻撃の溜め or 攻撃中はしっかり止まる
            if ((animState == (int)State.Attack01Charge) || (animState == (int)State.Attack01))
            {
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            }

        }

        //疑似的に重力を設定
        void Gravity()
        {
            Vector3 LocalGravity = new Vector3(0f,-10f,0f);
            rb.AddForce(LocalGravity, ForceMode.Acceleration); 
        }

        //アニメーターを更新
        void Animator()
        {
            Anim.SetInteger("AnimState", animState);
        }

    }//class Enemy01Controller
}//namespace Assets.Scripts.Enemy01s