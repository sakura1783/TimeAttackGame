using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaGenerator : MonoBehaviour
{
    [SerializeField] private CharaController charaPrefab;

    [SerializeField] private List<CharaDataSO.CharaData> charaDatasList = new List<CharaDataSO.CharaData>();

    public void SetUpCharaGenerator()
    {
        AddCharaDatasList();
    }

    /// <summary>
    /// プレイヤー生成
    /// </summary>
    /// <returns></returns>
    public CharaController GenerateChara()
    {
        //キャラの生成位置
        Vector2 generatePos = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);

        CharaController generatedChara = Instantiate(charaPrefab, generatePos, Quaternion.identity);

        return generatedChara;
    }

    /// <summary>
    /// 全てのキャラのデータをListに追加
    /// </summary>
    private void AddCharaDatasList()
    {
        //CharaDataSO内のCharaDataを一つずつListに追加
        for (int i = 0; i < DataBaseManager.instance.charaDataSO.charaDatasList.Count; i++)
        {
            charaDatasList.Add(DataBaseManager.instance.charaDataSO.charaDatasList[i]);
        }
    }
}
