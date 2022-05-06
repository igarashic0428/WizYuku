using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //GetRootGameObjects()

using Assets.Scripts.Cameras;

public class SortingLayerController : MonoBehaviour
{
    //�N���X�̃C���X�^���X
    CameraController CameraController;

    Scene activeScene;

    GameObject[] RootObjects;

    //==============================================================
    //SortingLayer���L���[�̏��Ԓʂ�ɍX�V�p�ϐ�
    //==============================================================
    //SortingLayer��Layer���ƍ��킹�邱��
    string[] SortingLayerNum = new string[] {
            "Dfault",
            "SortingLayer01",
            "SortingLayer02",
            "SortingLayer03",
            "SortingLayer04",
            "SortingLayer05",
            "SortingLayer06",
            "SortingLayer07",
            "SortingLayer08",
            "SortingLayer09",
            "SortingLayer10",
            "SortingLayer11",
            "SortingLayer12",
            "SortingLayer13",
            "SortingLayer14",
            "SortingLayer15",
            "SortingLayer16",
            "SortingLayer17",
            "SortingLayer18",
            "SortingLayer19",
            "SortingLayer20"
    };

    //Start is called before the first frame update
    void Start()
    {
        //�N���X�̃C���X�^���X�ɃN���X���i�[
        CameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();

        //���݂̃V�[���擾
        activeScene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //===========�`��֌W�̒����Ȃ̂�LateUpdate�ŏ�������=====================

        //==============================================================
        //�L���[�֊i�[����I�u�W�F�N�g�ƃL���[�T�C�Y�̎擾
        //==============================================================
        //ActiveScene�̃��[�g�ɂ���I�u�W�F�N�g���擾
        RootObjects = activeScene.GetRootGameObjects();

        //���C���[�̓���ւ����z�肳���I�u�W�F�N�g�p�̃L���[���Z�o�p�ϐ�
        int QueSize = 0;
        int[] QueToRootObjNum = new int[RootObjects.Length];

        //�L���[�����Z�o
        for (int i = 0; i < RootObjects.Length; i++)
        {
            //�擾�����I�u�W�F�N�g���w��̃^�O�Ȃ��(�����̃^�O�̓L�����N�^�[�A2�����̃I�u�W�F�N�g�A�p�[�e�B�N���ɐݒ肷��)
            if ((RootObjects[i].tag == "TagPlayer") || (RootObjects[i].tag == "TagDestinationPoint") || (RootObjects[i].tag == "TagEnemy") ||
                (RootObjects[i].tag == "TagSpriteFence") || (RootObjects[i].tag == "TagPlayerAttack"))
            {
                //�擾�����I�u�W�F�N�g���J�����̕`��͈͓�(�X�Ƀo�b�t�@�Ł}10)�Ȃ��
                if (((CameraController.GetPos().x - CameraController.GetOrthographicSize() - 10.0f) < RootObjects[i].transform.position.x) &&
                     (RootObjects[i].transform.position.x < (CameraController.GetPos().x + CameraController.GetOrthographicSize() + 10.0f)))
                {
                    //Debug.Log("SortingLayerController���^�O" + RootObjects[i].tag + "���L���[�ɓ���܂�");
                    QueToRootObjNum[QueSize] = i; //�L���[�ɓ����Obj�����Ԗڂ���ۑ�
                    QueSize++; //�L���[���𑝂₷
                }
            }
        }

        //==============================================================
        //�L���[�ւ̊i�[
        //==============================================================
        //���C���[�̓���ւ����K�v�ȃI�u�W�F�N�g�p�̃L���[�錾�B
        GameObject[] ObjQue = new GameObject[QueSize]; //�T�C�Y��QueSize
        for (int i = 0; i < QueSize; i++)
        {
            ObjQue[i] = RootObjects[QueToRootObjNum[i]];
        }

        //==============================================================
        //�L���[��3D��ԏ��Z���Ŕ�r���ĕ��בւ���
        //==============================================================
        //�}���\�[�g�Ŏ��{
        //QueSize-1�񃋁[�v(1��0��r����n�܂�)
        for (int i = 1; i < QueSize; i++)
        {
            int k = i;//i�ԖڂƔ�r����z��̗v�f�ԍ�

            //�z����ObjQue[i]��荶�ɂ��鑀��ς݂́AObjQue[i]���Z����������Obj�̎�O��ObjQue[i]��}��
            //a��Z���������������肵�A�傫�����͔̂z���̈���ɃX���C�h����
            GameObject a = ObjQue[i]; //while���[�v���̏�����ObjQue[i]���㏑������Ă��܂��ׂ����ŕۑ����Ă����B
            while (k >= 1 && ObjQue[k - 1].transform.position.z > a.transform.position.z)
            {
                ObjQue[k] = ObjQue[k - 1];
                k--;
                //Debug.Log("sortingLayerID�̕��ёւ������܂�");
            }
            ObjQue[k] = a; //ObjQue[i]���Z����������Obj�̎�O��ObjQue[i]��}��
        }

        //==============================================================
        //SortingLayer���L���[�̏��Ԓʂ�ɍX�V
        //==============================================================
        for (int i = 0; i < QueSize; i++) //�L���[�ɂ��ꂽObj�̐��������[�v
        {
            //�����̎q��S�Ď擾
            Transform[] ObjQueChildren = ObjQue[i].GetComponentsInChildren<Transform>(true); //������true�ɂ��邱�ƂŔ�A�N�e�B�u�̃I�u�W�F�N�g���擾

            //�S�Ă̎q�ɑ΂���SpriteRenderer�������Ă��邩���肵
            //�����Ă��Ȃ���Ή������Ȃ��B
            //�����Ă����sortingLayerID��(QueSize - i)�ɍX�V
            for (int j = 0; j < ObjQueChildren.Length; j++) //�L���[�ɂ��ꂽObj�̎q�̐��������[�v
            {
                SpriteRenderer SprRen = ObjQueChildren[j].GetComponent<SpriteRenderer>(); //SpriteRenderer�R���|�[�l���g���擾(�����Ă��Ȃ����null������)
                ParticleSystemRenderer ParSysRen = ObjQueChildren[j].GetComponent<ParticleSystemRenderer>(); //ParticleSystemRenderer�R���|�[�l���g���擾(�����Ă��Ȃ����null������)

                if (SprRen != null) //SpriteRenderer�������Ă���Ȃ��
                {
                    if (SprRen.sortingLayerName == SortingLayerNum[QueSize - i]) //���ёւ��Ώۂ̃I�u�W�F�N�g��SortingLayerNum�ɓ��肫��Ȃ��Ȃ�Ƃ����ŃG���[�ƂȂ�B
                    {
                        //���o�^����Ă���sortingLayer���A���ꂩ��o�^���������O�Ɠ����ł��邱�Ƃ�1�ł��m�F�ł����Ȃ�΁A
                        //�S�ē����n�Y�Ȃ̂ŁAj���[�v�𔲂��Ď���i���[�v���s���B
                        break;
                    }
                    else
                    {
                        //Debug.Log("sortingLayerID��" + ChildrenSpriteRenderer.sortingLayerID + "�ł�");
                        Debug.Log("sortingLayer���X�V���܂�");
                        //Debug.Log(ObjQue[i].name + "�̎q�I�u�W�F�N�g�u" + ObjQueChildren[j].name + "�v��SpriteRenderer��sortingLayerName��" + (QueSize - i) + "�Ԗڂ�sortingLayer�ɍX�V���܂�");
                        SprRen.sortingLayerName = SortingLayerNum[QueSize - i]; //sortingLayerID���傫���قǎ�O�ɕ`�悳���
                    }
                }
                else if (ParSysRen != null) //ParticleSystemRenderer�������Ă���Ȃ��
                {
                    if (ParSysRen.sortingLayerName == SortingLayerNum[QueSize - i]) //���ёւ��Ώۂ̃I�u�W�F�N�g��SortingLayerNum�ɓ��肫��Ȃ��Ȃ�Ƃ����ŃG���[�ƂȂ�B
                    {
                        //���o�^����Ă���sortingLayer���A���ꂩ��o�^���������O�Ɠ����ł��邱�Ƃ�1�ł��m�F�ł����Ȃ�΁A
                        //�S�ē����n�Y�Ȃ̂ŁAj���[�v�𔲂��Ď���i���[�v���s���B
                        break;
                    }
                    else
                    {
                        //Debug.Log("sortingLayerID��" + ChildrenSpriteRenderer.sortingLayerID + "�ł�");
                        Debug.Log("sortingLayer���X�V���܂�");
                        //Debug.Log(ObjQue[i].name + "�̎q�I�u�W�F�N�g�u" + ObjQueChildren[j].name + "�v��SpriteRenderer��sortingLayerName��" + (QueSize - i) + "�Ԗڂ�sortingLayer�ɍX�V���܂�");
                        ParSysRen.sortingLayerName = SortingLayerNum[QueSize - i]; //sortingLayerID���傫���قǎ�O�ɕ`�悳���
                    }
                }
                else //��������������Ă��Ȃ��Ȃ��
                {
                    /*�������Ȃ�*/
                }
            }
        }

    }
}