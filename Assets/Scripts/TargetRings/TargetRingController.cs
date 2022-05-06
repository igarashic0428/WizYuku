using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetRingController : MonoBehaviour
{
    GameObject targetObj;

    //�A�N�e�B�u�ɂȂ�����Ă΂��
    private void OnEnable()
    {
        StartCoroutine("SelfInactive"); //�R���[�`���̊֐�CheckIfAlive()���R�[������
    }

    void FixedUpdate()
    {
        if (targetObj != null)
        {
            this.transform.position = new Vector3(
                targetObj.transform.position.x,
                targetObj.transform.position.y + 0.3f,
                targetObj.transform.position.z);

            //���Z����]
            this.transform.Rotate(0f, 0f, 5.0f);
        }
        else
        { //�Ə������킹��I�u�W�F�N�g���j������Ă����玩���͔�A�N�e�B�u�ɂ���
            this.gameObject.SetActive(false);
        }
    }

    //�R���[�`���̊֐���`
    IEnumerator SelfInactive()
    {
        yield return new WaitForSeconds(0.5f); //���̃R���[�`���֐����ł��̍s���牺�̏�����0.5�b�҂�
        this.gameObject.SetActive(false); //���̃I�u�W�F�N�g���A�N�e�B�u�ɂ���

    }

    public void SetGameObject(GameObject Obj)
    {
        targetObj = Obj;
    }
}
