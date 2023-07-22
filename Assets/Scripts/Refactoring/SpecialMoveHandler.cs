using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//ゲーム機本体の役割。今回の必殺技を登録し、電源を入れ、落とす。必殺技の中身は知らない
public class SpecialMoveHandler : MonoBehaviour
{
    public bool isSpecialMoveActive = false;  //できればプロパティにした方がなおいい

    //今回使う必殺技。必殺技の中身までは知らない。なぜなら、ここで登録しているのは親クラスで、必殺技の中身は子クラスにあるため。漠然と「必殺技だよ」ということだけHandlerは知っている
    private SpecialMoveData currentSpecialMoveData;


    /// <summary>
    /// Handlerの初期設定。また、必殺技に共通の設定を行う
    /// </summary>
    /// <param name="specialMoveData"></param>
    /// <param name="gameManager"></param>
    /// <param name="enemyGenerator"></param>
    public void SetUpHandler(SpecialMoveData specialMoveData, GameManager gameManager, EnemyGenerator enemyGenerator)
    {
        //今回利用する必殺技の情報を引数から受け取る(ゲーム機にソフトをセット)
        currentSpecialMoveData = specialMoveData;

        if (currentSpecialMoveData == null)
        {
            Debug.Log("SpecialMoveDataを設定できません");

            return;
        }

        //必殺技に共通の設定を行う
        currentSpecialMoveData.SetUpSpecialMoveData(gameManager, enemyGenerator);
    }

    /// <summary>
    /// UIManager_Specialより実行する
    /// </summary>
    public void PrepareSpecialMove()
    {
        StartCoroutine(ExecuteSpecialMove());  //Execute = 実行する

        Debug.Log("SpecialMoveHandlerにて必殺技実行");
    }

    /// <summary>
    /// 必殺技の実処理
    /// </summary>
    /// <returns></returns>
    public IEnumerator ExecuteSpecialMove()
    {
        isSpecialMoveActive = true;

        //実処理
        //親クラスのBeforeSpecialMoveメソッドを実行する。しかし実際には子クラスがcurrentSpecialMoveDataに入っているので子クラス側のBeforeSpecialMoveメソッドが実行される => 振る舞いを変えている
        currentSpecialMoveData.BeforeSpecialMove();

        //待機時間(CharaDataではなく、必殺技の方に時間も紐付ける)
        yield return new WaitForSeconds(currentSpecialMoveData.duration);

        //後処理。ここも上のBeforeSpecialMoveメソッドと同じ
        currentSpecialMoveData.AfterSpecialMove();

        isSpecialMoveActive = false;
    }
}
