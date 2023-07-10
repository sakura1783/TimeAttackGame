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

    public int maxGenerateEnemyCount;  //生成する敵の数

    [SerializeField] private int maxGenerateCountEnemy0;
    [SerializeField] private int maxGenerateCountEnemy1;  //敵0と1を生成する最大数

    [SerializeField] private int generateCountEnemy0;
    [SerializeField] private int generateCountEnemy1;  //敵0と1を生成した数

    private int generateEnemyNo;

    void Start()
    {
        StartCoroutine(GenerateEnemy());
    }

    private IEnumerator GenerateEnemy()
    {
        while (maxGenerateEnemyCount > generateEnemyCount)
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

            //敵の種類をランダムに決定
            //int enemyNo = Random.Range(0, DataBaseManager.instance.enemyDataSO.enemyDatasList.Count);
            int randomNo = Random.Range(0, 100);
            Debug.Log("ランダムな値：" + randomNo);

            //もしEnemy0が生成できなくなったら、強制的にEnemy1を生成する
            if (generateCountEnemy0 >= maxGenerateCountEnemy0)
            {
                randomNo = Random.Range(60, 100);

                Debug.Log("Enemy0が生成できないため、強制的にEnemy1を生成します");
            }
            //もしEnemy1が生成できなくなったら、強制的にEnemy0を生成する
            if (generateCountEnemy1 >= maxGenerateCountEnemy1)
            {
                randomNo = Random.Range(0, 60);

                Debug.Log("Enemy1が生成できないため、強制的にEnemy0を生成します");
            }

            if (randomNo <= 59)
            {
                if (generateCountEnemy0 < maxGenerateCountEnemy0)
                {
                    generateEnemyNo = 0;

                    generateCountEnemy0++;

                    Debug.Log("生成する敵は" + generateEnemyNo + "です");
                }
            }
            else
            {
                if (generateCountEnemy1 < maxGenerateCountEnemy1)
                {
                    generateEnemyNo = 1;

                    generateCountEnemy1++;

                    Debug.Log("生成する敵は" + generateEnemyNo + "です");
                }
            }

            //enemyPrefab.SetUpEnemyController(charaController);  // <= これは間違い。
            generatedEnemy.SetUpEnemyController(gameManager, DataBaseManager.instance.enemyDataSO.enemyDatasList[generateEnemyNo]);  //Enemyのプレハブではなく、生成したEnemyの情報に命令を出す。
        }
    }
}
