using UnityEngine;

public class SpecialMoveData : ScriptableObject, ISpecialMove
{
    //メンバ変数には、全ての必殺技で共通して管理しておきたい情報を書いておく
    //修飾子をprotectedにすると、このクラスを継承したクラスでもその情報が扱えるようになる
    protected GameManager gameManager;
    protected EnemyGenerator enemyGenerator;

    public int maxSpecialMoveCount;
    public int interval;
    public float duration;


    /// <summary>
    /// 必殺技共通の初期設定
    /// </summary>
    /// <param name="gameManager"></param>
    /// <param name="enemyGenerator"></param>
    public void SetUpSpecialMoveData(GameManager gameManager, EnemyGenerator enemyGenerator)
    {
        this.gameManager = gameManager;
        this.enemyGenerator = enemyGenerator;

        Debug.Log("SpecialMoveData設定しました");
    }

    public virtual void BeforeSpecialMove()
    {
        //子クラスで振る舞いを変える
    }

    public virtual void AfterSpecialMove()
    {
        //子クラスで振る舞いを変える
    }
}
