using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameClear : MonoBehaviour
{
    [SerializeField] private CanvasGroup lblGameClearCanvasGroup;
    [SerializeField] private CanvasGroup lblTapCanvasGroup;
    [SerializeField] private CanvasGroup clearTimeCanvasGroup;

    [SerializeField] private Button btnTapArea;

    [SerializeField] private Text txtClearTime;

    void Start()
    {
        AudioManager.instance.PreparePlayBGM(3);

        SwitchActivateButtons(false);

        SetUpButtons();

        txtClearTime.text = GameData.instance.ClearTime.ToString();

        StartCoroutine(SetUpPanel());
    }

    private IEnumerator SetUpPanel()
    {
        lblGameClearCanvasGroup.alpha = 0;
        lblTapCanvasGroup.alpha = 0;
        clearTimeCanvasGroup.alpha = 0;

        //yield return new WaitForSeconds(1);

        //lblGameClearCanvasGroup.DOFade(1, 3).SetEase(Ease.InQuad);

        //yield return new WaitForSeconds(1);

        //clearTimeCanvasGroup.DOFade(1, 3).SetEase(Ease.InQuad);

        //yield return new WaitForSeconds(2);

        //lblTapCanvasGroup.DOFade(1, 1.5f).SetEase(Ease.InQuad).SetLoops(-1, LoopType.Yoyo);

        //yield return new WaitForSeconds(2);

        //SwitchActivateButtons(true);

        //上記の処理をシーケンスを利用して記述する
        Sequence seqence = DOTween.Sequence();

        seqence.AppendInterval(1);
        seqence.Append(lblGameClearCanvasGroup.DOFade(1, 3).SetEase(Ease.InQuad));
        seqence.AppendInterval(1);
        seqence.Append(clearTimeCanvasGroup.DOFade(1, 3).SetEase(Ease.InQuad));
        seqence.AppendInterval(2);

        Tween loopTween = lblTapCanvasGroup.DOFade(1, 1.5f).SetEase(Ease.InQuad);
        loopTween.SetLoops(-1, LoopType.Yoyo).SetLink(gameObject).OnStepComplete(() => SwitchActivateButtons(true));

        seqence.Append(loopTween);

        yield return null;
    }

    private void SetUpButtons()
    {
        btnTapArea.onClick.AddListener(OnClickButtonTapArea);
    }

    /// <summary>
    /// ボタンのアクティブ状態の切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    private void SwitchActivateButtons(bool isSwitch)
    {
        btnTapArea.interactable = isSwitch;
    }

    /// <summary>
    /// btnTapAreaを押した時の処理
    /// </summary>
    private void OnClickButtonTapArea()
    {
        SceneStateManager.instance.PrepareLoadNextScene(SceneType.Home);
    }
}
