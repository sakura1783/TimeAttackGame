using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjAttackRange : MonoBehaviour
{
    private GameManager gameManager;

    [SerializeField] private CharaController chara;

    /// <summary>
    /// 攻撃範囲可視化オブジェクト生成
    /// </summary>
    /// <param name="gameManager"></param>
    public void CreateObjAttackRange(GameManager gameManager)
    {
        this.gameManager = gameManager;
        chara = gameManager.CharaController;

        ObjAttackRange objAttackRange = Instantiate(this, chara.transform.position, Quaternion.identity);

        //TODO サイズ設定
        //objAttackRange.transform.localScale = new Vector2(DataBaseManager.instance.attackRangeSizeSO.attackRangeSizeList.);
    }

    void Update()
    {
        if (chara == null)
        {
            return;
        }
        transform.position = chara.transform.position;
    }
}
