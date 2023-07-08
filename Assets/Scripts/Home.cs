using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class Home : MonoBehaviour
{
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
        SwitchActivateTitleButtons(false);

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
    }

    /// <summary>
    /// タイトル画面のボタンのアクティブ状態の切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    private void SwitchActivateTitleButtons(bool isSwitch)
    {
        btnHome.interactable = isSwitch;
    }

    /// <summary>
    /// ホーム画面のボタンのアクティブ状態の切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    private void SwitchActivateHomeButtons(bool isSwitch)
    {

    }

    /// <summary>
    /// btnHomeを押した際の処理
    /// </summary>
    private void OnClickButtonHome()
    {
        titleCanvasGroup.DOFade(0, 1).SetEase(Ease.InQuad);

        btnHome.interactable = false;
    }
}
