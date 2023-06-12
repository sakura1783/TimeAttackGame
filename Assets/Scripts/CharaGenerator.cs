using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaGenerator : MonoBehaviour
{
    [SerializeField] private CharaController charaPrefab;

    public CharaController GenerateChara()
    {
        //キャラの生成位置
        Vector2 generatePos = new Vector2(Camera.main.transform.position.x, Camera.main.transform.position.y);

        CharaController generatedChara = Instantiate(charaPrefab, generatePos, Quaternion.identity);

        return generatedChara;
    }
}
