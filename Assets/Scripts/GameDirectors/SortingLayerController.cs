using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; //GetRootGameObjects()

using Assets.Scripts.Cameras;

public class SortingLayerController : MonoBehaviour
{
    //クラスのインスタンス
    CameraController CameraController;

    Scene activeScene;

    GameObject[] RootObjects;

    //==============================================================
    //SortingLayerをキューの順番通りに更新用変数
    //==============================================================
    //SortingLayerのLayer名と合わせること
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
        //クラスのインスタンスにクラスを格納
        CameraController = GameObject.Find("Main Camera").GetComponent<CameraController>();

        //現在のシーン取得
        activeScene = SceneManager.GetActiveScene();
    }

    // Update is called once per frame
    void LateUpdate()
    {
        //===========描画関係の調整なのでLateUpdateで処理する=====================

        //==============================================================
        //キューへ格納するオブジェクトとキューサイズの取得
        //==============================================================
        //ActiveSceneのルートにあるオブジェクトを取得
        RootObjects = activeScene.GetRootGameObjects();

        //レイヤーの入れ替えが想定されるオブジェクト用のキュー数算出用変数
        int QueSize = 0;
        int[] QueToRootObjNum = new int[RootObjects.Length];

        //キュー数を算出
        for (int i = 0; i < RootObjects.Length; i++)
        {
            //取得したオブジェクトが指定のタグならば(これらのタグはキャラクター、2次元のオブジェクト、パーティクルに設定する)
            if ((RootObjects[i].tag == "TagPlayer") || (RootObjects[i].tag == "TagDestinationPoint") || (RootObjects[i].tag == "TagEnemy") ||
                (RootObjects[i].tag == "TagSpriteFence") || (RootObjects[i].tag == "TagPlayerAttack"))
            {
                //取得したオブジェクトがカメラの描画範囲内(更にバッファで±10)ならば
                if (((CameraController.GetPos().x - CameraController.GetOrthographicSize() - 10.0f) < RootObjects[i].transform.position.x) &&
                     (RootObjects[i].transform.position.x < (CameraController.GetPos().x + CameraController.GetOrthographicSize() + 10.0f)))
                {
                    //Debug.Log("SortingLayerControllerがタグ" + RootObjects[i].tag + "をキューに入れます");
                    QueToRootObjNum[QueSize] = i; //キューに入れるObjが何番目かを保存
                    QueSize++; //キュー数を増やす
                }
            }
        }

        //==============================================================
        //キューへの格納
        //==============================================================
        //レイヤーの入れ替えが必要なオブジェクト用のキュー宣言。
        GameObject[] ObjQue = new GameObject[QueSize]; //サイズはQueSize
        for (int i = 0; i < QueSize; i++)
        {
            ObjQue[i] = RootObjects[QueToRootObjNum[i]];
        }

        //==============================================================
        //キューを3D空間上のZ軸で比較して並べ替える
        //==============================================================
        //挿入ソートで実施
        //QueSize-1回ループ(1と0比較から始まる)
        for (int i = 1; i < QueSize; i++)
        {
            int k = i;//i番目と比較する配列の要素番号

            //配列上のObjQue[i]より左にある操作済みの、ObjQue[i]よりZ軸が小さいObjの手前にObjQue[i]を挿入
            //aのZ軸が小さいか判定し、大きいものは配列上の一つ左にスライドする
            GameObject a = ObjQue[i]; //whileループ内の処理でObjQue[i]が上書きされてしまう為ここで保存しておく。
            while (k >= 1 && ObjQue[k - 1].transform.position.z > a.transform.position.z)
            {
                ObjQue[k] = ObjQue[k - 1];
                k--;
                //Debug.Log("sortingLayerIDの並び替えをします");
            }
            ObjQue[k] = a; //ObjQue[i]よりZ軸が小さいObjの手前にObjQue[i]を挿入
        }

        //==============================================================
        //SortingLayerをキューの順番通りに更新
        //==============================================================
        for (int i = 0; i < QueSize; i++) //キューにいれたObjの数だけループ
        {
            //自分の子を全て取得
            Transform[] ObjQueChildren = ObjQue[i].GetComponentsInChildren<Transform>(true); //引数をtrueにすることで非アクティブのオブジェクトも取得

            //全ての子に対してSpriteRendererを持っているか判定し
            //持っていなければ何もしない。
            //持っていればsortingLayerIDを(QueSize - i)に更新
            for (int j = 0; j < ObjQueChildren.Length; j++) //キューにいれたObjの子の数だけループ
            {
                SpriteRenderer SprRen = ObjQueChildren[j].GetComponent<SpriteRenderer>(); //SpriteRendererコンポーネントを取得(持っていなければnullが入る)
                ParticleSystemRenderer ParSysRen = ObjQueChildren[j].GetComponent<ParticleSystemRenderer>(); //ParticleSystemRendererコンポーネントを取得(持っていなければnullが入る)

                if (SprRen != null) //SpriteRendererを持っているならば
                {
                    if (SprRen.sortingLayerName == SortingLayerNum[QueSize - i]) //並び替え対象のオブジェクトがSortingLayerNumに入りきらなくなるとここでエラーとなる。
                    {
                        //今登録されているsortingLayerが、これから登録したい名前と同じであることが1個でも確認できたならば、
                        //全て同じハズなので、jループを抜けて次のiループを行う。
                        break;
                    }
                    else
                    {
                        //Debug.Log("sortingLayerIDは" + ChildrenSpriteRenderer.sortingLayerID + "です");
                        Debug.Log("sortingLayerを更新します");
                        //Debug.Log(ObjQue[i].name + "の子オブジェクト「" + ObjQueChildren[j].name + "」のSpriteRendererのsortingLayerNameを" + (QueSize - i) + "番目のsortingLayerに更新します");
                        SprRen.sortingLayerName = SortingLayerNum[QueSize - i]; //sortingLayerIDが大きいほど手前に描画される
                    }
                }
                else if (ParSysRen != null) //ParticleSystemRendererを持っているならば
                {
                    if (ParSysRen.sortingLayerName == SortingLayerNum[QueSize - i]) //並び替え対象のオブジェクトがSortingLayerNumに入りきらなくなるとここでエラーとなる。
                    {
                        //今登録されているsortingLayerが、これから登録したい名前と同じであることが1個でも確認できたならば、
                        //全て同じハズなので、jループを抜けて次のiループを行う。
                        break;
                    }
                    else
                    {
                        //Debug.Log("sortingLayerIDは" + ChildrenSpriteRenderer.sortingLayerID + "です");
                        Debug.Log("sortingLayerを更新します");
                        //Debug.Log(ObjQue[i].name + "の子オブジェクト「" + ObjQueChildren[j].name + "」のSpriteRendererのsortingLayerNameを" + (QueSize - i) + "番目のsortingLayerに更新します");
                        ParSysRen.sortingLayerName = SortingLayerNum[QueSize - i]; //sortingLayerIDが大きいほど手前に描画される
                    }
                }
                else //いずれもを持っていないならば
                {
                    /*何もしない*/
                }
            }
        }

    }
}