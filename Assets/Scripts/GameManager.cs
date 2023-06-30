using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public float timer = 0;

    private int killEnemyCount = 0;

    private bool isGameClear = false;

    public int maxGenerateEnemyCount;

    [SerializeField] private CharaGenerator charaGenerator;

    private CharaController charaController;
    public CharaController CharaController => charaController;

    [SerializeField] private GameObject enemy;

    [SerializeField] private ObjAttackRange objAttackRange;

    [SerializeField] private CineMachine cineMachine;

    [SerializeField] private ItemGenerator itemGenerator;
    public ItemGenerator ItemGenerator => itemGenerator;

    [SerializeField] private List<Item> items = new List<Item>();  //TODO Item型の変数を記述する(SetUpItemメソッドをStartメソッド内で実行したい)

    [SerializeField] private UIManager uiManager;
    public UIManager UiManager => uiManager;

    void Start()
    {
        charaController = charaGenerator.GenerateChara();  //戻り値のあるメソッドの活用。GenerateCharaメソッドの戻り値をcharaController変数に代入。こうすることで、生成されたキャラの情報が提供され、次行の命令がChara(Clone)に対しての命令となる。

        charaController.SetUpCharaController(this, DataBaseManager.instance.charaDataSO.charaDatasList[0], uiManager);  //TODO []の中にはランダムな変数を入れる

        cineMachine.SetUpCinemachine(this);  //キャラ生成前に書いてしまうと追尾対象のキャラがまだいないのでエラーになってしまう、注意

        objAttackRange.CreateObjAttackRange(this);

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
        if (killEnemyCount >= maxGenerateEnemyCount)
        {
            Debug.Log("ゲームクリア");

            //ゲーム開始時敵のプレハブに追加したNavMeshAgent2Dコンポーネントを削除
            //Destroy(enemy.GetComponent<NavMeshAgent2D>());
        }
    }

    /// <summary>
    /// 敵キル数をカウントアップ
    /// </summary>
    public int AddKillEnemyCount()
    {
        killEnemyCount++;

        Debug.Log("敵キル数：" + killEnemyCount + "体");

        return killEnemyCount;
    }
}
