using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class LifeGauge : MonoBehaviour
{
    [SerializeField] private Slider slider;

    private EnemyController enemyController;

    public void SetUpLifeGauge(EnemyController enemyController)
    {
        this.enemyController = enemyController;  //ある特定のエネミーのhpが全部のHPバーの値に反映されてしまうかも

        slider.maxValue = this.enemyController.maxHp; 
    }

    void Update()
    {
        if (slider.maxValue == 1)  // <= SliderのmaxValueが初期値の1の時は、まだSetUpメソッドが動いていない。SetUpメソッドが動いてからUpdateが動くようにする
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
        slider.DOValue(enemyController.hp, 0.5f).SetLink(gameObject);  //0.5秒かけてhpを減らす、DOTween中にゲームオブジェクトが破棄される可能性がある場合、SetLink()を記述する
    }
}
