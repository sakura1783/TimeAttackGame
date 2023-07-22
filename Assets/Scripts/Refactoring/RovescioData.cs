using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RovescioData", menuName = "Create SpecialMoveData_Rovescio")]
public class RovescioData : SpecialMoveData
{
    /// <summary>
    /// 必殺技の中身
    /// </summary>
    public override void BeforeSpecialMove()
    {
        //TODO 演出

        //シーン上に生成されている全ての敵を破壊する
        for (int i = 0; i < enemyGenerator.enemiesList.Count; i++)
        {
            EnemyController enemies = enemyGenerator.enemiesList[i];

            //演出(パーティクル生成)
            Instantiate(ParticleManager.instance.GetParticleFromName(ParticleName.Rovescio), enemies.transform.position, Quaternion.identity);

            Destroy(enemies.gameObject);

            //Listから必殺技で倒した敵を削除する(この処理を書かないと、倒した敵がListからRemoveされるのは通常攻撃で倒した時のみになるのでRovescioで倒した敵の情報はリストから削除されない。よって、2回目以降Rovescioを発動した時、Listに敵の情報がないよ、というMissingエラーが出てしまう)
            //enemyGenerator.enemiesList.Remove(enemies);

            //敵キル数をカウントアップ
            gameManager.AddKillEnemyCount();
        }

        //エネミーのListの中身を全て削除する(for文の中ではListの中身をいじらないようにする。for文の中でListをいじると、Listの中身が減る。for文で繰り返しの処理を行っている最中にListの数が変わるということは、想定している回数、for文が回らない)
        enemyGenerator.enemiesList.Clear();

        Debug.Log("Rovescioが発動されました");
    }

    /// <summary>
    /// 必殺技が終わった後の処理
    /// </summary>
    public override void AfterSpecialMove()
    {
        Debug.Log("Rovescioが終了しました");
    }
}
