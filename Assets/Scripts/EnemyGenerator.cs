using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private EnemyController enemyPrefab;
    //public EnemyController EnemyPrefab => enemyPrefab;

    [SerializeField] private Vector2[] generatePos;

    [SerializeField] private int intervalGenerateTime;

    [SerializeField] private GameManager gameManager;

    private int generateEnemyCount = 0;

    public List<EnemyController> enemiesList = new List<EnemyController>();

    [SerializeField] private CharaController charaController;

    private EnemyController generatedEnemy;
    public EnemyController GeneratedEnemy => generatedEnemy;

    void Start()
    {
        StartCoroutine(GenerateEnemy());
    }

    private IEnumerator GenerateEnemy()
    {
        while (gameManager.maxGenerateEnemyCount > generateEnemyCount)
        {
            //配列の中からエネミーを生成する位置をランダムに一つ選ぶ
            var generateTran = Random.Range(0, generatePos.Length);

            yield return new WaitForSeconds(intervalGenerateTime);

            generatedEnemy = Instantiate(enemyPrefab, generatePos[generateTran], Quaternion.identity);

            //今後のためにGameManagerにEnemyController型の情報を渡す
            gameManager.EnemyController = generatedEnemy;

            //TODO ObjAttackRangeゲームオブジェクト生成

            generateEnemyCount++;

            enemiesList.Add(generatedEnemy);

            //enemyPrefab.SetUpEnemyController(charaController);  // <= これは間違い。
            generatedEnemy.SetUpEnemyController(gameManager);  //Enemyのプレハブではなく、生成したEnemyの情報に命令を出す。
        }
    }
}
