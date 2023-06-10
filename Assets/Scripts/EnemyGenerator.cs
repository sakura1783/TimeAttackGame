using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private EnemyController enemyPrefab;

    [SerializeField] private Vector2[] generatePos;

    [SerializeField] private int intervalGenerateTime;

    [SerializeField] private GameManager gameManager;

    private int generateEnemyCount = 0;

    public List<EnemyController> enemiesList = new List<EnemyController>();

    [SerializeField] private CharaController charaController;

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

            EnemyController generatedEnemy = Instantiate(enemyPrefab, generatePos[generateTran], Quaternion.identity);    

            generateEnemyCount++;

            enemiesList.Add(generatedEnemy);

            enemyPrefab.SetUpEnemyController(charaController);
        }
    }
}
