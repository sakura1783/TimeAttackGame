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

            Instantiate(enemyPrefab, generatePos[generateTran], Quaternion.identity);    

            generateEnemyCount++;
        }
    }
}
