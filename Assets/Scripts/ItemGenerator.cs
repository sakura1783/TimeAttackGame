using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] private Item[] itemsPrefab;

    [SerializeField] private EnemyGenerator enemyGenerator;

    //private EnemyController enemyController;

    private Vector2 enemyPos;

    public void SetUpItemGenerator()
    {
        //enemyController = enemyGenerator.EnemyPrefab;
    }

    /// <summary>
    /// 一定の確率でアイテム生成
    /// </summary>
    public void GenerateItem()
    {
        int num = Random.Range(0, 99);
        Debug.Log("ランダムな値：" + num);

        if (num <= 29)
        {
            enemyPos = enemyGenerator.EnemyPrefab.EnemyPos;

            Debug.Log("今調べたいもの：" + enemyGenerator.EnemyPrefab);

            int randomIndex = Random.Range(0, itemsPrefab.Length);

            Item item = Instantiate(itemsPrefab[randomIndex], enemyPos, Quaternion.identity);
        }
    }
}
