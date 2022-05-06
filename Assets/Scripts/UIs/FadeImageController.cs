using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeImageController : MonoBehaviour
{
    [Header("�ŏ�����t�F�[�h�C�����������Ă��邩�ǂ���")] public bool isFirstFadeInComp;

    private Image fadeImg = null;

    //�t�F�[�h�C���֘A
    private int frameCount;
    private float fadeTimer;
    private bool isFadeIn;
    private bool isCompFadeIn = false;

    //�t�F�[�h�A�E�g�֘A
    private bool isFadeOut;
    private bool isCompFadeOut = false;

    /// <summary>
    /// �t�F�[�h�C�����J�n����
    /// </summary>
    public void StartFadeIn()
    {
        if (isFadeIn || isFadeOut)
        {
            return; //�t�F�[�h���͐V���Ƀt�F�[�h���J�n���Ȃ�
        }

        //�t�F�[�h�C�����J�n����ꍇ�̏����l�ݒ�
        isFadeIn = true;
        isCompFadeIn = false;
        fadeTimer = 0.0f;

        fadeImg.color = new Color(1, 1, 1, 1);
        fadeImg.fillAmount = 1;
        fadeImg.raycastTarget = true; //�t�F�[�h�摜�̓����蔻��I���ɂ��邱�ƂŃ{�^�����������Ȃ��悤�ɂ���
    }

    /// <summary>
    /// �t�F�[�h�C���������������ǂ���
    /// </summary>
    /// <returns></returns>
    public bool IsFadeInComplete()
    {
        return isCompFadeIn;
    }

    /// <summary>
    /// �t�F�[�h�A�E�g���J�n����
    /// </summary>
    public void StartFadeOut()
    {
        if (isFadeIn || isFadeOut)
        {
            return; //�t�F�[�h���͐V���Ƀt�F�[�h���J�n���Ȃ�
        }

        //�t�F�[�h�A�E�g�J�n����ꍇ�̏����l�ݒ�
        isFadeOut = true;
        isCompFadeOut = false;
        fadeTimer = 0.0f;

        fadeImg.color = new Color(1, 1, 1, 0);
        fadeImg.fillAmount = 0;
        fadeImg.raycastTarget = true; //�t�F�[�h�摜�̓����蔻��I���ɂ��邱�ƂŃ{�^�����������Ȃ��悤�ɂ���
    }

    /// <summary>
    /// �t�F�[�h�A�E�g�������������ǂ���
    /// </summary>
    /// <returns></returns>
    public bool IsFadeOutComplete()
    {
        return isCompFadeOut;
    }


    // Start is called before the first frame update
    void Start()
    {
        fadeImg = GetComponent<Image>();

        if (isFirstFadeInComp == true)
        {
            FadeInComplete();
        }
        else
        {
            StartFadeIn();
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (frameCount >2) //�V�[���J�n���A�ǂݍ��݂̃��O�΍��2�t���[���҂�
        {
            if (isFadeIn) //�t�F�[�h�C������
            {
                FadeInUpdate();
            }
            else if (isFadeOut)
            {
                FadeOutUpdate();
            }
        }
        frameCount++;
    }

    //�t�F�[�h�C�����̍X�V
    private void FadeInUpdate()
    {
        if (fadeTimer < 1f) //�V�[���J�n����1�b�����ăt�F�[�h�C������
        {
            fadeImg.color = new Color(1, 1, 1, 1 - fadeTimer);//��Z�F1�b�����ē����ɂȂ�
            fadeImg.fillAmount = 1f - fadeTimer; //1�b�����Ċz��h��Ԃ�
        }
        else
        {
            FadeInComplete();
        }
        fadeTimer += Time.deltaTime;
    }

    //�t�F�[�h�C����������
    private void FadeInComplete()
    {
        //�t�F�[�h�C���������Ɋe��p�����[�^���Z�b�g
        fadeImg.color = new Color(1, 1, 1, 0);//��Z
        fadeImg.fillAmount = 0; //�z�͓h��Ԃ��Ȃ�
        fadeImg.raycastTarget = false; //Image�̓����蔻����I�t

        fadeTimer = 0.0f;
        isFadeIn = false;
        isCompFadeIn = true;
    }

    //�t�F�[�h�A�E�g���̍X�V
    private void FadeOutUpdate()
    {
        if (fadeTimer < 1f) //�V�[���J�n����1�b�����ăt�F�[�h�A�E�g����
        {
            fadeImg.color = new Color(1, 1, 1, fadeTimer);//�F����Z�F1�b�����ĕs�����ɂȂ�
            fadeImg.fillAmount = fadeTimer; //1�b�����Ċz�𔍂���
        }
        else
        {
            FadeOutComplete();
        }
        fadeTimer += Time.deltaTime;
    }

    //�t�F�[�h�A�E�g��������
    private void FadeOutComplete()
    {
        //�t�F�[�h�A�E�g�������Ɋe��p�����[�^���Z�b�g
        fadeImg.color = new Color(1, 1, 1, 1);//�F����Z
        fadeImg.fillAmount = 1; //�z��h��Ԃ�
        fadeImg.raycastTarget = true; //�t�F�[�h�摜�̓����蔻��I���ɂ��邱�ƂŃ{�^�����������Ȃ��悤�ɂ���

        fadeTimer = 0.0f;
        isFadeOut = false;
        isCompFadeOut = true;
    }


}
