//======================
// ���̃X�N���v�g�̊T�v
// �p�[�e�B�N���̕��o���~�܂�A��ʂ���p�[�e�B�N�������ׂď������Ƃ��A�I�u�W�F�N�g�������I�ɔj�󂷂�B
// 0.5 �b���ƂɃ`�F�b�N���s���A���t���[���p�[�e�B�N���V�X�e���̏�Ԃ�₢���킹�Ȃ��悤�ɂ���B

using UnityEngine;
using System.Collections;

//======================
//���̃X�N���v�g�ɕK�v�ȃR���|�[�l���g
//���̃X�N���v�g�A�^�b�`����ƁA���̃I�u�W�F�N�g�Ɏ����I�ɃR���|�[�l���g����������
[RequireComponent(typeof(ParticleSystem))]

//======================
//ParticleController�N���X
public class ParticleController : MonoBehaviour
{
	//�I�u�W�F�N�g���L���ɂȂ����Ƃ���Unity����Ă΂��
	//Awake��Start�ƈ���Ė����ɂȂ��Ă���A������x�L���ɂȂ��Ă��Ă΂��B
	void OnEnable()
	{
		StartCoroutine("CheckIfAlive"); //�R���[�`���̊֐�CheckIfAlive()���R�[������
	}

	//�R���[�`���̊֐���`
	IEnumerator CheckIfAlive()
	{
		ParticleSystem ps = this.GetComponent<ParticleSystem>();

		while (true && ps != null) //ParticleSystem���j������Ă��Ȃ����
		{
			yield return new WaitForSeconds(0.5f); //���̃R���[�`���֐����ł��̍s���牺�̏�����0.5�b�҂�

			if (!ps.IsAlive(true)) //ParticleSystem����~���Ă���Ȃ��
			{
				GameObject.Destroy(this.gameObject); //�I�u�W�F�N�g��j������
			}
		}
	}
}
