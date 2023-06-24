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

    void Start()
    {
        charaGenerator.SetUpCharaGenerator();

        charaController = charaGenerator.GenerateChara();  //戻り値のあるメソッドの活用。GenerateCharaメソッドの戻り値をcharaController変数に代入。こうすることで、生成されたキャラの情報が提供され、次行の命令がChara(Clone)に対しての命令となる。

        charaController.SetUpCharaController(this);

        cineMachine.SetUpCinemachine(this);  //キャラ生成前に書いてしまうと追尾対象のキャラがまだいないのでエラーになってしまう、注意

        objAttackRange.CreateObjAttackRange(this);

        itemGenerator.SetUpItemGenerator();
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
}
