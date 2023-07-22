using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MezzanotteData", menuName = "Create SpecialMoveData_Mezzanotte")]
public class MezzanotteData : SpecialMoveData
{
    private List<ParticleSystem> particleMezzanotteList = new();  //Mezzanotteで生成されたパーティクルを管理するためのリスト


    public override void BeforeSpecialMove()
    {
        //TODO 演出

        //シーン上に生成されている全ての敵の移動とアニメーションを停止する(Pauseメソッドを実行することでisPausedがtrueになるので、自動的に攻撃もしなくなる)
        for (int i = 0; i < enemyGenerator.enemiesList.Count; i++)
        {
            EnemyController enemies = enemyGenerator.enemiesList[i];

            //演出　パーティクル生成
            ParticleSystem generatedParticleMezzanotte = Instantiate(ParticleManager.instance.GetParticleFromName(ParticleName.Mezzanotte), enemies.transform.position, Quaternion.identity);

            //パーティクルとエネミーを親子関係にする
            generatedParticleMezzanotte.transform.SetParent(enemies.transform);

            //生成したパーティクルをListに追加
            particleMezzanotteList.Add(generatedParticleMezzanotte);

            enemies.PauseMove();
            enemies.PauseAnimation();
        }

        Debug.Log("Mezzanotteが発動されました");
    }

    public override void AfterSpecialMove()
    {
        //for (int i = 0; i < enemyGenerator.enemiesList.Count; i++)
        //{
        //    EnemyController enemies = enemyGenerator.enemiesList[i];

        //    enemies.ResumeMove();
        //    enemies.ResumeAnimation();
        //}

        //上の処理をforeachで書く。foreachにすることで該当のクラスにアクセスできる
        foreach (EnemyController enemy in enemyGenerator.enemiesList)
        {
            enemy.ResumeMove();
            enemy.ResumeAnimation();
        }

        //演出終了　生成されてListに入っている全てのパーティクルをStopする(Stopすることでパーティクルは自動的に削除される)
        //for (int i = 0; i < particleMezzanotteList.Count; i++)
        //{
        //    ParticleSystem particle = particleMezzanotteList[i];

        //    particle.Stop();
        //}

        //上の処理をforeachで書く。これも上の理由と同じ
        foreach (ParticleSystem particle in particleMezzanotteList)
        {
            particle.Stop();
        }
        //Listに追加したパーティクルをListから削除
        particleMezzanotteList.Clear();

        Debug.Log("Mezzanotteが終了しました");
    }
}
