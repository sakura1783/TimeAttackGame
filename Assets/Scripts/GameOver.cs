using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameOver : MonoBehaviour
{
    [SerializeField] private CanvasGroup lblCanvasGroup;
    [SerializeField] private CanvasGroup btnCanvasGroup;

    [SerializeField] private Button btnHome;
    [SerializeField] private Button btnRestart;

    void Start()
    {
        //TODO SetUpButton()

        StartCoroutine(SetUpPanel());
    }

    private IEnumerator SetUpPanel()
    {
        lblCanvasGroup.alpha = 0;
        btnCanvasGroup.alpha = 0;

        yield return new WaitForSeconds(1);

        lblCanvasGroup.DOFade(1, 3).SetEase(Ease.InQuad);

        yield return new WaitForSeconds(2);

        btnCanvasGroup.DOFade(1, 3).SetEase(Ease.InQuad);
    }
}
