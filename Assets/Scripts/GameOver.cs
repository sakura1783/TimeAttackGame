using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    [SerializeField] private CanvasGroup lblCanvasGroup;
    [SerializeField] private CanvasGroup btnCanvasGroup;

    [SerializeField] private Button btnHome;
    [SerializeField] private Button btnRestart;

    void Start()
    {
        SwitchActivateButtons(false);

        SetUpButtons();

        StartCoroutine(SetUpPanel());
    }

    private void SetUpButtons()
    {
        btnHome.onClick.AddListener(OnClickButtonHome);
        btnRestart.onClick.AddListener(OnClickButtonRestart);
    }

    private IEnumerator SetUpPanel()
    {
        lblCanvasGroup.alpha = 0;
        btnCanvasGroup.alpha = 0;

        yield return new WaitForSeconds(1);

        lblCanvasGroup.DOFade(1, 3).SetEase(Ease.InQuad);

        yield return new WaitForSeconds(2);

        btnCanvasGroup.DOFade(1, 3).SetEase(Ease.InQuad).OnComplete(() => SwitchActivateButtons(true));
    }

    /// <summary>
    /// 「ホーム」ボタンを押した際の処理
    /// </summary>
    private void OnClickButtonHome()
    {
        SceneManager.LoadScene("Home");
    }

    /// <summary>
    /// 「リトライ」ボタンを押した際の処理
    /// </summary>
    private void OnClickButtonRestart()
    {
        SceneManager.LoadScene("Battle");
    }

    /// <summary>
    /// 各ボタンのアクティブ状態の切り替え
    /// </summary>
    /// <param name="isSwitch"></param>
    private void SwitchActivateButtons(bool isSwitch)
    {
        btnHome.interactable = isSwitch;
        btnRestart.interactable = isSwitch;
    }
}
