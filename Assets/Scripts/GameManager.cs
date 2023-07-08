using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;
using DG.Tweening;

public class GameManager : MonoBehaviour
{
    public float timer = 0;

    private int dataKillEnemyCount;  //クリア判定に利用する敵キル数

    private bool isGameClear = false;

    public int maxGenerateEnemyCount;

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

    void Start()
    {
        DOTween.SetTweensCapacity(3125, 50);

        charaController = charaGenerator.GenerateChara();  //戻り値のあるメソッドの活用。GenerateCharaメソッドの戻り値をcharaController変数に代入。こうすることで、生成されたキャラの情報が提供され、次行の命令がChara(Clone)に対しての命令となる。

        charaController.SetUpCharaController(this, DataBaseManager.instance.charaDataSO.charaDatasList[0], uiManager);  //TODO []の中にはランダムな変数を入れる

        cineMachine.SetUpCinemachine(this);  //キャラ生成前に書いてしまうと追尾対象のキャラがまだいないのでエラーになってしまう、注意

        //ObjAttackRange生成、設定
        ObjAttackRange objAttackRange = Instantiate(objAttackRangePrefab, charaController.transform.position, Quaternion.identity);
        objAttackRange.SetUpObjAttackRange(charaController);

        itemGenerator.SetUpItemGenerator();

        //items[0].SetUpItem(this);  //TODO []内には変数を入れる　今は仮値

        uiManager.SetUpUIManager();
    }

    void Update()
    {
        if (isGameClear)
        {
            return;
        }

        timer += Time.deltaTime;
    }

    /// <summary>
    /// ゲームクリア判定
    /// </summary>
    public void GameClear()
    {
        if (dataKillEnemyCount >= maxGenerateEnemyCount)
        {
            Debug.Log("ゲームクリア");

            //ゲーム開始時敵のプレハブに追加したNavMeshAgent2Dコンポーネントを削除
            //Destroy(enemy.GetComponent<NavMeshAgent2D>());
        }
    }

    /// <summary>
    /// 敵キル数をカウントアップ
    /// </summary>
    public void AddKillEnemyCount()
    {
        dataKillEnemyCount++;

        Debug.Log("敵キル数：" + dataKillEnemyCount + "体");
    }
}
