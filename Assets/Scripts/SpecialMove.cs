using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpecialMove : MonoBehaviour
{
    /// <summary>
    /// 必殺技を使用する際の処理
    /// </summary>
    public void UseSpecialMove(CharaType charaType)
    {
        //どの必殺技を発動するか選んで実行
        SelectSpecialMove(charaType);
    }

    /// <summary>
    /// キャラによって必殺技を変える。どの必殺技か決めて実行　
    /// </summary>
    /// <param name="charaType"></param>
    private void SelectSpecialMove(CharaType charaType)
    {
        switch (charaType)
        {
            case CharaType.Yellow:
                Rovescio();
                break;
            case CharaType.Black:
                Mezzanotte();
                break;
            case CharaType.Pink:
                Magia();
                break;
            default:
                Debug.Log("CharaTypeがどれにも一致しないため、必殺技が行えませんでした");
                break;
        }
    }

    /// <summary>
    /// 金髪の女の子の必殺技　敵を一網打尽にする
    /// </summary>
    private void Rovescio()
    {
        Debug.Log("Rovescioが発動されました");
    }

    /// <summary>
    /// 黒髪の女の子の必殺技　時間停止
    /// </summary>
    private void Mezzanotte()
    {
        Debug.Log("Mezzanotteが発動されました");
    }

    /// <summary>
    /// ピンク髪の女の子の必殺技　仲間と一緒に戦う
    /// </summary>
    private void Magia()
    {
        Debug.Log("Magiaが発動されました");
    }
}
