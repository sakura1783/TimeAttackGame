using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public float timer = 0;

    private int dataKillEnemyCount;  //クリア判定に利用する敵キル数
    public int DataKillEnemyCount => dataKillEnemyCount;

    private bool isGameUp = false;  //ゲームが終了しているか

    [SerializeField] private CharaGenerator charaGenerator;

    private CharaController charaController;
    public CharaController CharaController => charaController;

    [SerializeField] private GameObject enemy;

    [SerializeField] private ObjAttackRange objAttackRangePrefab;

    [SerializeField] private CineMachine cineMachine;

    [SerializeField] private ItemGenerator itemGenerator;
    public ItemGenerator ItemGenerator => itemGenerator;

    [SerializeField] private List<Item> items = new List<Item>();  //TODO Item型の変数を記述する(SetUpItemメソッドをStartメソッド内で実行したい)

    [SerializeField] private UIManager uiManager;
    public UIManager UiManager => uiManager;

    private EnemyController enemyController;
    public EnemyController EnemyController { get; set; }  //敵生成時にEnemyGeneratorから情報を渡してもらう

    [SerializeField] private EnemyGenerator enemyGenerator;
    public EnemyGenerator EnemyGenerator => enemyGenerator;

    public bool isGameOver = false;

    [SerializeField] private ParticleSystem charaDestroyParticle;

    public Canvas canvasFloatingTran;

    [SerializeField] private SpecialMoveManager specialMoveManager;

    void Start()
    {
        DOTween.SetTweensCapacity(3125, 50);

        AudioManager.instance.PreparePlayBGM(1);

        charaController = charaGenerator.GenerateChara();  //戻り値のあるメソッドの活用。GenerateCharaメソッドの戻り値をcharaController変数に代入。こうすることで、生成されたキャラの情報が提供され、次行の命令がChara(Clone)に対しての命令となる。

        charaController.SetUpCharaController(this, DataBaseManager.instance.charaDataSO.charaDatasList[GameData.instance.GenerateCharaNo], uiManager, charaController.CharaData.specialMoveData);

        enemyGenerator.SetUpEnemyGenerator();

        cineMachine.SetUpCinemachine(this);  //キャラ生成前に書いてしまうと追尾対象のキャラがまだいないのでエラーになってしまう、注意

        //ObjAttackRange生成、設定
        ObjAttackRange objAttackRange = Instantiate(objAttackRangePrefab, charaController.transform.position, Quaternion.identity);
        objAttackRange.SetUpObjAttackRange(charaController);

        itemGenerator.SetUpItemGenerator();

        //items[0].SetUpItem(this);  //TODO []内には変数を入れる　今は仮値

        uiManager.SetUpUIManager();

        //必殺技の準備
        if (specialMoveManager)
        {
            specialMoveManager.SetUpSpecialMoveManager(charaController.CharaData.specialMoveData, this, enemyGenerator);
        }
    }

    void Update()
    {
        if (isGameUp)
        {
            return;
        }

        timer += Time.deltaTime;

        if (dataKillEnemyCount >= enemyGenerator.MaxGenerateEnemyCount)
        {
            GameClear();
        }
    }

    /// <summary>
    /// ゲームクリア時の処理
    /// </summary>
    public void GameClear()
    {
        isGameUp = true;

        //GameDataのclearTime変数に経過時間を代入
        GameData.instance.ClearTime = timer;

        Debug.Log("ゲームクリア");

        SceneStateManager.instance.PrepareLoadNextScene(SceneType.GameClear);
    }

    /// <summary>
    /// ゲームオーバー時の処理
    /// </summary>
    public void GameOver()
    {
        isGameOver = true;

        Debug.Log("ゲームオーバー");

        //yield return new WaitForSeconds(2);

        //Instantiate(charaDestroyParticle, charaController.transform.position, Quaternion.identity);

        //Destroy(charaController.gameObject, 0.5f);

        //yield return new WaitForSeconds(1);  //コルーチンは非同期処理だが、DOTweenとは異なりゲームオブジェクトのライフタイムと紐づいているので、Destroyされた後の処理は動かなくなる

        SceneStateManager.instance.PrepareLoadNextScene(SceneType.GameOver);
    }

    /// <summary>
    /// GameOverメソッドを実行する
    /// </summary>
    //public void PrepareGameOver()
    //{
    //    //同じクラス内でコルーチンメソッドを実行しているので、このゲームオブジェクトが存在している場合は、必ず処理が動くし、このゲームオブジェクトがなくなれば処理は止まる
    //    //外部クラスが直接コルーチンを実行していないので、処理が止まった時に、外部クラスのゲームオブジェクトがあるかどうかのチェックは不要になる
    //    //また、private扱いできる
    //    //StartCoroutine(GameOver());

    //    GameOver();
    //}

    /// <summary>
    /// 敵キル数をカウントアップ
    /// </summary>
    public void AddKillEnemyCount()
    {
        dataKillEnemyCount++;

        Debug.Log("敵キル数：" + dataKillEnemyCount + "体");
    }
}
