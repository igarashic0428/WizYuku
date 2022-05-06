using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Players;

public class DestinationPointController : MonoBehaviour
{
    float angle = 0;

    void FixedUpdate()
    {
        //常にY軸回転
        this.transform.Rotate(0f, 5f, 0f);
        angle++;
        if (360f < angle) { angle = 0; }
    }

    public void Setpos(Vector3 vec)
    {
        this.transform.position = vec;
    }

    public void SetActive()
    {
        //非アクティブならアクティブにしてポジションを設定する
        if (this.gameObject.activeSelf == false) {
            this.gameObject.SetActive(true);
        }
    }

    public void SetInactive()
    {
        //アクティブなら非アクティブにする
        if (this.gameObject.activeSelf == true)
        {
            this.gameObject.SetActive(false);
        }
    }
}
