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
    /// ピンク髪の女の子の必殺技　仲間と一緒に戦う
    /// </summary>
    private void Magia()
    {
        Debug.Log("Magiaが発動されました");
    }
}
