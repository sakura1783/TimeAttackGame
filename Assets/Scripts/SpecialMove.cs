using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialMove : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;  //情報をもらうために記述

    [SerializeField] private EnemyGenerator enemyGenerator;

    /// <summary>
    /// 必殺技を使用する際の処理
    /// </summary>
    public void UseSpecialMove(CharaType charaType)
    {
        //どの必殺技を発動するか選んで実行
        SelectSpecialMove(charaType);

        //必殺技継続時間分待ってから(待つ処理をコルーチンにすると、コルーチンは非同期処理であるため、待つ処理が完了するのを待たず、EndSpecialMoveが動いてしまう。注意)
        float elapsedTime = 0f;  //経過時間
        float duration = gameManager.CharaController.durationSpecialMove;  //待つ時間(必殺技継続時間)

        Debug.Log(duration + "秒待ちます");

        while (elapsedTime <= duration)  //経過時間が待つ時間を下回っている間は
        {
            elapsedTime += Time.deltaTime;
        }

        //必殺技終了
        EndSpecialMove(charaType);
    }

    /// <summary>
    /// キャラによって必殺技を変える。どの必殺技か決めて実行　
    /// </summary>
    /// <param name="charaType"></param>
    private void SelectSpecialMove(CharaType charaType)
    {
        switch (charaType)
        {
            case CharaType.Yellow:
                Rovescio();
                break;
            case CharaType.Black:
                Mezzanotte();
                break;
            case CharaType.Pink:
                Magia();
                break;
            default:
                Debug.Log("CharaTypeがどれにも一致しないため、必殺技が行えませんでした");
                break;
        }
    }

    /// <summary>
    /// 必殺技終了
    /// </summary>
    private void EndSpecialMove(CharaType charaType)
    {
        switch (charaType)
        {
            case CharaType.Yellow:
                EndRovescio();
                break;
            case CharaType.Black:
                EndMezzanotte();
                break;
            case CharaType.Pink:
                EndMagia();
                break;
        }
    }

    /// <summary>
    /// 金髪の女の子の必殺技　敵を一網打尽にする
    /// </summary>
    private void Rovescio()
    {
        //TODO 演出

        //シーン上に生成されている全ての敵を破壊する
        for (int i = 0; i < enemyGenerator.enemiesList.Count; i++)
        {
            EnemyController enemies = enemyGenerator.enemiesList[i];

            Destroy(enemies.gameObject);
        }

        Debug.Log("Rovescioが発動されました");
    }

    /// <summary>
    /// Rovescio終了
    /// </summary>
    private void EndRovescio()
    {
        Debug.Log("Rovescioが終了しました");
    }

    /// <summary>
    /// 黒髪の女の子の必殺技　時間停止
    /// </summary>
    private void Mezzanotte()
    {
        //TODO 演出

        //シーン上に生成されている全ての敵の移動とアニメーションを停止する(Pauseメソッドを実行することでisPausedがtrueになるので、自動的に攻撃もしなくなる)
        for (int i = 0; i < enemyGenerator.enemiesList.Count; i++)
        {
            EnemyController enemies = enemyGenerator.enemiesList[i];

            enemies.PauseMove();
            enemies.PauseAnimation();
        }

        Debug.Log("Mezzanotteが発動されました");
    }

    /// <summary>
    /// Mezzanotte終了
    /// </summary>
    private void EndMezzanotte()
    {
        Debug.Log("Mezzanotteが終了しました");
    }

    /// <summary>
    /// ピンク髪の女の子の必殺技　仲間と一緒に戦う
    /// </summary>
    private void Magia()
    {
        //TODO 演出

        Debug.Log("Magiaが発動されました");
    }

    /// <summary>
    /// Magia終了
    /// </summary>
    private void EndMagia()
    {
        Debug.Log("Magiaが終了しました");
    }

    /// <summary>
    /// 必殺技継続時間分だけ待つ
    /// </summary>
    /// <returns></returns>
    //private IEnumerator WaitDuringSpecialMove()
    //{
    //    int waitTime = gameManager.CharaController.durationSpecialMove;

    //    yield return new WaitForSeconds(waitTime);

    //    Debug.Log("WaitDuringSpecialMoveメソッドの処理が動きました");
    //}
}
