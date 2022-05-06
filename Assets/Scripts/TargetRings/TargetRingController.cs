using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRingController : MonoBehaviour
{
    GameObject targetObj;

    //アクティブになったら呼ばれる
    private void OnEnable()
    {
        StartCoroutine("SelfInactive"); //コルーチンの関数CheckIfAlive()をコールする
    }

    void FixedUpdate()
    {
        if (targetObj != null)
        {
            this.transform.position = new Vector3(
                targetObj.transform.position.x,
                targetObj.transform.position.y + 0.3f,
                targetObj.transform.position.z);

            //常にZ軸回転
            this.transform.Rotate(0f, 0f, 5.0f);
        }
        else
        { //照準を合わせるオブジェクトが破棄されていたら自分は非アクティブにする
            this.gameObject.SetActive(false);
        }
    }

    //コルーチンの関数定義
    IEnumerator SelfInactive()
    {
        yield return new WaitForSeconds(0.5f); //このコルーチン関数内でこの行から下の処理は0.5秒待つ
        this.gameObject.SetActive(false); //このオブジェクトを非アクティブにする

    }

    public void SetGameObject(GameObject Obj)
    {
        targetObj = Obj;
    }
}
