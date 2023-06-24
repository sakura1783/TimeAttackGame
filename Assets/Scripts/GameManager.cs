using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class GameManager : MonoBehaviour
{
    public float timer = 0;

    public int killEnemyCount;

    private bool isGameClear = false;

    public int maxGenerateEnemyCount;

    [SerializeField] private CharaGenerator charaGenerator;

    public CharaController charaController;

    [SerializeField] private GameObject enemy;

    [SerializeField] private ObjAttackRange objAttackRange;

    [SerializeField] private CineMachine cineMachine;

    [SerializeField] private ItemGenerator itemGenerator;
    public ItemGenerator ItemGenerator => itemGenerator;

    [SerializeField] private Transform floatingDamageTran;

    [SerializeField] private FloatingMessage floatingMessagePrefab;

    //[SerializeField] private List<Item> items = new List<Item>();  //TODO Item型の変数を記述する(SetUpItemメソッドをStartメソッド内で実行したい)

    void Start()
    {
        charaController = charaGenerator.GenerateChara();  //戻り値のあるメソッドの活用。GenerateCharaメソッドの戻り値をcharaController変数に代入。こうすることで、生成されたキャラの情報が提供され、次行の命令がChara(Clone)に対しての命令となる。

        charaController.SetUpCharaController(this, DataBaseManager.instance.charaDataSO.charaDatasList[0]);  //TODO []の中にはランダムな変数を入れる

        cineMachine.SetUpCinemachine(this);  //キャラ生成前に書いてしまうと追尾対象のキャラがまだいないのでエラーになってしまう、注意

        objAttackRange.CreateObjAttackRange(this);

        itemGenerator.SetUpItemGenerator();

        //TODO SetUpItem()を実行する
    }

    void Update()
    {
        if (isGameClear == true)
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
    /// フロート表示の生成
    /// </summary>
    public void CreateFloatingMessage(int point)
    {
        FloatingMessage floatingMessage = Instantiate(floatingMessagePrefab, floatingDamageTran, false);

        //生成したフロート表示の設定用メソッドを実行。引数として、バレットの攻撃力値とフロート表示の種類を指定して渡す
        floatingMessage.DisplayFloatingMessage(point, FloatingMessage.FloatingMessageType.Damage);
    }
}
