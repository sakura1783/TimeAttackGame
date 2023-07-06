using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialMove : MonoBehaviour
{
    [SerializeField] private GameManager gameManager;  //情報をもらうために記述

    [SerializeField] private EnemyGenerator enemyGenerator;

    public bool isSpecialMoveActive = false;  //必殺技発動中かどうか(必殺技発動中に行わない処理を実装するため、UIManager内で使う)

    //public bool isMezzanotte = false;  //Mezzanotte発動中かどうか

    [SerializeField] private ParticleSystem particleRovescio;

    [SerializeField] private ParticleSystem particleMezzanotte;
    private ParticleSystem generatedParticleMezzanotte;
    public List<ParticleSystem> particleMezzanotteList = new List<ParticleSystem>();

    [SerializeField] private MagiaCharaGenerator magiaCharaGenerator;  //Magia時にキャラを生成するための変数。gameManagerから情報をもらう。

    [SerializeField] private List<MagiaChara> magiaCharasList = new List<MagiaChara>();

    /// <summary>
    /// 必殺技を使用する際の処理
    /// </summary>
    public IEnumerator UseSpecialMove(CharaType charaType)
    {
        //どの必殺技を発動するか選んで実行
        SelectSpecialMove(charaType);

        isSpecialMoveActive = true;

        //必殺技継続時間分待ってから
        Debug.Log(gameManager.CharaController.durationSpecialMove + "秒待ちます");
        yield return new WaitForSeconds(gameManager.CharaController.durationSpecialMove);

        //float elapsedTime = 0f;  //経過時間
        //float duration = gameManager.CharaController.durationSpecialMove;  //待つ時間(必殺技継続時間)

        //Debug.Log(duration + "秒待ちます");

        //while (elapsedTime <= duration)  //経過時間が待つ時間を下回っている間は
        //{
        //    elapsedTime += Time.deltaTime;
        //}

        //必殺技終了
        EndSpecialMove(charaType);

        isSpecialMoveActive = false;
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
            default:
                Debug.Log("CharaTypeがどれにも一致しないため、必殺技を終了できませんでした");
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

            //演出(パーティクル生成)
            Instantiate(particleRovescio, enemies.transform.position, Quaternion.identity);

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

            //演出　パーティクル生成
            generatedParticleMezzanotte = Instantiate(particleMezzanotte, enemies.transform.position, Quaternion.identity);

            //パーティクルとエネミーを親子関係にする
            generatedParticleMezzanotte.transform.SetParent(enemies.transform);

            //生成したパーティクルをListに追加
            particleMezzanotteList.Add(generatedParticleMezzanotte);

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
        for (int i = 0; i < enemyGenerator.enemiesList.Count; i++)
        {
            EnemyController enemies = enemyGenerator.enemiesList[i];

            enemies.ResumeMove();
            enemies.ResumeAnimation();
        }

        //演出終了　生成されてListに入っている全てのパーティクルをStopする(Stopすることでパーティクルは自動的に削除される)
        for (int i = 0; i < particleMezzanotteList.Count; i++)
        {
            ParticleSystem particle = particleMezzanotteList[i];

            particle.Stop();
        }
        //Listに追加したパーティクルをListから削除
        particleMezzanotteList.Clear();

        Debug.Log("Mezzanotteが終了しました");
    }

    /// <summary>
    /// ピンク髪の女の子の必殺技　仲間と一緒に戦う
    /// </summary>
    private void Magia()
    {
        //TODO 演出

        //Chara[0]の生成とセットアップをして、リストに追加する
        MagiaChara charaZero = magiaCharaGenerator.GenerateMagiaCharaZero();
        charaZero.SetUpMagiaChara(gameManager, DataBaseManager.instance.charaDataSO.charaDatasList[0]);
        magiaCharasList.Add(charaZero);

        //Chara[1]の生成とセットアップをして、リストに追加する
        MagiaChara charaOne = magiaCharaGenerator.GenerateMagiaCharaOne();
        charaOne.SetUpMagiaChara(gameManager, DataBaseManager.instance.charaDataSO.charaDatasList[1]);
        magiaCharasList.Add(charaOne);

        Debug.Log("Magiaが発動されました");
    }

    /// <summary>
    /// Magia終了
    /// </summary>
    private void EndMagia()
    {
        //リストに入っている全てのキャラ(Magiaで生成された全てのキャラ)をDestroyする
        for (int i = 0; i < magiaCharasList.Count; i++)
        {
            MagiaChara magiaChara = magiaCharasList[i];

            Destroy(magiaChara.gameObject);  //引数がmagiaCharaだと、magiaCharaクラスのコンポーネントが削除されるだけで、ゲームオブジェクト自体は削除されない。注意
        }
        //リストの中身を空にする
        magiaCharasList.Clear();

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
