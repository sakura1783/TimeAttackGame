using DG.Tweening.Core.Easing;
using UnityEngine;

//HandlerとUIManager_Specialの制御を行う、必殺技の管理者
public class SpecialMoveManager : MonoBehaviour
{
    private int availablePoints;  // 使用可能なポイント(キル数)

    //[SerializeField] private int debugPoint = 5;  // TODO デバッグ用。スペースキーを押すと、この数だけ増える

    private SpecialMoveHandler specialMoveHandler;

    private UIManager_Special uiManagerSpecial;

    //必殺技の管理用。必殺技のデータからもらう
    public int maxSpecialMoveCount;
    public int useSpecialMoveCount;
    private int interval;

    //パーティクル関係
    [SerializeField] private ParticleSystem particleSpecialMoveGauge;
    public ParticleSystem generatedParticle;
    [SerializeField] private Transform particleTran;
    private bool isParticlePlay = false;

    private bool isSetUpFinished = false;


    void Update()
    {
        if (!isSetUpFinished)
        {
            return;
        }

        //もし必殺技を発動した回数が必殺技発動可能回数を上回ったら、Updateの以下の処理はしない
        if (useSpecialMoveCount >= maxSpecialMoveCount)
        {
            return;
        }

        //もし必殺技発動中なら、Updateの以下の処理はしない(Sキーを押しても反応がなくなる)
        if (specialMoveHandler.isSpecialMoveActive)
        {
            return;
        }

        // TODO デバッグ用
        // if (Input.GetKeyDown(KeyCode.Space))
        // {
        //     AddPoint(debugPoint);
        // }

        //もし必殺技発動に必要なポイントを上回ったら
        if (availablePoints >= interval)
        {
            if (Input.GetKeyDown(KeyCode.S))
            {
                Debug.Log("Sキーが押されました");

                ActivateSpecialMove();

                //パーティクル放出を止める
                generatedParticle.Stop();

                isParticlePlay = false;
            }
        }
    }

    /// <summary>
    /// GameManagerから実行する
    /// </summary>
    /// <param name="specialMoveData"></param>
    /// <param name="gameManger"></param>
    /// <param name="enemyGenerator"></param>
    public void SetUpSpecialMoveManager(SpecialMoveData specialMoveData, GameManager gameManger, EnemyGenerator enemyGenerator)
    {
        maxSpecialMoveCount = specialMoveData.maxSpecialMoveCount;
        interval = specialMoveData.interval;

        if (TryGetComponent(out specialMoveHandler))
        {
            specialMoveHandler.SetUpHandler(specialMoveData , gameManger, enemyGenerator);

            Debug.Log("①完了");
        }

        if (TryGetComponent(out uiManagerSpecial))
        {
            uiManagerSpecial.SetUp(this);
        }

        isSetUpFinished = true;
    }

    /// <summary>
    /// 必殺技使用
    /// </summary>
    public void ActivateSpecialMove()
    {
        specialMoveHandler.PrepareSpecialMove();

        //必殺技のポイントを消費
        availablePoints = 0;

        useSpecialMoveCount++;

        UpdateUIState();
    }

    /// <summary>
    /// 必殺技ポイント(キル数)加算
    /// </summary>
    /// <param name="point"></param>
    public void AddPoint(int point)
    {
        if (useSpecialMoveCount >= maxSpecialMoveCount)
        {
            return;
        }

        //キル数加算の上、少ない方の値を現在値とし、かつ、上限値を超えないように制御する
        availablePoints = Mathf.Min(availablePoints + point, interval);

        UpdateUIState();
    }

    /// <summary>
    /// UI(必殺技ボタン、必殺技ゲージ)の制御
    /// </summary>
    private void UpdateUIState()
    {
        //bool isSwitch = availablePoints >= interval;  //下の処理はこれを省略している

        //uiManagerSpecial.UpdateButtonInteractable(availablePoints >= interval);

        float nextFillAmount = (float)availablePoints / interval;

        uiManagerSpecial.UpdateSpecialMoveGauge(nextFillAmount);

        if (nextFillAmount >= 1)
        {
            //ゲージが溜まったのでパーティクルを生成
            if (generatedParticle == null)
            {
                generatedParticle = Instantiate(particleSpecialMoveGauge, particleTran);

                generatedParticle.Play();

                isParticlePlay = true;
            }

            if (!isParticlePlay)
            {
                generatedParticle.Play();

                isParticlePlay = true;
            }
        }
    }
}
