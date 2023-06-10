using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public float timer = 0;

    public int killEnemyCount;

    private bool isGameClear = false;

    public int maxGenerateEnemyCount;

    [SerializeField] private CharaGenerator charaGenerator;
    [SerializeField] private CharaController charaController;

    void Start()
    {
        charaGenerator.GenerateChara();

        charaController.SetUpCharaController(this);
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
        }
    }
}
