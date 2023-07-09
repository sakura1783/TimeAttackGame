using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class PlacementGameDiscliptionPopUp : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;

    [SerializeField] private Button btnClose;

    [SerializeField] private Home homeManager;


    public void SetUpPlacementGameDiscliptionPopUp()
    {
        canvasGroup.alpha = 0;

        SwitchActivateButtons(false);

        btnClose.onClick.AddListener(OnClickButtonClose);
    }

    /// <summary>
    /// ボタンのアクティブ状態の切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    private void SwitchActivateButtons(bool isSwitch)
    {
        btnClose.interactable = isSwitch;
    }

    /// <summary>
    /// btnCloseを押した際の処理
    /// </summary>
    private void OnClickButtonClose()
    {
        HidePopUp();

        homeManager.SwitchActivateHomeButtons(true);
    }

    /// <summary>
    /// ポップアップの表示
    /// </summary>
    public void ShowPopUp()
    {
        canvasGroup.DOFade(1, 0.5f).SetEase(Ease.InQuad);

        SwitchActivateButtons(true);
    }

    /// <summary>
    /// ポップアップの非表示
    /// </summary>
    private void HidePopUp()
    {
        canvasGroup.DOFade(0, 0.5f).SetEase(Ease.InQuad);

        SwitchActivateButtons(false);
    }
}
