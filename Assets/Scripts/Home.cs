using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour
{
    [SerializeField] private PlacementCharaSelectPopUp placementCharaSelectPop;

    [SerializeField] private PlacementGameDiscliptionPopUp placementGameDiscliptionPop;

    private List<CharaDataSO.CharaData> charaDatasList = new List<CharaDataSO.CharaData>();

    [Header("タイトル画面の各情報")]
    [SerializeField] private CanvasGroup titleCanvasGroup;
    [SerializeField] private CanvasGroup lblTapToHomeCanvasGroup;

    [SerializeField] private Button btnHome;

    [Header("ホーム画面の各情報")]
    [SerializeField] private Button btnGameStart;
    [SerializeField] private Button btnOpenCharaSelectPop;
    [SerializeField] private Button btnOpenGameDiscliptionPop;


    void Start()
    {
        AudioManager.instance.PreparePlayBGM(0);

        SwitchActivateTitleButtons(false);
        SwitchActivateHomeButtons(false);

        CreateCharaDatasList();

        placementCharaSelectPop.SetUpPlacementCharaSelectPopUp(charaDatasList);

        placementGameDiscliptionPop.SetUpPlacementGameDiscliptionPopUp();

        SetUpButtons();

        StartCoroutine(SetUpDisplay());
    }

    private IEnumerator SetUpDisplay()
    {
        lblTapToHomeCanvasGroup.alpha = 0;

        yield return new WaitForSeconds(1);

        lblTapToHomeCanvasGroup.DOFade(1, 1.5f).SetEase(Ease.InQuad).SetLoops(-1, LoopType.Yoyo);

        yield return new WaitForSeconds(2);

        SwitchActivateTitleButtons(true);
    }

    private void SetUpButtons()
    {
        btnHome.onClick.AddListener(OnClickButtonHome);

        btnGameStart.onClick.AddListener(OnClickButtonGameStart);
        btnOpenCharaSelectPop.onClick.AddListener(OnClickButtonOpenCharaSelectPop);
        btnOpenGameDiscliptionPop.onClick.AddListener(OnClickButtonOpenGameDiscliptionPop);
    }

    /// <summary>
    /// タイトル画面のボタンのアクティブ状態の切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    private void SwitchActivateTitleButtons(bool isSwitch)
    {
        btnHome.interactable = isSwitch;
        //btnHome.gameObject.SetActive(isSwitch);
    }

    /// <summary>
    /// ホーム画面のボタンのアクティブ状態の切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    public void SwitchActivateHomeButtons(bool isSwitch)
    {
        btnGameStart.interactable = isSwitch;
        btnOpenCharaSelectPop.interactable = isSwitch;
        btnOpenGameDiscliptionPop.interactable = isSwitch;
    }

    /// <summary>
    /// btnHomeを押した際の処理
    /// </summary>
    private void OnClickButtonHome()
    {
        //btnHomeとimgBackGroundのタップ判定を無くす(CanvasGroupで両方とも一緒に制御する)　これで、btnOpenCharaSelectPopUpが押せる状態になる
        titleCanvasGroup.blocksRaycasts = false;

        titleCanvasGroup.DOFade(0, 1).SetEase(Ease.InQuad);

        SwitchActivateHomeButtons(true);
    }

    /// <summary>
    /// btnStartを押した際の処理
    /// </summary>
    private void OnClickButtonGameStart()
    {
        SceneStateManager.instance.PrepareLoadNextScene(SceneType.Battle);
    }

    /// <summary>
    /// btnOpenCharaSelectPopUpを押した際の処理
    /// </summary>
    private void OnClickButtonOpenCharaSelectPop()
    {
        SwitchActivateHomeButtons(false);

        placementCharaSelectPop.ShowPopUp();
    }

    /// <summary>
    /// btnOpenGameDiscliptionPopUpを押した際の処理
    /// </summary>
    private void OnClickButtonOpenGameDiscliptionPop()
    {
        SwitchActivateHomeButtons(false);

        placementGameDiscliptionPop.ShowPopUp();
    }

    /// <summary>
    /// キャラのデータをリスト化
    /// </summary>
    private void CreateCharaDatasList()
    {
        for (int i = 0; i < DataBaseManager.instance.charaDataSO.charaDatasList.Count; i++)
        {
            charaDatasList.Add(DataBaseManager.instance.charaDataSO.charaDatasList[i]);
        }
    }
}
