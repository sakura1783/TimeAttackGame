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
    public void SetUpObjAttackRange(CharaController charaController)
    {
        chara = charaController;

        //ObjAttackRange objAttackRange = Instantiate(this, chara.transform.position, Quaternion.identity);

        //TODO 親子関係を作る、子供にしたらUpdateコメントアウトする

        //サイズ、色を設定
        transform.localScale = Vector2.one * DataBaseManager.instance.GetAttackRangeSize(charaController.CharaData.attackRangeType).radius;

        if (TryGetComponent(out SpriteRenderer spriteRenderer))
        {
        //    Color rangeColor;

        //    switch (charaController.CharaData.charaType)
        //    {
        //        case CharaType.Yellow:
        //            rangeColor = Color.yellow;
        //            break;
        //        case CharaType.Black:
        //            rangeColor = Color.black;
        //            break;
        //        case CharaType.Pink:
        //            rangeColor = Color.magenta;
        //            break;
        //        default:
        //            rangeColor = Color.white;
        //            break;
        //    }
        //    spriteRenderer.color = rangeColor;

        //    spriteRenderer.color = new Color(1, 1, 1, 0.5f);

            Debug.Log("色：" + charaController.CharaData.charaType);

            spriteRenderer.color = charaController.CharaData.charaType switch
            {
                CharaType.Yellow => Color.yellow,
                CharaType.Black => Color.black,
                CharaType.Pink => Color.magenta,
                _ => Color.white
            };
            spriteRenderer.color = new Color(1, 1, 1, 0.5f);
        }
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
