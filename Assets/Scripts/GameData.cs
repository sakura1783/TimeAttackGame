using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameData : MonoBehaviour
{
    public static GameData instance;

    //各データ
    private int generateCharaNo;
    public int GenerateCharaNo { get; set; }

    private float clearTime;
    public float ClearTime { get; set; }


    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
