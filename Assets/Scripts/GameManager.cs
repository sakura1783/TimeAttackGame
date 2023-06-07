using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float timer = 0;

    [SerializeField] private int clearCount;

    [SerializeField] private int killEnemyCount;

    private bool isGameClear = false;

    void Update()
    {
        if (isGameClear == true)
        {
            return;
        }

        timer += Time.deltaTime;

        KillEnemyCount();
    }

    /// <summary>
    /// 倒した敵の数
    /// </summary>
    private void KillEnemyCount()
    {
        if (Input.GetMouseButtonDown(0))
        {
            killEnemyCount++;

            if (killEnemyCount >= clearCount)
            {
                Debug.Log("ゲームクリア");
            }
        }
    }
}
