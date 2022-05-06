namespace Assets.Scripts.Audio
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AudioManager : MonoBehaviour
    {
        //�V���O���g���֘A
        public static AudioManager instance = null;

        //BGM�ԍ�
        public enum BgmNum
        {
            Stage1
        }

        //SE�ԍ�
        public enum SeNum
        {
            Button,
            Explosion,
            FireBallGenerate,
            GameOver,
            Goal,
            HitAttackPoint,
            Jump
        }

        //Audio�֘A
        AudioSource bgmAudioSource;
        AudioSource seAudioSource;

        AudioClip bgmClip;
        AudioClip seClip;

        //�C���X�y�N�^�[��Ŋ��蓖�Ă�
        [Header("Stage1��BGM")] public AudioClip BgmStage1;
        [Header("Button�N���b�N����SE")] public AudioClip Button;
        [Header("��������SE")] public AudioClip Explosion;
        [Header("�t�@�C�A�{�[����������SE")] public AudioClip FireBallGenerate;
        [Header("�Q�[���I�[�o�[����SE")] public AudioClip GameOver;
        [Header("�S�[������SE")] public AudioClip Goal;
        [Header("�Ō���������������SE")] public AudioClip HitAttackPoint;
        [Header("�W�����v��SE")] public AudioClip Jump;

        //�V���O���g���֘A
        private void Awake()
        {
            if (instance == null)
            {
                instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            //Audio�֘A
            bgmAudioSource = this.transform.Find("BGM").GetComponent<AudioSource>();
            seAudioSource = this.transform.Find("SE").GetComponent<AudioSource>();
        }

        /// <summary>
        /// BGM��炷
        /// </summary>
        public void PlayBGM(int clipNum)
        {
            switch (clipNum) {
                case (int)BgmNum.Stage1:
                    bgmClip = BgmStage1;
                    break;
                default:
                    bgmClip = null;
                    break;
            }

            if (bgmAudioSource != null)
            {
                bgmAudioSource.clip = bgmClip;
                bgmAudioSource.Play();
            }
            else
            {
                Debug.Log("BGM�̃I�[�f�B�I�\�[�X���ݒ肳��Ă��܂���");
            }
        }

        /// <summary>
        /// BGM���~�߂�
        /// </summary>
        public void StopBGM()
        {
            bgmAudioSource.Stop();
        }

        /// <summary>
        /// SE��炷
        /// </summary>
        public void PlaySE(int ClipNum)
        {
            switch (ClipNum)
            {
                case (int)SeNum.Button:
                    seClip = Button;
                    break;
                case (int)SeNum.Explosion:
                    seClip = Explosion;
                    break;
                case (int)SeNum.FireBallGenerate:
                    seClip = FireBallGenerate;
                    break;
                case (int)SeNum.GameOver:
                    seClip = GameOver;
                    break;
                case (int)SeNum.Goal:
                    seClip = Goal;
                    break;
                case (int)SeNum.HitAttackPoint:
                    seClip = HitAttackPoint;
                    break;
                case (int)SeNum.Jump:
                    seClip = Jump;
                    break;
                default:
                    seClip = null;
                    break;
            }

            if (seAudioSource != null)
            {
                seAudioSource.PlayOneShot(seClip);
            }
            else
            {
                Debug.Log("SE�̃I�[�f�B�I�\�[�X���ݒ肳��Ă��܂���");
            }
        }


    }
}