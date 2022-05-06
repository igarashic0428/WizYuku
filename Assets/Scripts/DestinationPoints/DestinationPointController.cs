using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Assets.Scripts.Players;

public class DestinationPointController : MonoBehaviour
{
    float angle = 0;

    void FixedUpdate()
    {
        //���Y����]
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
        //��A�N�e�B�u�Ȃ�A�N�e�B�u�ɂ��ă|�W�V������ݒ肷��
        if (this.gameObject.activeSelf == false) {
            this.gameObject.SetActive(true);
        }
    }

    public void SetInactive()
    {
        //�A�N�e�B�u�Ȃ��A�N�e�B�u�ɂ���
        if (this.gameObject.activeSelf == true)
        {
            this.gameObject.SetActive(false);
        }
    }
}
