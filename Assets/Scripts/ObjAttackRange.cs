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

        //ObjAttackRange objAttackRange = Instantiate(this, chara.transform.position, Quaternion.identity);  // <= これだと、ObjAttackRangeを二つ生成してしまうことになる(自身をInstantiateしているため)

        //親子関係を作る。プレイヤーの子に設定する　(子供にしたらUpdateを省ける)
        this.gameObject.transform.SetParent(chara.transform);

        //サイズを設定
        //transform.localScale = Vector2.one * DataBaseManager.instance.GetAttackRangeSize(charaController.CharaData.attackRangeType).radius;
        var newScale = Vector2.one * DataBaseManager.instance.GetAttackRangeSize(charaController.CharaData.attackRangeType).radius * 2;  //最後に2を掛けて直径にする
        //var parentLossyScale = chara.transform.lossyScale;

        //transform.localScale = new Vector2(newScale.x / parentLossyScale.x, newScale.y / parentLossyScale.y);
        transform.localScale *= newScale;  //約1.4のキャラのサイズに対して、newScaleを掛け算する

        //色を設定
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

            spriteRenderer.color = charaController.CharaData.charaType switch
            {
                CharaType.Yellow => Color.yellow,
                CharaType.Black => Color.black,
                CharaType.Pink => Color.magenta,
                _ => Color.white
            };
            spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.3f);
        }
    }

    //void Update()
    //{
    //    if (chara == null)
    //    {
    //        return;
    //    }
    //    transform.position = chara.transform.position;
    //}
}
