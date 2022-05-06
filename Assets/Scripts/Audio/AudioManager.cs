namespace Assets.Scripts.Audio
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class AudioManager : MonoBehaviour
    {
        //シングルトン関連
        public static AudioManager instance = null;

        //BGM番号
        public enum BgmNum
        {
            Stage1
        }

        //SE番号
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

        //Audio関連
        AudioSource bgmAudioSource;
        AudioSource seAudioSource;

        AudioClip bgmClip;
        AudioClip seClip;

        //インスペクター上で割り当てる
        [Header("Stage1のBGM")] public AudioClip BgmStage1;
        [Header("Buttonクリック時のSE")] public AudioClip Button;
        [Header("爆発時のSE")] public AudioClip Explosion;
        [Header("ファイアボール生成時のSE")] public AudioClip FireBallGenerate;
        [Header("ゲームオーバー時のSE")] public AudioClip GameOver;
        [Header("ゴール時のSE")] public AudioClip Goal;
        [Header("打撃が当たった時のSE")] public AudioClip HitAttackPoint;
        [Header("ジャンプのSE")] public AudioClip Jump;

        //シングルトン関連
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
            //Audio関連
            bgmAudioSource = this.transform.Find("BGM").GetComponent<AudioSource>();
            seAudioSource = this.transform.Find("SE").GetComponent<AudioSource>();
        }

        /// <summary>
        /// BGMを鳴らす
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
                Debug.Log("BGMのオーディオソースが設定されていません");
            }
        }

        /// <summary>
        /// BGMを止める
        /// </summary>
        public void StopBGM()
        {
            bgmAudioSource.Stop();
        }

        /// <summary>
        /// SEを鳴らす
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
                Debug.Log("SEのオーディオソースが設定されていません");
            }
        }


    }
}