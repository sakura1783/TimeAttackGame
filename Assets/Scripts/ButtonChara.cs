using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChara : MonoBehaviour
{
    [SerializeField] private Button btnChara;

    [SerializeField] private Image imgChara;

    private PlacementCharaSelectPopUp placementCharaSelectPop;

    private CharaDataSO.CharaData charaData;

    public void SetUpButtonChara(PlacementCharaSelectPopUp placementCharaSelectPop, CharaDataSO.CharaData charaData)
    {
        this.placementCharaSelectPop = placementCharaSelectPop;
        this.charaData = charaData;

        imgChara.sprite = this.charaData.charaSprite;

        btnChara.onClick.AddListener(OnClickButtonChara);
    }

    /// <summary>
    /// btnCharaを押した際の処理
    /// </summary>
    private void OnClickButtonChara()
    {
        placementCharaSelectPop.SetSelectCharaDetail(charaData);
    }
}
