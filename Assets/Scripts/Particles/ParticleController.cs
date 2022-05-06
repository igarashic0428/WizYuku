//======================
// このスクリプトの概要
// パーティクルの放出が止まり、画面からパーティクルがすべて消えたとき、オブジェクトを自動的に破壊する。
// 0.5 秒ごとにチェックを行い、毎フレームパーティクルシステムの状態を問い合わせないようにする。

using UnityEngine;
using System.Collections;

//======================
//このスクリプトに必要なコンポーネント
//このスクリプトアタッチすると、そのオブジェクトに自動的にコンポーネントが加えられる
[RequireComponent(typeof(ParticleSystem))]

//======================
//ParticleControllerクラス
public class ParticleController : MonoBehaviour
{
	//オブジェクトが有効になったときにUnityから呼ばれる
	//AwakeやStartと違って無効になってから、もう一度有効になっても呼ばれる。
	void OnEnable()
	{
		StartCoroutine("CheckIfAlive"); //コルーチンの関数CheckIfAlive()をコールする
	}

	//コルーチンの関数定義
	IEnumerator CheckIfAlive()
	{
		ParticleSystem ps = this.GetComponent<ParticleSystem>();

		while (true && ps != null) //ParticleSystemが破棄されていなければ
		{
			yield return new WaitForSeconds(0.5f); //このコルーチン関数内でこの行から下の処理は0.5秒待つ

			if (!ps.IsAlive(true)) //ParticleSystemが停止しているならば
			{
				GameObject.Destroy(this.gameObject); //オブジェクトを破棄する
			}
		}
	}
}
