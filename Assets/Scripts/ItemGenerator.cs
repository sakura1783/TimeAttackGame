using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemGenerator : MonoBehaviour
{
    [SerializeField] private Item[] itemsPrefab;

    //[SerializeField] private EnemyGenerator enemyGenerator;

    //private EnemyController enemyController;

    //private Vector2 enemyPos;

    //[SerializeField] private int generateNum;

    public void SetUpItemGenerator()
    {
        //enemyController = enemyGenerator.EnemyPrefab;
    }

    /// <summary>
    /// 一定の確率でアイテム生成
    /// </summary>
    public void GenerateItem(Transform tran)
    {
        //int num = Random.Range(0, 99);
        //Debug.Log("ランダムな値：" + num);

        //if (num <= generateNum)
        //{
        //enemyPos = enemyGenerator.GeneratedEnemy.EnemyPos;

        //Debug.Log("今調べたいもの：" + enemyGenerator.GeneratedEnemy.EnemyPos);

        int randomIndex = Random.Range(0, itemsPrefab.Length);

        Item item = Instantiate(itemsPrefab[randomIndex], tran.position, Quaternion.identity);
        //}
    }
}
