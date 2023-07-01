using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text txtTime;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private Image imgIntervalAttackTime;

    //private BulletGenerator bulletGenerator;

    [SerializeField] private Image imgSpecialMoveCount;

    private int maxValue;  //FillAmountの最大値

    [SerializeField] private ParticleSystem particleSpecialMoveGauge;

    private ParticleSystem generatedParticle;

    [SerializeField] private Transform particleTran;

    public void SetUpUIManager()
    {
        //bulletGenerator = gameManager.CharaController.BulletGenerator;

        maxValue = gameManager.CharaController.intervalKillCountSpecialMove;

        imgSpecialMoveCount.fillAmount = 0;
    }

    void Update()
    {
        txtTime.text = gameManager.timer.ToString("n2");
    }

    /// <summary>
    /// 攻撃までの時間をUIに反映する
    /// </summary>
    /// <param name="interval"></param>
    public void SetIntervalAttackTime(float interval)
    {
        imgIntervalAttackTime.fillAmount = 0;

        imgIntervalAttackTime.DOFillAmount(1, interval);
    }

    /// <summary>
    /// 必殺技ゲージ更新処理
    /// </summary>
    /// <param name="killCount"></param>
    public void SetIntervalSpecialMove(int killCount)
    {
        float value = Mathf.Clamp(killCount, 0f, maxValue);

        float setValue = value / maxValue;

        //imgSpecialMoveCount.fillAmount =  setValue;
        imgSpecialMoveCount.DOFillAmount(setValue, 0.5f);  // <= DOFillAmount(この値に, 何秒で)

        //if (value >= maxValue)
        //{
        //    //パーティクル生成
        //    generatedParticle = Instantiate(particleSpecialMoveGauge, particleTran, false);

        //    Debug.Log("作られたパーティクル：" + generatedParticle);

        //    generatedParticle.Play();

        //    Debug.Log("パーティクル・Play");
        //}
    }
}
