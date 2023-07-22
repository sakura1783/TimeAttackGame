using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UIManager : MonoBehaviour
{
    [SerializeField] private Text txtTime;
    [SerializeField] private Text txtKillEnemyCount;

    [SerializeField] private GameManager gameManager;
    public GameManager GameManager => gameManager;

    [SerializeField] private Image imgIntervalAttackTime;

    //private BulletGenerator bulletGenerator;

    [SerializeField] private Image imgSpecialMoveCount;

    private int maxValue;  //FillAmountの最大値(必殺技発動のために必要な敵Kill数)
    private float value;  //FillAmountの現在値(現在の敵Kill数)

    //[SerializeField] private ParticleSystem particleSpecialMoveGauge;

    //private ParticleSystem generatedParticle;

    //[SerializeField] private Transform particleTran;

    //private bool isParticlePlay = false;

    //[SerializeField] private SpecialMove specialMove;
    //public SpecialMove SpecialMove => specialMove;

    public int specialMoveCount;  //必殺技を発動した回数

    //public int uiKillEnemyCount;  //UIゲージ更新に使う敵キル数

    private int specialMovePoint = 0;  //今までに獲得したspecialMovePoint

    public void SetUpUIManager()
    {
        //bulletGenerator = gameManager.CharaController.BulletGenerator;

        maxValue = gameManager.CharaController.intervalKillCountSpecialMove;

        imgSpecialMoveCount.fillAmount = 0;
    }

    void Update()
    {
        txtTime.text = gameManager.timer.ToString("n2");

        txtKillEnemyCount.text = gameManager.DataKillEnemyCount.ToString();

        ////もし必殺技を発動した回数が必殺技発動可能回数を上回ったら、Updateの以下の処理はしない
        //if (specialMoveCount >= gameManager.CharaController.maxSpecialMoveCount)
        //{
        //    return;
        //}

        ////もし必殺技発動中なら、Updateの以下の処理はしない(Sキーを押しても反応がなくなる)
        //if (specialMove.isSpecialMoveActive)
        //{
        //    return;
        //}

        ////もし現在の敵Kill数が必殺技発動に必要な敵Kill数以上になったら
        //if (value >= maxValue)
        //{
        //    if (Input.GetKeyDown(KeyCode.S))  // 左のようなコードは、Update内で使用されることが想定されている。よって、下のSetメソッドのif文の条件に書いても思い通りの挙動にならないので注意。
        //    {
        //        Debug.Log("Sキーが押されました");

        //        //uiKillCountを0にして、この後にSetIntervalSpecialMoveを行うことでUIのゲージをリセットする
        //        //gameManager.EnemyController.killCount = 0;  //これはgameManager.killEnemyCountの戻り値を反映したものなので、これに0を代入してしまうと、一生0のままになってしまうので、その後たとえ敵を倒したとしても、ゲージが更新されない。
        //        //uiKillEnemyCount = 0;
        //        specialMovePoint = 0;

        //        //必殺技ゲージ更新処理　必殺技発動後に更新処理を行うことで、ゲージを0にできる。ここに書かないと、敵がDestroyされた時にのみ、更新処理が動くので、必殺技を発動してもゲージが満タンのままになる
        //        SetIntervalSpecialMove(0);

        //        //パーティクル放出を止める
        //        generatedParticle.Stop();

        //        isParticlePlay = false;

        //        //必殺技発動
        //        specialMove.StartCoroutine(specialMove.UseSpecialMove(gameManager.CharaController.charaType));  //コルーチンメソッドを呼び出す場合、このような書式で記述する。

        //        //必殺技発動回数をカウント
        //        specialMoveCount++;
        //    }
        //}
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
    //public void SetIntervalSpecialMove(int addSpecialMovePoint)
    //{
    //    //value = Mathf.Clamp(gameManager.EnemyController.killCount, 0f, maxValue);
    //    //value = Mathf.Clamp(uiKillEnemyCount, 0f, maxValue);
    //    specialMovePoint += addSpecialMovePoint;
    //    value = Mathf.Clamp(specialMovePoint, 0f, maxValue);
    //    Debug.Log("addSpecialMovePoint : " + addSpecialMovePoint);

    //    float setValue = value / maxValue;

    //    //imgSpecialMoveCount.fillAmount =  setValue;
    //    imgSpecialMoveCount.DOFillAmount(setValue, 0.5f);  // <= DOFillAmount(この値に, 何秒で)

    //    Debug.Log("必殺技ゲージ更新しました" + setValue);

    //    if (value >= maxValue)
    //    {
    //        //もしまだパーティクルが1つも生成されていなかったら
    //        if (generatedParticle == null)
    //        {
    //            //パーティクル生成
    //            generatedParticle = Instantiate(particleSpecialMoveGauge, particleTran, false);
    //        }

    //        //もしまだパーティクルがPlayされていなかったら
    //        if (!isParticlePlay)
    //        {
    //            generatedParticle.Play();

    //            isParticlePlay = true;

    //            return;
    //        }

    //        ////この処理は敵がDestroyされる時だけに動くため、ここに書くと処理が動かない。
    //        // (Sキーを押して)isSKeyPressedがtrueになったら
    //        //if (isSKeyPressed)
    //        //{
    //        //    Debug.Log("Sキーが押されて、isSKeyPressedがtrueになりました");

    //        //    //パーティクル放出を止める
    //        //    generatedParticle.Stop();

    //        //    isParticlePlay = false;

    //        //    //UIのゲージを0にする
    //        //    killCount = 0;

    //        //    //必殺技発動
    //        //    specialMove.UseSpecialMove(gameManager.CharaController.charaType);
    //        //}
    //    }

    //    isParticlePlay = false;
    //    //Debug.Log("パーティクル・Playに続いて出てきたら想定外の動きになります");
    //}
}
