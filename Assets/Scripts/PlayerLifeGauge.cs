using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

//LifeGauge.csのプレイヤー版
public class PlayerLifeGauge : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private CharaController charaController;

    public void SetUpLifeGauge(CharaController charaController)
    {
        this.charaController = charaController;

        slider.maxValue = this.charaController.maxHp;
    }

    void Update()
    {
        if (slider.maxValue == 1)
        {
            return;
        }

        UpdateLifeGauge();
    }

    /// <summary>
    /// ライフゲージ更新処理
    /// </summary>
    private void UpdateLifeGauge()
    {
        //slider.value = enemyController.hp;
        slider.DOValue(charaController.hp, 0.5f).SetLink(gameObject);
    }
}
