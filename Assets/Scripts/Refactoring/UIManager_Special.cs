using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using DG.Tweening.Core.Easing;

//必殺技専用のUI制御クラス
public class UIManager_Special : MonoBehaviour
{
    //[SerializeField] private Button btnSpecialMove;  //もし必殺技をボタンで発動するなら記述する。

    [SerializeField] private Image imgSpecialMoveGauge;

    private readonly float maxFillAmount = 1;


    void Update()
    {
        
    }

    public void SetUp(SpecialMoveManager specialMoveManager)
    {
        //btnSpecialMove変数がnullでない時
        //if (btnSpecialMove)
        //{
        //    btnSpecialMove.onClick.RemoveAllListeners();
        //    btnSpecialMove.onClick.AddListener(specialMoveManager.ActiveSkill);

        //    UpdateButtonInteractable(false);
        //}

        //if (imgSpecialMoveGauge)
        //{
        //    imgSpecialMoveGauge.fillAmount = 0;
        //}

        //上の処理をメソッド化
        SetUpSpecialMoveGauge();
    }

    /// <summary>
    /// 必殺技ゲージの設定
    /// </summary>
    public void SetUpSpecialMoveGauge()
    {

        if (imgSpecialMoveGauge)
        {
            imgSpecialMoveGauge.fillAmount = 0;
        }
    }

    /// <summary>
    /// ボタンの状態更新
    /// </summary>
    /// <param name="interactable"></param>
    //public void UpdateButtonInteractable(bool interactable)
    //{
    //    if (btnSpecialMove != null)
    //    {
    //        btnSpecialMove.interactable = interactable;

    //        Debug.Log($"ボタンを更新しました：{interactable}");
    //    }
    //}

    /// <summary>
    /// ゲージ更新
    /// </summary>
    /// <param name="nextFillAmount"></param>
    public void UpdateSpecialMoveGauge(float nextFillAmount)
    {
        imgSpecialMoveGauge.DOFillAmount(nextFillAmount, 0.5f).SetEase(Ease.Linear);

        Debug.Log($"必殺技ゲージ更新しました：{nextFillAmount}");
    }
}
