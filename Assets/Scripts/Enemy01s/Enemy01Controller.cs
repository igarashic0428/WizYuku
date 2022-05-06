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
        //Skeleton�I�u�W�F�N�g��Animator�R���|�[�l���g�ɐݒ肵��AnimState�̐��l�ƍ��킹�邱��
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

        //==============================================�萔��`==============================================

        //NotFindSoIdle,NotFindSoWalk�֘A
        float NOT_FIND_SO_IDLE_COUNT_TIME = 3f; //�����~�܂鎞��
        float NOT_FIND_SO_WALK_COUNT_TIME = 3f; //��������
        float WALK_SPEED_X = 3f;

        //FindingPlayer�֘A
        float ENEMY_HEAD_POS_Y = 4.5f; //ENEMY�̓��Ɉʒu
        float PLAYER_HEAD_POS_Y = 3f; //Player�̓��Ɉʒu

        float MAX_DISTANCE = 7f; //Player��T�����C�Ǝ˂̍ő勗��

        float FIND_TIME = 1.5f; //Player�����������̐��䎞��

        float CHASE_SPEED = 6f; //�v���C���[��ǂ������鎞�̈ړ����x

        //Chase�֘A
        float CHASE_DESTINATION_CORRECTION_X = 2f; //�ǂ������鎞�̖ړI�n�␳�l(X��)
        float ATTACK01_ABLE_DISTANCE_X = 2f; //�U���ɓ���ԍ���(X��)
        float ATTACK01_ABLE_DISTANCE_Z = 0.5f; //�U���ɓ���ԍ���(Z��)

        //Attack01�֘A
        float ATTACK01_CHARGE_TIME = 0.5f; //���߃A�j���[�V�����̎���
        float ATTACK01_TIME = 0.5f; //�U���A�j���[�V�����̎���

        //Recoil�֘A
        float RECOIL_TIME = 1f; //�����A�j���[�V�����̎���

        //ChaseToNotFoundCheck�֘A
        float CHASE_TO_NOT_FOUND_TIME = 3; //�������m�莞��

        //NotFound�֘A
        float NOT_FOUND_TIME = 3f; //�v���C���[�������������̃A�j���[�V�����̎���

        //Die�֘A
        float DIE_TIME = 1f; //Die�A�j���[�V�����̎���

        //==============================================�ϐ���`==============================================
        PlayerController PlayerController;

        public int animState; //�A�j���[�V�������

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

        //�������Z�֘A
        private Rigidbody rb; //�������Z�p

        //�C���X�^���X����
        GameObject objSkeleton; //�q�I�u�W�F�N�g�uSkeleton�v���擾

        private Animator Anim; //Animator Component �i�[�p

        /*======================================================*/
        /*====���J�֐�==========================================*/
        /*======================================================*/

        //�ʒu�擾
        public Vector3 GetPos()
        {
            return this.transform.position;
        }

        //Hp�擾
        public float GetHp()
        {
            return Enemy01Para.hp;
        }

        /*==============================================================*/
        /*====Unity���璼�ڌĂ΂��֐�=================================*/
        /*==============================================================*/

        // Start is called before the first frame update
        void Start()
        {
            PlayerController = GameObject.Find("Player").GetComponent<PlayerController>();

            Enemy01ReceiveDamage = this.gameObject.GetComponent<Enemy01ReceiveDamage>();

            animState = (int)State.NotFindSoIdle;

            rb = GetComponent<Rigidbody>(); //�������Z�p

            objSkeleton = transform.Find("Skeleton").gameObject; //Skeleton��gameObject�擾
            Anim = objSkeleton.GetComponent<Animator>();         //Skeleton��Animator�R���|�[�l���g�擾

            Enemy01Para.hp = (int)100; //����HP
            Enemy01Para.power = (int)20; //�U����
        }

        // Update is called once per frame
        //void Update() { }

        void FixedUpdate()
        {
            //====================================================
            //�����̐���֘A========================================

            //Idle, Walk
            NotFindSoIdle(); //Player��������Ȃ����ߗ����~�܂��Ԃ𐧌�
            NotFindSoWalk(); //Player��������Ȃ����ߕ�����Ԃ𐧌�
            FindingPlayer(); //Player��T����������Find�ɑJ�ڂ���

            //Find
            Find(); //��������Ԃ̐���

            //Chase
            ChasePlayer(); //Player��ǂ������鐧��
            ChaseToAttack01Charge(); //Player�̖T�܂ŗ�����Attack01Charge�ɑJ�ڂ���

            //Attack01Charge
            Attack01Charge(); //Attack01Charge�Ȃ��Attack01�̗��ߐ���

            //Attack01
            Attack01(); //�U�����鐧��

            //Recoil
            Recoil();//�����̐���

            //Chase
            ChaseToNotFoundCheck(); //Player������������NotFindSoWalk�ɑJ�ڂ���

            //NotFound
            NotFound(); //Player�������������̐���

            //��������
            FallCheck();

            //�v���C���[���S�[������������
            //PlayerGoalCheck();

            DieCheck(); //���S����

            //Die
            Die();

            //��U������
            ReceiveDamageCheck();

            //�������Z�֘A
            PhysicsUpdate();//�����������X�V

            //�^���I�ɏd�͂�ݒ�
            Gravity();

            //�A�j���[�^�[�֘A
            Animator(); //�A�j���[�^�[���X�V

        }


        /*======================================================*/
        /*====�����֐�==========================================*/
        /*======================================================*/

        //�����~�܂��Ԃ𐧌�
        void NotFindSoIdle()
        {
            //==========================================================================================
            //�����~�܂��Ԃ𐧌�
            if (animState == (int)State.NotFindSoIdle)
            {
                //������ԂɂȂ�������͊J�n���Ԃ�ۑ�
                if (isNotFindSoIdle == false)
                {
                    isNotFindSoIdle = true;
                    stateStartTime = Time.time;
                }

                //���΂炭�����~�܂��Ԃ𑱂����甽�΂������ĕ�����Ԃɐ؂�ւ���
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

        //������Ԃ𐧌�
        void NotFindSoWalk()
        {
            //==========================================================================================
            //������Ԃ𐧌�
            if (animState == (int)State.NotFindSoWalk)
            {
                //������ԂɂȂ�������͊J�n���Ԃ�ۑ�
                if (isNotFindSoWalk == false)
                {
                    isNotFindSoWalk = true;
                    stateStartTime = Time.time;
                }

                //���΂炭������Ԃ𑱂����痧���~�܂��Ԃɐ؂�ւ���
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

        //Player��T������
        void FindingPlayer()
        {
            //Player���������u�Ԃ̐���
            if (animState == (int)State.NotFindSoIdle || animState == (int)State.NotFindSoWalk) {

                //====���C�p�ɃG�l�~�[����v���C���[�ւ̕������Z�o====
                Vector3 PlayerPos;
                Vector3 EnemyPos;
                Vector3 Direction;

                //Enemy�̈ʒu���Z�o
                EnemyPos = this.transform.position;
                EnemyPos.y = EnemyPos.y + ENEMY_HEAD_POS_Y;

                //Player�̈ʒu���Z�o
                PlayerPos = PlayerController.GetPos();
                PlayerPos.y = PlayerPos.y + PLAYER_HEAD_POS_Y;
                Direction = (PlayerPos - EnemyPos).normalized;

                //====EnemyPos����PlayerPos�̕����փ��C���΂�====
                RaycastHit hit;
                bool hasHit = Physics.Raycast(EnemyPos, Direction, out hit, MAX_DISTANCE); //Ray�𐶐�

                Ray ray = new Ray(EnemyPos, Direction); //�f�o�b�O�p
                Debug.DrawRay(ray.origin, ray.direction * MAX_DISTANCE, Color.red); //�f�o�b�O�p

                //====���C���v���C���[�ɏՓ˂�����Find�ɑJ�ڂ���====
                if (hasHit) //����Ray�𓊎˂��ĉ��炩�̃R���C�_�[�ɏՓ˂�����
                {
                    GameObject obj = hit.collider.gameObject; // �Փ˂�������I�u�W�F�N�g���擾

                    if (obj.tag == "TagPlayer") //���C���Փ˂�������I�u�W�F�N�g�̃^�O��TagPlayer�Ȃ�
                    {
                        if ( ( (0f <= objSkeleton.transform.localScale.x) &&
                               (0f <= (obj.transform.position.x - this.transform.position.x) ) ) || //�E�������v���C���[���E�ɂ���Ȃ��
                             ( (0f > objSkeleton.transform.localScale.x) &&
                               (0f > (obj.transform.position.x - this.transform.position.x)  ) )) //���������v���C���[�����ɂ���Ȃ��
                        {
                            Debug.Log("Enemy��" + obj.name + "�������܂���"); //�R���\�[���ɕ\��
                            animState = (int)State.Find; //Find�ɑJ�ڂ���
                        }
                    }
                }
            }
        }

        //��������Ԃ̐���
        void Find()
        {
            //====���������̐���====
            if (animState == (int)State.Find)
            {
                //��������ԂɂȂ�������͊J�n���Ԃ�ۑ�
                if (isFind == false)
                {
                    isFind = true;
                    stateStartTime = Time.time;

                    //�ueye L open�v�ueye R open�v���A�N�e�B�u�ɂ���
                    string str_eye_R_open = "Skeleton/PSB_Character_SkeletonB/eye_R";//���g�́ueye R open�v�I�u�W�F�N�g���擾
                    string str_eye_L_open = "Skeleton/PSB_Character_SkeletonB/eye_L";//���g�́ueye L open�v�I�u�W�F�N�g���擾
                    Transform EyeROpen = this.transform.Find(str_eye_R_open);//���g�́ueye R open�v�I�u�W�F�N�g���擾
                    Transform EyeLOpen = this.transform.Find(str_eye_L_open);//���g�́ueye L open�v�I�u�W�F�N�g���擾
                    EyeROpen.gameObject.SetActive(true);
                    EyeLOpen.gameObject.SetActive(true);
                }
                //��������Ԃ�FIND_TIME�b�o�߂�����Chase�ɑJ��
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

        //�v���C���[��ǂ������鐧��
        void ChasePlayer()
        {
            //Player���������u�Ԃ̐���
            if (animState == (int)State.Chase)
            {
                //====�G�l�~�[����v���C���[�ւ̕������Z�o====
                Vector3 PlayerPos;
                Vector3 EnemyPos;
                Vector3 Destination;
                Vector3 d;
                EnemyPos = this.transform.position; //Enemy�̈ʒu���擾
                PlayerPos = PlayerController.GetPos(); //Player�̈ʒu���擾
                Destination = PlayerPos;
                if ((Destination.x - EnemyPos.x) <= 0f)//�v���C���[�̎�O��ڎw���B�������Ȃ�Ώ����E�֕␳�A�E�����Ȃ�Ώ������֕␳
                {
                    Destination.x += CHASE_DESTINATION_CORRECTION_X;
                }
                else
                {
                    Destination.x -= CHASE_DESTINATION_CORRECTION_X;
                }
                d = (Destination - EnemyPos); //�������Z�o
                d.y = 0f; //�󒆂ւ͒ǂ������Ȃ�
                ChaseDirection = d.normalized; //�������Z�o

                //�X�v���C�g�̌�����ݒ�
                if (0f<= (PlayerPos.x - EnemyPos.x)) //�v���C���[���E�ɂ���Ȃ��
                {
                    if (objSkeleton.transform.localScale.x < 0f) //���������Ă����甽�]����
                    {
                        objSkeleton.transform.localScale = new Vector3(
                            objSkeleton.transform.localScale.x * -1f,
                            objSkeleton.transform.localScale.y,
                            objSkeleton.transform.localScale.z);
                    }
                }
                else //�v���C���[�����ɂ���Ȃ��
                {
                    if (0f < objSkeleton.transform.localScale.x) //�E�������Ă����甽�]����
                    {
                        objSkeleton.transform.localScale = new Vector3(
                            objSkeleton.transform.localScale.x * -1f,
                            objSkeleton.transform.localScale.y,
                            objSkeleton.transform.localScale.z);
                    }
                }
            }
        }

        //Player�̖T�܂ŗ�����Attack01Charge�ɑJ��
        void ChaseToAttack01Charge()
        {
            if (animState == (int)State.Chase)
            {
                //�G�l�~�[�ƃv���C���[�̋������Z�o
                float DistanceX;
                float DistanceZ;
                DistanceX = Mathf.Abs(this.transform.position.x - PlayerController.GetPos().x);
                DistanceZ = Mathf.Abs(this.transform.position.z - PlayerController.GetPos().z);

                //�T�܂ŗ��Ă�����
                if ( (DistanceX < ATTACK01_ABLE_DISTANCE_X) && (DistanceZ < ATTACK01_ABLE_DISTANCE_Z) )
                {
                    //Attack01Charge�ɑJ��
                    animState = (int)State.Attack01Charge;
                }
            }
        }

        //Attack01Charge�Ȃ��Attack01�̗��ߐ���
        void Attack01Charge()
        {
            //Attack01Charge��ԂȂ�Ώ��莞�Ԍo�������Attack01��ԂɈڍs����
            if (animState == (int)State.Attack01Charge)
            {
                //Attack01Charge��ԂɂȂ�������͊J�n���Ԃ�ۑ�
                if (isAttack01Charge == false)
                {
                    isAttack01Charge = true;
                    stateStartTime = Time.time;
                }

                //���莞�Ԍo�������Attack01��ԂɈڍs����
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

        //Attack01�̐���
        void Attack01()
        {
            //Attack01��ԂȂ��
            if (animState == (int)State.Attack01)
            {
                if (isAttack01 == false) //Attack01Charge��ԂɂȂ�������Ȃ��
                {
                    isAttack01 = true;
                    stateStartTime = Time.time; //�J�n���Ԃ�ۑ�����

                    //Attack01�̃I�u�W�F�N�g�𐶐�����===========================================
                    const string str_bone_AttackPoint = "Skeleton/PSB_Character_SkeletonB/bone_1_base/bone_2_body/bone_5_ArmUpL/bone_6_ArmDownL/bone_7_HandL";
                    Transform ParentTransform = this.transform.Find(str_bone_AttackPoint); //���g�́ubone_9�v�I�u�W�F�N�g���擾
                    GameObject Enemy01Attack01Prefab = (GameObject)Resources.Load("Enemy01Attack01"); //Enemy01Attack01�v���n�u���擾 ���K��Resources�t�H���_�Ɋi�[���邱��
                    GameObject AttackPoint = Instantiate(Enemy01Attack01Prefab, ParentTransform); //�ubone_9�v�I�u�W�F�N�g�̎q�Ƃ���Enemy01Attack01�v���n�u����I�u�W�F�N�g��������

                    //���������I�u�W�F�N�g�̃R���|�[�l���g���擾
                    Enemy01Attack01Controller Enemy01Attack01Controller = AttackPoint.GetComponent<Enemy01Attack01Controller>();
                    Enemy01Attack01Controller.SetPower(Enemy01Para.power);
                    Enemy01Attack01Controller.SetTag("TagEnemyAttack"); //�^�O���Z�b�g����
                }

                //���莞�Ԍo�������Attack01��ԂɈڍs����
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

        //�����̐���
        void Recoil()
        {
            //Recoil��ԂȂ�Ώ��莞�Ԍo�������Chase��ԂɈڍs����
            if (animState == (int)State.Recoil)
            {
                //Attack01Charge��ԂɂȂ�������͊J�n���Ԃ�ۑ�
                if (isRecoil == false)
                {
                    isRecoil = true;
                    stateStartTime = Time.time;
                }

                //���莞�Ԍo�������Attack01��ԂɈڍs����
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

        //�v���C���[�������������`�F�b�N
        void ChaseToNotFoundCheck()
        {
            if (animState == (int)State.Chase)
            {
                bool isNotFoundPlayer=false;

                //�v���C���[�������������m�F
                //�������Ă�������Ƀv���C���[�����Ȃ�
                if (((0f <= (PlayerController.GetPos().x - this.transform.position.x)) &&
                      (objSkeleton.transform.localScale.x < 0)) || //�v���C���[���E�ɂ��� ���� �G�l�~�[�͍��������Ă���
                     ((0f > (PlayerController.GetPos().x - this.transform.position.x)) &&
                      (objSkeleton.transform.localScale.x > 0)))   //�v���C���[�����ɂ��� ���� �G�l�~�[�͉E�������Ă���
                {
                    isNotFoundPlayer = true; //�v���C���[����������
                }
                else
                {
                    //���C���v���C���[�ɏƎ˂��ăv���C���[�ɓ����邩�m�F(FindingPlayer�̃��W�b�N���p)
                    //====���C�p�ɃG�l�~�[����v���C���[�ւ̕������Z�o====
                    Vector3 PlayerPos;
                    Vector3 EnemyPos;
                    Vector3 Direction;

                    //Enemy�̈ʒu���Z�o
                    EnemyPos = this.transform.position;
                    EnemyPos.y = EnemyPos.y + ENEMY_HEAD_POS_Y;

                    //Player�̈ʒu���Z�o
                    PlayerPos = PlayerController.GetPos();
                    PlayerPos.y = PlayerPos.y + PLAYER_HEAD_POS_Y;
                    Direction = (PlayerPos - EnemyPos).normalized;

                    //====EnemyPos����PlayerPos�̕����փ��C���΂�====
                    RaycastHit hit;
                    bool hasHit = Physics.Raycast(EnemyPos, Direction, out hit, MAX_DISTANCE); //Ray�𐶐�

                    Ray ray = new Ray(EnemyPos, Direction); //�f�o�b�O�p
                    Debug.DrawRay(ray.origin, ray.direction * MAX_DISTANCE, Color.red, 1f); //�f�o�b�O�p

                    //====���C���v���C���[�ɏՓ˂�����Find�ɑJ�ڂ���====
                    if (hasHit) //����Ray�𓊎˂��ĉ��炩�̃R���C�_�[�ɏՓ˂�����
                    {
                        GameObject obj = hit.collider.gameObject; // �Փ˂�������I�u�W�F�N�g���擾

                        if (obj.tag != "TagPlayer") //���C���Փ˂�������I�u�W�F�N�g�̃^�O��TagPlayer�ł͂Ȃ��Ȃ�
                        {
                            isNotFoundPlayer = true; //�v���C���[����������
                        }
                    }
                }

                //�v���C���[������������Ԃ����莞�ԑ������Ȃ��NotFound��ԂɈڍs����
                if (isNotFoundPlayer == true)
                {
                    //Attack01Charge��ԂɂȂ�������͊J�n���Ԃ�ۑ�
                    if (isChaseToNotFound == false)
                    {
                        isChaseToNotFound = true;
                        stateStartTime = Time.time;
                    }

                    //���莞�Ԍo�������Attack01��ԂɈڍs����
                    float t = Time.time;
                    if (CHASE_TO_NOT_FOUND_TIME < (t - stateStartTime))
                    {
                        animState = (int)State.NotFound; //NotFound�ɑJ��
                    }
                }
                else
                {
                    isChaseToNotFound = false;
                }
            }
        }

        //Player�������������̐���
        void NotFound() {
            //NotFound��ԂȂ�Ώ��莞�Ԍo�������NotFindSoWalk��ԂɈڍs����
            if (animState == (int)State.NotFound)
            {
                //Attack01Charge��ԂɂȂ�������͊J�n���Ԃ�ۑ�
                if (isNotFound == false)
                {
                    isNotFound = true;
                    stateStartTime = Time.time;

                    //�ueye L open�v�ueye R open�v���A�N�e�B�u�ɂ���
                    string str_eye_R_open = "Skeleton/PSB_Character_SkeletonB/eye_R";//���g�́ueye_R�v�I�u�W�F�N�g���擾
                    string str_eye_L_open = "Skeleton/PSB_Character_SkeletonB/eye_L";//���g�́ueye_L�v�I�u�W�F�N�g���擾
                    Transform EyeROpen = transform.Find(str_eye_R_open);//���g�́ueye R open�v�I�u�W�F�N�g���擾
                    Transform EyeLOpen = transform.Find(str_eye_L_open);//���g�́ueye L open�v�I�u�W�F�N�g���擾
                    EyeROpen.gameObject.SetActive(false);
                    EyeLOpen.gameObject.SetActive(false);
                }

                //���莞�Ԍo�������Attack01��ԂɈڍs����
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

        //��������
        void FallCheck()
        {
            if (this.transform.position.y < -12.0f) //y����-12�ȉ��Ȃ�Η��������Ɣ��f����
            {
                //���S�p�̃p�[�e�B�N������
                GameObject Prefab = (GameObject)Resources.Load("ParticleSoul"); //�v���n�u���擾 ���K��Resources�t�H���_�Ɋi�[���邱��
                GameObject ParticleSoul = Instantiate(Prefab); //�v���n�u����I�u�W�F�N�g��������
                Vector3 pos = this.transform.position;
                pos.y += 3; //�Ⴗ���ăJ�����Ɏʂ�܂łɃ��O������̂Ńp�[�e�B�N���̊J�n�ʒu�����������ݒ�
                ParticleSoul.transform.position = pos;

                //Stage1Manager�Ɏ��񂾂��Ƃ�`����
                Stage1Manager IStage1Manager = GameObject.Find("Stage1Manager").GetComponent<Stage1Manager>();
                IStage1Manager.SetEnemyDeadCountInc();

                //���g��j������
                Destroy(this.gameObject);
            }
        }

        //���S����
        void DieCheck()
        {
            if (animState != (int)State.Die) //Die��Ԃł͂Ȃ��Ȃ��
            {
                    if (Enemy01Para.hp <= 0) //Hp��0�ȉ��Ȃ��
                    {
                        animState = (int)State.Die; //Die��Ԃɂ���
                    }
            }
        }

        //���S���̋���
        void Die()
        {
            //Die��ԂȂ��
            if (animState == (int)State.Die)
            {
                //Die��ԂɂȂ�������͊J�n���Ԃ�ۑ�
                if (isDie == false)
                {
                    isDie = true;
                    stateStartTime = Time.time;

                    //�ueye_L�v�ueye_R�v���A�N�e�B�u�ɂ���
                    string str_eye_R_open = "Skeleton/PSB_Character_SkeletonB/eye_R";//���g�́ueye R open�v�I�u�W�F�N�g���擾
                    string str_eye_L_open = "Skeleton/PSB_Character_SkeletonB/eye_L";//���g�́ueye L open�v�I�u�W�F�N�g���擾
                    Transform EyeROpen = this.transform.Find(str_eye_R_open);//���g�́ueye R open�v�I�u�W�F�N�g���擾
                    Transform EyeLOpen = this.transform.Find(str_eye_L_open);//���g�́ueye L open�v�I�u�W�F�N�g���擾
                    EyeROpen.gameObject.SetActive(false);
                    EyeLOpen.gameObject.SetActive(false);

                    //Stage1Manager�Ɏ��񂾂��Ƃ�`����
                    Stage1Manager IStage1Manager = GameObject.Find("Stage1Manager").GetComponent<Stage1Manager>();
                    IStage1Manager.SetEnemyDeadCountInc();
                }

                //���莞�Ԍo������Ɏ��g��j������
                float t = Time.time;
                if (DIE_TIME < (t - stateStartTime))
                {
                    if (isDieParticle == false)
                    {
                        //���S���̉��o
                        GameObject Prefab = (GameObject)Resources.Load("ParticleSoul"); //�v���n�u���擾 ���K��Resources�t�H���_�Ɋi�[���邱��
                        GameObject ParticleSoul = Instantiate(Prefab); //�v���n�u����I�u�W�F�N�g��������
                        ParticleSoul.transform.position = this.transform.position;

                        //1�b�㎩�g��j������
                        StartCoroutine("OneSecLateDestroy"); //�R���[�`���̊֐�OneSecLateDestroy()���R�[������

                        isDieParticle = true;
                    }
                }
            }
            else
            {
                isDie = false;
            }
        }

        //1�b�㎩�g��j������
        IEnumerator OneSecLateDestroy()
        {
            yield return new WaitForSeconds(1.0f); //���̃R���[�`���֐����ł��̍s���牺�̏�����1.0�b�҂�
            GameObject.Destroy(this.gameObject); //���g��j������
        }

        //�_���[�W���󂯂�
        void ReceiveDamageCheck()
        {
            //��U������𓾂�
            isReceiveDamage = Enemy01ReceiveDamage.GetIsReceiveDamage();

            if (isReceiveDamage == true) //�G����̍U�����󂯂Ă���Ȃ��
            {
                //�_���[�W���󂯂�
                int d = Enemy01ReceiveDamage.GetDamege();
                Enemy01Para.hp -= d;
                
                //Debug.Log("�U�����󂯂܂���");
            }
        }

        //���������̍X�V
        void PhysicsUpdate()
        {
            //Player��������Ȃ����痧���~�܂��Ԃ𐧌�
            if (animState == (int)State.NotFindSoIdle)
            {
                //XZ���ʈړ��̑��x��0�ɂ���B
                rb.velocity = new Vector3(0.0f, rb.velocity.y, 0.0f);
            }

            //Player��������Ȃ����������Ԃ𐧌�
            if (animState == (int)State.NotFindSoWalk)
            {
                //XZ���ʈړ��̑��x���X�V����B
                if (0 < objSkeleton.transform.localScale.x) //�E�����Ȃ��
                {
                    //�E�ֈړ�
                    rb.velocity = new Vector3(
                        WALK_SPEED_X,
                        rb.velocity.y,
                        rb.velocity.z);
                }
                else //�������Ȃ��
                {
                    //���ֈړ�
                    rb.velocity = new Vector3(
                        WALK_SPEED_X * (-1f),
                        rb.velocity.y,
                        rb.velocity.z);
                }
            }

            //Player��ǂ�������
            if (animState == (int)State.Chase)
            {
                rb.velocity = new Vector3(
                    ChaseDirection.x * CHASE_SPEED,
                    rb.velocity.y,
                    ChaseDirection.z * CHASE_SPEED); 
            }

            //�U���̗��� or �U�����͂�������~�܂�
            if ((animState == (int)State.Attack01Charge) || (animState == (int)State.Attack01))
            {
                rb.velocity = new Vector3(0f, rb.velocity.y, 0f);
            }

        }

        //�^���I�ɏd�͂�ݒ�
        void Gravity()
        {
            Vector3 LocalGravity = new Vector3(0f,-10f,0f);
            rb.AddForce(LocalGravity, ForceMode.Acceleration); 
        }

        //�A�j���[�^�[���X�V
        void Animator()
        {
            Anim.SetInteger("AnimState", animState);
        }

    }//class Enemy01Controller
}//namespace Assets.Scripts.Enemy01s