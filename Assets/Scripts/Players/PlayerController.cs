
//キャラクターの移動方法切替スイッチ
#define UNUSE_NAVMESHAGENT
#if UNUSE_NAVMESHAGENT
#else
#endif

namespace Assets.Scripts.Players
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    using Assets.Scripts.Inputs;
    using Assets.Scripts.Players.PlayerAttacks;

    using Assets.Scripts.Goals;

    using Assets.Scripts.Audio;

    public class PlayerController : MonoBehaviour
    {
        //==============================================struct==============================================
        struct Parameters
        {
            public int hp;
            public int power;
        }
        Parameters PlayerPara;

        //==============================================enum==============================================
        enum State
        {
            Idle,
            Run,
            Attack01,
            Attack02,
            Charge,
            JumpUp,
            JumpDown,
            Fall,
            Hurt,
            HurtEnd,
            Die,
            Goal
        }

        //==============================================定数定義==============================================
        //移動関連
        const float MOVE_SPEED_X = 5.0f;            //移動速度X軸
        const float MOVE_SPEED_Z = 6.0f;            //移動速度Z軸
        const float RUN_END_CONFIRM_DIFF = 0.3f;    //移動終了確定位置差

        //ジャンプ関連
        const float RUN_JUMP_ADD_VELOCITY_X = 4f;   //走る→ジャンプのジャンプ力加算値
        const float JUMP_INIT_VELOCITY_X = 4f;      //ジャンプ初期速度X軸
        const float JUMP_INIT_VELOCITY_Y = 4f;      //ジャンプ初期速度Y軸
        const float JUMP_VELOCITY_DEC_X = 0.05f;    //ジャンプ速度減少値X軸
        const float JUMP_VELOCITY_DEC_Y = 0.1f;     //ジャンプ速度減少値Y軸

        //フィールド上の移動限界判定
        const float MOVE_FIELD_GUARD_X_MIN = -8f;
        const float MOVE_FIELD_GUARD_X_MAX = 188f;
        const float MOVE_FIELD_GUARD_Z_MIN = -10f;
        const float MOVE_FIELD_GUARD_Z_MAX = 17f;
        const float MOVE_FIELD_GUARD_POWER = 1f;

        //Hurt関連
        const float HURT_TIME = 0.25f;

        //Attack関連
        const float ATTACK01_TIME = 0.5f;

        //==============================================変数定義==============================================

        //==============================================外部インターフェース呼び出し==============================================
        InputController InputController;

        //プライベート変数

        //GroundCheck関連
        PlayerGroundCheck PlayerGroundCheck; //PlayerGroundCheckのインスタンス
        private bool isGround = false;

        //ReceiveDamage関連
        PlayerReceiveDamage PlayerReceiveDamage; //PlayerReceiveDamageのインスタンス
        private bool isReceiveDamage = false;

        int animState; //アニメーション状態
        int animStateOld; //アニメーション状態前回値

        private Rigidbody rb;
        private Animator Anim;

        private bool playerAlive = true; //Playerが生きているか判定用

        //InputCheck関連
        bool isInputMove; //Move入力
        bool isInputJump; //Jump入力
        bool isInputAttack; //Attack入力

        //Move関連
        Vector3 moveDestination; //行き先
        Vector3 moveNDirectionXZ; //正規化した方向(XZ平面)

        Vector3 jumpVec;
        float jumpVelocityX;//ジャンプ速度X
        float jumpVelocityY;//ジャンプ速度Y

        DestinationPointController DestinationPointController; //DestinationPointControllertのインスタンス

        //Hurt関連
        bool isHurt;
        float hurtStartTime;

        //Attack関連
        bool isAttack01;
        float attack01StartTime;

        //Inspectorから設定する
        [SerializeField] GameObject TargetRing;

        //GameOver関連
        bool isGameOver;

        //Goal関連
        bool isGoal;

        //==============================================関数定義==============================================

        /*==============================================================*/
        /*====公開関数==================================================*/
        /*==============================================================*/

        //位置取得
        public Vector3 GetPos()
        {
            return this.transform.position;
        }

        //Hp取得
        public int GetHp()
        {
            return PlayerPara.hp;
        }

        //名前を取得
        public string GetName()
        {
            return this.gameObject.name; //名前を取得
        }

        //GameOver判定を取得
        public bool GetIsGameOver()
        {
            return isGameOver; //名前を取得
        }

        //Update更新処理
        public void UpdatePlayer()
        {
            //Restart();
            if (playerAlive) //プレイヤーが生きている場合ならばこの処理を行う
            {
                InputCheck(); //入力を判定してプレイヤーの挙動を決める

                Move(); //行き先の取得
                Jump();
                Hurt();
                Attack(); //攻撃処理
                FallCheck(); //落下判定
                DieCheck(); //死亡判定
                GoalCheck(); //ゴール判定
            }
            UpdateAnimator();
        }

        /*=======================================================================*/
        /*====Unityから直接呼ばれる関数==========================================*/
        /*=======================================================================*/

        // Start is called before the first frame update
        void Start()
        {
            //インターフェースのインスタンス生成
            InputController = GameObject.Find("InputController").GetComponent<InputController>();

            //外部クラスのインスタンス生成
            PlayerGroundCheck = this.transform.Find("PlayerGroundCheck").GetComponent<PlayerGroundCheck>();
            PlayerReceiveDamage = this.gameObject.GetComponent<PlayerReceiveDamage>();

            //DestinationPointControllertのインスタンス
            DestinationPointController = GameObject.Find("DestinationPoint").GetComponent<DestinationPointController>();

            //=============================================

            animState = (int)State.Idle;
            animStateOld = (int)State.Idle;

            rb = GetComponent<Rigidbody>();

            //「Chara」のAnimatorコンポーネント取得
            GameObject Chara = transform.Find("Chara").gameObject;
            Anim = Chara.GetComponent<Animator>();

            PlayerPara.hp = (int)100; //初期HP
            PlayerPara.power = (int)20; //初期HP

        }

        void FixedUpdate()
        {
            if (playerAlive)
            { //プレイヤーが生きている場合ならばこの処理を行う
              //一定周期で更新したいもの、物理演算関連のものはこの関数内で制御する。

                //衝突判定
                GroundCheck();

                //被攻撃判定
                ReceiveDamageCheck();

                //================================================================
                //ジャンプ中の制御
                Jumping();
            }

            //================================================================
            //Rigidbodyのvelocity(速度)を更新する。
            UpdateVelocity();

            //疑似的に重力を設定
            Gravity();

            //フィールド上の移動限界ガード
            FieldGuard();
        }

        /*=====================================================================================*/
        /*====FixedUpdateからコールされる内部関数==============================================*/
        /*=====================================================================================*/

        //物理判定の更新毎に呼ぶ
        void GroundCheck()
        {
            //JumpDown中
            if (animState == (int)State.JumpDown)
            {
                //接地判定を得る(毎周期更新する)
                isGround = PlayerGroundCheck.GetIsGround();
                //接地している 
                if (isGround == true)
                {
                    Debug.Log("着地しました");
                    animState = (int)State.Idle;
                }
            }
        }

        //ダメージを受ける
        void ReceiveDamageCheck()
        {
            //被攻撃判定を得る
            isReceiveDamage = PlayerReceiveDamage.GetIsReceiveDamage();

            if (isReceiveDamage == true) //敵からの攻撃を受けているならば
            {
                animState = (int)State.Hurt; //Hurtに遷移する

                if ( 0<=(PlayerReceiveDamage.GetEnemyAttackPos().x - this.transform.position.x) ) //右から攻撃を受けたならば
                {
                    rb.AddForce(new Vector2(-5f, 1f), ForceMode.Impulse); //リジッドボディに左向きの衝撃を加える
                }
                else //左から攻撃を受けたならば
                {
                    rb.AddForce(new Vector2(5f, 1f), ForceMode.Impulse); //リジッドボディに右向きの衝撃を加える
                }

                //ダメージを受ける
                int d = PlayerReceiveDamage.GetDamege();
                PlayerPara.hp -= d;

                //打撃が当たった時のSEを鳴らす
                AudioManager.instance.PlaySE((int)AudioManager.SeNum.HitAttackPoint);

                //Debug.Log("敵から攻撃を受けました");
            }
        }

        //ジャンプ中の制御
        void Jumping()
        {
            //ジャンプ中の演算は固定周期で行いたい為、ここで行う。
            if (animState == (int)State.JumpUp)
            {
                //ジャンプ上昇中は毎周期ジャンプ速度を下げる
                jumpVelocityY -= JUMP_VELOCITY_DEC_Y;
                jumpVelocityX -= JUMP_VELOCITY_DEC_X;
                //ジャンプ上昇速度が0ならばジャンプ下降に遷移
                if (jumpVelocityY <= 0)
                {
                    Debug.Log("ジャンプ速度Yが0になりました");
                    animState = (int)State.JumpDown; //ジャンプ下降
                    jumpVelocityY = 0f;
                }
            }
            if (animState == (int)State.JumpDown)
            {
                //ジャンプ下降中はX軸のジャンプ速度を下げる
                jumpVelocityX -= JUMP_VELOCITY_DEC_X;
                if (jumpVelocityX <= 0f)
                {
                    Debug.Log("ジャンプ速度Xが0になりました");
                    jumpVelocityX = 0f;
                }
            }
        }

        //Rigidbodyのvelocity(速度)を更新する。
        void UpdateVelocity()
        {
            //※物理的な挙動は無視する為、
            //  Rigidbodyのvelocity(速度)を直接更新して移動やジャンプを行う。
            if (animState == (int)State.Idle)
            {
                //XZ平面移動の速度を0にする。
                rb.velocity = new Vector3(0.0f, rb.velocity.y, 0.0f);

                //DestinationPointを非アクティブにする
                DestinationPointController.SetInactive();
            }
            if (animState == (int)State.Run)
            {
                //XZ平面移動の速度を更新する。
                rb.velocity = new Vector3(
                    moveNDirectionXZ.x * MOVE_SPEED_X,
                    rb.velocity.y,
                    moveNDirectionXZ.z * MOVE_SPEED_Z);
            }
            if (animState == (int)State.JumpUp)
            {
                //ジャンプ上昇
                rb.velocity = new Vector3(
                    jumpVec.x * jumpVelocityX, //JumpVelocityXは初期値から段々下がる
                    jumpVec.y * jumpVelocityY, //JumpVelocityYは初期値から段々下がる
                    rb.velocity.z);

                //DestinationPointを非アクティブにする
                DestinationPointController.SetInactive();
            }
            if (animState == (int)State.JumpDown)
            {
                //ジャンプ下降
                rb.velocity = new Vector3(
                    jumpVec.x * jumpVelocityX, //JumpVelocityXは初期値から段々下がる
                    rb.velocity.y,
                    rb.velocity.z);

                //DestinationPointを非アクティブにする
                DestinationPointController.SetInactive();
            }
            if (animState == (int)State.Die)
            {
                //その場に留まるが、重力だけは有効とする
                rb.velocity = new Vector3(0.0f, rb.velocity.y, 0.0f);
            }

            if (animState == (int)State.Goal)
            {
                //その場に留まるが、重力だけは有効とする
                rb.velocity = new Vector3(0.0f, rb.velocity.y, 0.0f);
            }
        }

        //疑似的に重力を設定
        void Gravity()
        {
            Vector3 LocalGravity = new Vector3(0f, -10f, 0f);
            rb.AddForce(LocalGravity, ForceMode.Acceleration);
        }


        //フィールド上の移動限界ガード
        void FieldGuard()
        {
            //フィールド上の移動限界ガード 範囲外なら戻す方向に力を設定する
            if (transform.position.x < MOVE_FIELD_GUARD_X_MIN)
            {
                Debug.Log("移動限界です X_MIN");
                rb.velocity = new Vector3(MOVE_FIELD_GUARD_POWER, rb.velocity.y, rb.velocity.z);
            }
            if (MOVE_FIELD_GUARD_X_MAX < transform.position.x)
            {
                Debug.Log("移動限界です X_MAX");
                rb.velocity = new Vector3((-1f) * MOVE_FIELD_GUARD_POWER, rb.velocity.y, rb.velocity.z);
            }
            if (transform.position.z < MOVE_FIELD_GUARD_Z_MIN)
            {
                Debug.Log("移動限界です Z_MIN");
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, MOVE_FIELD_GUARD_POWER);
            }
            if (MOVE_FIELD_GUARD_Z_MAX < transform.position.z)
            {
                Debug.Log("移動限界です Z_MAX");
                rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, (-1f) * MOVE_FIELD_GUARD_POWER);
            }
        }


        /*=====================================================================================================*/
        /*====UpdatePlayerからコールされる内部関数=============================================================*/
        /*=====================================================================================================*/

        //入力を判定してプレイヤーの挙動を決める
        void InputCheck()
        {
            //1周期で一回しか処理されないように毎回初期化する
            isInputMove = false; //Move入力初期化
            isInputJump = false; //Jump入力初期化
            isInputAttack = false; //Attack入力初期化

            if (InputController.GetIsTap()) //タップ(左クリック)されたならば
            {
                string tmpstr = InputController.GetTapObjTag();
                if (tmpstr == "TagGround") //地面ならば
                {
                    isInputMove = true; //Move入力
                }

                if ( tmpstr == "TagEnemy") //エネミーならば
                {
                    isInputAttack = true; //Attack入力
                }
            }

            //上フリックまたは右クリックされたならばジャンプ入力
            if ((InputController.GetIsUpFlick()) || (InputController.GetInputisRightClick()))
            {
                isInputJump = true;
            }
        }


        //行き先の取得
        void Move()
        {
            //=========================================
            //タップされた瞬間の制御
            //=========================================
            if ((animState == (int)State.Idle) ||//Idel中であれば移動入力可能
                (animState == (int)State.Run))
            {
                if (isInputMove == true)
                {
                    //Debug.Log("移動します。");
                    animState = (int)State.Run; //移動する

                    //目的地を取得
                    moveDestination = InputController.GetTapPos3d();

                    //目的地がキャラクターより右ならば右を向く、左ならば左を向く
                    int d;
                    if (moveDestination.x - transform.position.x > 0)
                    { d = 1;/*右*/ }
                    else { d = -1;/*左*/ }
                    //子オブジェクト「Chara」に対し向き設定
                    GameObject Chara = transform.Find("Chara").gameObject;
                    Chara.transform.localScale = new Vector3(d, 1, 1);

                    //DestinationPoint目的地をセットしてアクティブにする
                    DestinationPointController.Setpos(moveDestination);
                    DestinationPointController.SetActive();
                }
            }

            //=========================================
            //移動中の制御
            //=========================================
            if (animState == (int)State.Run) {
                if (RUN_END_CONFIRM_DIFF < Vector3.Distance(transform.position, moveDestination)) //目的地との距離が大きければRun
                {
                    //XZ平面の移動方向を算出
                    Vector3 d = moveDestination - transform.position;
                    d.y = 0.0f;
                    moveNDirectionXZ = d.normalized; //XZ平面のベクトルで正規化(長さが1のベクトル化)
                }
                else //目的地との距離が僅かならIdle
                {
                    animState = (int)State.Idle; //立ち止まる
                }
            }
        }

        void Jump()
        {
            //ジャンプ許可条件
            if ((animState == (int)State.Idle) ||
                (animState == (int)State.Run))
            {
                //ジャンプ入力ならば
                if (isInputJump == true)
                {
                    Debug.Log("ジャンプします。");

                    //ジャンプのxz方向を算出
                    Vector3 vecxz = Vector3.zero;
                    //ターゲットポイントがアクティブならば
                    if (DestinationPointController.gameObject.activeSelf == true)
                    {
                        //ターゲットポイントへの正規ベクトルを取得
                        vecxz = DestinationPointController.transform.position - this.transform.position;
                        vecxz.y = 0f;
                        vecxz.Normalize();
                    }
                    else
                    {
                        //ターゲットポイントがアクティブならばゼロベクトル
                    }

                    //移動中かつジャンプ方向が一致しているならジャンプ初期速度加算値を設定
                    float vx = 0f;
                    if ((animState == (int)State.Run) &&
                        ((0f < moveNDirectionXZ.x) && (0f < vecxz.x)) ||
                        ((moveNDirectionXZ.x < 0f) && (vecxz.x < 0f)))
                    {
                        vx = RUN_JUMP_ADD_VELOCITY_X;
                    }
                    //プレイヤーの状態をジャンプ上昇中にしてジャンプ速度にジャンプ初期速度を設定
                    animState = (int)State.JumpUp;
                    jumpVelocityX = JUMP_INIT_VELOCITY_X + vx;
                    jumpVelocityY = JUMP_INIT_VELOCITY_Y;

                    //ジャンプ方向の算出
                    jumpVec = new Vector3(vecxz.x, 1f, vecxz.z);

                    //Charaの描画向き設定
                    //子オブジェクト「Chara」に対し向き設定
                    GameObject Chara = transform.Find("Chara").gameObject;
                    float d = 0;
                    if (jumpVec.x < 0)
                    {
                        d = -1;/*左*/
                    }
                    else if (jumpVec.x > 0)
                    {
                        d = 1;/*右*/
                    }
                    else
                    {
                        d = Chara.transform.localScale.x;//(JumpNVec.x == 0)の時は今の向きのままにする
                    }
                    Chara.transform.localScale = new Vector3(d, 1, 1);

                    //ジャンプのSEを鳴らす
                    AudioManager.instance.PlaySE((int)AudioManager.SeNum.Jump);
                }
            }
        }

        //Hurt状態の制御
        void Hurt()
        {
            //Hurt許可条件
            if ((animState == (int)State.Hurt)|| (animState == (int)State.HurtEnd))
            {
                //Hurt状態の処理
                //初回は開始時間を保存
                if (isHurt == false)
                {
                    isHurt = true;
                    hurtStartTime = Time.time;
                }

                //所定時間経った後にIdle状態に移行する
                float t = Time.time;
                if (HURT_TIME < (t - hurtStartTime))
                {
                    animState = (int)State.Idle;
                }
            }
            else
            {
                isHurt = false;
            }
        }

        //Attack制御
        void Attack()
        {
            if (isInputAttack == true)
            {
                animState = (int)State.Attack01;
            }

            //Attack許可条件
            if (animState == (int)State.Attack01)
            {
                //Attack01状態の処理
                //初回は攻撃関連の初期設定をセット
                if (isAttack01 == false)
                {
                    isAttack01 = true;
                    attack01StartTime = Time.time;//開始時間を保存

                    //タップしたGameObjectを取得
                    GameObject TapObj = InputController.GetTapGameObject();

                    //TargetRingにタップしたGameObjectをセット
                    TargetRing.SetActive(true);
                    TargetRingController TargetRingController = TargetRing.GetComponent<TargetRingController>();
                    TargetRingController.SetGameObject(TapObj);

                    //SetFireBallにタップしたGameObjectをセットする
                    StartCoroutine(GenerateFireBall(TapObj)); //コルーチンの関数SetFireBall()をコールする

                    //プレイヤーの向きを設定する
                    //TapObjがキャラクターより右ならば右を向く、左ならば左を向く
                    int d;
                    if (TapObj.transform.position.x - transform.position.x > 0)
                    { d = 1;/*右*/ }
                    else { d = -1;/*左*/ }
                    //子オブジェクト「Chara」に対し向き設定
                    GameObject Chara = this.transform.Find("Chara").gameObject;
                    Chara.transform.localScale = new Vector3(d, 1, 1);
                }

                //所定時間経った後にIdle状態に移行する
                float t = Time.time;
                if (ATTACK01_TIME < (t - attack01StartTime))
                {
                    animState = (int)State.Idle;
                }
            }
            else
            {
                isAttack01 = false;
            }
        }

        //コルーチンの関数定義
        IEnumerator GenerateFireBall(GameObject TapObj)
        {
            yield return new WaitForSeconds(0.25f); //このコルーチン関数内でこの行から下の処理はしての秒数待つ

            if (TapObj != null) {
                //ファイアボールを生成
                GameObject FireBallPrefab = (GameObject)Resources.Load("PlayerAttack01FireBall"); //プレハブを取得 ※必ずResourcesフォルダに格納すること
                GameObject FireBall = Instantiate(FireBallPrefab); //プレハブからオブジェクト生成する

                //生成したオブジェクトのコンポーネントを取得
                PlayerAttack01Controller PlayerAttack01Controller = FireBall.GetComponent<PlayerAttack01Controller>();
                Vector3 v = new Vector3(
                    this.transform.position.x,
                    this.transform.position.y + 5.0f,
                    this.transform.position.z);
                PlayerAttack01Controller.SetPos(v); //初期位置をセットする
                PlayerAttack01Controller.SetPower(PlayerPara.power); //初期位置をセットする
                PlayerAttack01Controller.SetTargetObj(TapObj); //TapObjをセットする
                PlayerAttack01Controller.SetTag("TagPlayerAttack"); //タグをセットする

                //ファイアボール生成時のSEを鳴らす
                AudioManager.instance.PlaySE((int)AudioManager.SeNum.FireBallGenerate);
            }
        }

        //落下判定
        void FallCheck()
        {
            if (isGameOver == false)
            {
                if (this.transform.position.y < -10.0f) //y軸が-10以下ならば落下したと判断する
                {
                    Vector3 pos = this.transform.position;
                    this.gameObject.SetActive(false);

                    isGameOver = true;

                    //ゲームオーバー時のSEを鳴らす
                    AudioManager.instance.PlaySE((int)AudioManager.SeNum.GameOver);
                }
            }
        }

        //死亡判定
        void DieCheck()
        {
            if (animState != (int)State.Die) //Die状態ではないならば
            {
                if (isGameOver == false) //ゲームオーバーではないならば
                {
                    if (PlayerPara.hp <= 0) //Hpが0以下ならば
                    {
                        animState = (int)State.Die; //Die状態にする

                        isGameOver = true; //ゲームオーバーにする
                        playerAlive = false;

                        //ゲームオーバー時のSEを鳴らす
                        AudioManager.instance.PlaySE((int)AudioManager.SeNum.GameOver);
                    }
                }
            }
        }

        //ゴール判定
        void GoalCheck()
        {
            if (animState != (int)State.Goal) //Goal状態ではないならば
            {
                if (isGoal == false) //ゲームオーバーではないならば
                {
                    //ゴールマネージャーから状態を取得

                    
                    if (GameObject.Find("Goal") != null)
                    {
                        GoalController IGoal = GameObject.Find("Goal").GetComponent<GoalController>();
                        isGoal = IGoal.GetIsGoal();
                    }
                }

                if (isGoal == true) {
                    animState = (int)State.Goal; //Goal状態にする

                    //DestinationPointを非アクティブにする
                    DestinationPointController.SetInactive();

                    //ゴール時のSEを鳴らす
                    AudioManager.instance.PlaySE((int)AudioManager.SeNum.Goal);
                }
            }
        }

        //アニメーターの更新
        void UpdateAnimator()
        {
            if (animState != animStateOld)//アニメーターへのセットは遷移の瞬間に1回だけ行う
            {
                Anim.SetInteger("AnimState", animState);
            }

            //前回値更新
            animStateOld = animState;
        }

    }
}
