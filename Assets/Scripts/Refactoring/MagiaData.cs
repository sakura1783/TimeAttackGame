using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MagiaData", menuName = "Create SpecialMoveData_Magia")]
public class MagiaData : SpecialMoveData
{
    [SerializeField] private MagiaCharaGenerator magiaCharaGenerator;

    [SerializeField] private int cloneCount;  //追加

    private List<MagiaChara> magiaCharaList = new();


    public override void BeforeSpecialMove()
    {
        //TODO 演出

        ////Chara[0]の生成とセットアップをして、リストに追加する。パーティクル(演出)も生成する
        //MagiaChara charaZero = magiaCharaGenerator.GenerateMagiaCharaZero();
        //charaZero.SetUpMagiaChara(gameManager, DataBaseManager.instance.charaDataSO.charaDatasList[0]);
        //ParticleSystem particleZero = Instantiate(particleMagia, charaZero.transform.position, Quaternion.identity);
        //particleZero.transform.SetParent(charaZero.transform);
        //magiaCharasList.Add(charaZero);

        ////Chara[1]の生成とセットアップをして、リストに追加する。パーティクル(演出)も生成する
        //MagiaChara charaOne = magiaCharaGenerator.GenerateMagiaCharaOne();
        //charaOne.SetUpMagiaChara(gameManager, DataBaseManager.instance.charaDataSO.charaDatasList[1]);
        //ParticleSystem particleOne = Instantiate(particleMagia, charaOne.transform.position, Quaternion.identity);
        //particleOne.transform.SetParent(charaOne.transform);
        //magiaCharasList.Add(charaOne);

        //上の処理を効率よく
        for (int i = 0; i < cloneCount; i++)
        {
            MagiaChara chara = magiaCharaGenerator.GenerateMagiaChara(i);
            chara.SetUpMagiaChara(gameManager, DataBaseManager.instance.charaDataSO.charaDatasList[i]);
            ParticleSystem particle = Instantiate(ParticleManager.instance.GetParticleFromName(ParticleName.Magia), chara.transform.position, Quaternion.identity);
            particle.transform.SetParent(chara.transform);
            magiaCharaList.Add(chara);
        }

        Debug.Log("Magiaが発動されました");
    }

    public override void AfterSpecialMove()
    {
        //リストに入っている全てのキャラ(Magiaで生成された全てのキャラ)をDestroyする
        for (int i = 0; i < magiaCharaList.Count; i++)
        {
            MagiaChara magiaChara = magiaCharaList[i];

            Destroy(magiaChara.gameObject);  //引数がmagiaCharaだと、magiaCharaクラスのコンポーネントが削除されるだけで、ゲームオブジェクト自体は削除されない。注意
        }
        //リストの中身を空にする
        magiaCharaList.Clear();

        Debug.Log("Magiaが終了しました");
    }
}
