using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagiaCharaGenerator : MonoBehaviour
{
    [SerializeField] private MagiaChara charaPrefab;

    private Vector2[] generatePos =
    {
        new (4, 2),
        new (-4, -2),
    };

    /// <summary>
    /// CharaData[0]のキャラを生成する
    /// </summary>
    /// <returns></returns>
    //public MagiaChara GenerateMagiaCharaZero()
    //{
    //    Vector2 generatePos = new Vector2(4, 2);

    //    MagiaChara generatedChara = Instantiate(charaPrefab, generatePos, Quaternion.identity);

    //    return generatedChara;
    //}

    /// <summary>
    /// CharaData[1] のキャラを生成する
    /// </summary>
    /// <returns></returns>
    //public MagiaChara GenerateMagiaCharaOne()
    //{
    //    Vector2 generatePos = new Vector2(-4, -2);

    //    MagiaChara generatedChara = Instantiate(charaPrefab, generatePos, Quaternion.identity);

    //    return generatedChara;
    //}

    /// <summary>
    /// 
    /// </summary>
    /// <param name="posIndex"></param>
    /// <returns></returns>
    public MagiaChara GenerateMagiaChara(int posIndex)
    {
        MagiaChara generateChara = Instantiate(charaPrefab, generatePos[posIndex], Quaternion.identity);

        return generateChara;
    }
}
