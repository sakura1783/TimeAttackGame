using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlacementCharaSelectPopUp : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private Button btnSelect;

    [SerializeField] private Image imgSelectChara;
    [SerializeField] private Image imgSelectedChara;

    [SerializeField] private Text txtCharaName;
    [SerializeField] private Text txtAttackPoint;
    [SerializeField] private Text txtIntervalAttackTime;
    [SerializeField] private Text txtAttackRange;
    [SerializeField] private Text txtSpecialMoveName;
    [SerializeField] private Text txtMaxSpecialMoveCount;
    [SerializeField] private Text txtNeedSpecialMovePoint;
    [SerializeField] private Text txtDiscliption;

    [SerializeField] private ButtonChara btnCharaPrefab;  //キャラのボタン(btnChara)のプレハブ

    [SerializeField] private Transform btnCharaTran;  //キャラのボタンを生成する位置

    private List<ButtonChara> btnCharasList = new List<ButtonChara>();  //生成したキャラのボタンを管理する

    private CharaDataSO.CharaData selectCharaData;  //現在選択しているキャラの情報を管理する

    [SerializeField] private Home homeManager;


    public void SetUpPlacementCharaSelectPopUp(List<CharaDataSO.CharaData> charaDatasList)
    {
        canvasGroup.alpha = 0;

        SwitchActivateButtons(false);

        //CharaDataSOに登録されているキャラ分だけ
        for (int i = 0; i < charaDatasList.Count; i++)
        {
            //キャラのボタンを生成
            ButtonChara btnChara = Instantiate(btnCharaPrefab, btnCharaTran, false);

            //ボタンのゲームオブジェクトの設定
            btnChara.SetUpButtonChara(this, charaDatasList[i]);

            btnCharasList.Add(btnChara);

            //最初に生成したボタンの場合
            if (i == 0)
            {
                //選択しているキャラとして初期値に設定
                SetSelectCharaDetail(charaDatasList[i]);
            }
        }

        SwitchActivateButtonChara(false);

        //各ボタンにメソッドを登録
        btnSelect.onClick.AddListener(OnClickButtonSelect);
    }

    /// <summary>
    /// ボタンのアクティブ状態の切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    private void SwitchActivateButtons(bool isSwitch)
    {
        btnSelect.interactable = isSwitch;
    }

    /// <summary>
    /// btnCharaのアクティブ状態の切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    private void SwitchActivateButtonChara(bool isSwitch)
    {
        for (int i = 0; i < btnCharasList.Count; i++)
        {
            //if (btnCharasList[i].TryGetComponent(out Button btnChara))
            //{
            //    btnChara.interactable = isSwitch;
            //}

            btnCharasList[i].gameObject.SetActive(isSwitch);

            Debug.Log("btnCharaのSetActive : " + isSwitch);
        }
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    public void ShowPopUp()
    {
        canvasGroup.DOFade(1, 0.5f).SetEase(Ease.InQuad);

        SwitchActivateButtons(true);
        SwitchActivateButtonChara(true);
    }

    /// <summary>
    /// ポップアップの非表示
    /// </summary>
    private void HidePopUp()
    {
        canvasGroup.DOFade(0, 0.5f).SetEase(Ease.InQuad);

        SwitchActivateButtons(false);
        SwitchActivateButtonChara(false);
    }

    /// <summary>
    /// 「このキャラに決定する」ボタン(btnSelect)を押した際の処理
    /// </summary>
    private void OnClickButtonSelect()
    {
        imgSelectedChara.sprite = selectCharaData.charaSprite;

        HidePopUp();

        homeManager.SwitchActivateHomeButtons(true);
    }

    /// <summary>
    /// btnCharaで選択されたキャラの情報をポップアップに表示する
    /// </summary>
    public void SetSelectCharaDetail(CharaDataSO.CharaData charaData)
    {
        selectCharaData = charaData;

        //各値の設定
        imgSelectChara.sprite = charaData.charaSprite;
        txtCharaName.text = charaData.charaName;
        txtAttackPoint.text = charaData.attackPower.ToString();
        txtIntervalAttackTime.text = charaData.intervalAttackTime.ToString() + "秒";
        txtAttackRange.text = charaData.attackRangeType.ToString();
        txtSpecialMoveName.text = charaData.specialMoveName;
        txtMaxSpecialMoveCount.text = charaData.maxSpecialMoveCount.ToString() + "回";
        txtNeedSpecialMovePoint.text = charaData.intervalKillCountSpecialMove.ToString();
        txtDiscliption.text = charaData.discription;
    }
}
