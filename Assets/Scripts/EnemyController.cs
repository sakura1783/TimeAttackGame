using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Animator anim;

    private NavMeshAgent2D navMeshAgent2D;

    public CharaController charaController;

    [SerializeField] private EnemyDataSO.EnemyData enemyData;

    public int maxHp;
    public int hp;
    private int itemDropRate;
    [SerializeField] private int attackPower;
    [SerializeField] private float interval;

    private GameManager gameManager;

    private ItemGenerator itemGenerator;

    private bool isCountingUp = false;

    [SerializeField] private FloatingMessage floatingMessagePrefab;

    [SerializeField] private Transform enemyFloatingMessageTran;

    //private Vector2 enemyPos;
    //public Vector2 EnemyPos => enemyPos;

    private UIManager uiManager;

    private bool isPaused = false;  //移動やアニメーションが一時停止中かどうか

    public void SetUpEnemyController(GameManager gameManager)
    {
        this.gameManager = gameManager;

        this.charaController = this.gameManager.CharaController;

        TryGetComponent(out anim);
        //TryGetComponent(out this.navMeshAgent2D);

        if (navMeshAgent2D == null)
        {
            //var navMeshAgent2D = gameObject.AddComponent<NavMeshAgent2D>();
            //Debug.Log("1 : " + this.navMeshAgent2D);

            //this.navMeshAgent2D = navMeshAgent2D;
            //Debug.Log("2 : " + this.navMeshAgent2D);

            navMeshAgent2D = gameObject.AddComponent<NavMeshAgent2D>();
            Debug.Log(navMeshAgent2D);
        }

        uiManager = this.gameManager.UiManager;

        itemGenerator = gameManager.ItemGenerator;

        //EnemyゲームオブジェクトからSlider_LifeGaugeを探す
        GameObject sliderLifeGauge = transform.GetChild(2).GetChild(0).gameObject;

        //上で取得したGameObject型のsliderLifeGauge変数をTryGetComponentしてLifeGauge型の情報を取得、その後、LifeGaugeのSetUpメソッドを実行
        if (sliderLifeGauge.TryGetComponent(out LifeGauge lifeGauge))
        {
            lifeGauge.SetUpLifeGauge(this);
        }

        //各値を設定
        hp = maxHp;
    }

    void Update()
    {
        if (navMeshAgent2D == null)
        {
            return;
        }

        //もし、移動やアニメーションが一時停止中の場合、以下の移動処理とアニメ遷移処理は行わない
        if (isPaused)
        {
            return;
        }

        navMeshAgent2D.destination = charaController.transform.position;  //destination = 目的地

        ChangeAnimDirection();
    }

    /// <summary>
    /// アニメーション変更
    /// </summary>
    private void ChangeAnimDirection()
    {
        Vector2 direction = (charaController.transform.position - transform.position).normalized;

        anim.SetFloat("X", direction.x);
        anim.SetFloat("Y", direction.y);
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (!isCountingUp)
        {
            if (col.gameObject.tag == "Player")
            {
                isCountingUp = true;

                StartCoroutine(PrepareAttack());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        isCountingUp = false;
    }

    /// <summary>
    /// 攻撃準備
    /// </summary>
    /// <returns></returns>
    private IEnumerator PrepareAttack()
    {
        //時間停止中は以下の攻撃準備処理を行わない
        if (!isPaused)
        {
            float timer = 0;

            while (isCountingUp)
            {
                timer += Time.deltaTime;

                if (timer >= interval)
                {
                    Attack();

                    timer = 0;
                }

                yield return null;
            }
        }
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    private void Attack()
    {
        Debug.Log("敵の攻撃");

        //プレイヤーダメージ処理
        charaController.ReceiveDamage(attackPower);
    }

    /// <summary>
    /// 攻撃を受けた際のダメージ処理
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage)
    {
        hp -= damage;

        //フロート表示を行うEnemyの子のCanvasのTransformを取得
        Canvas canvas = GetComponentInChildren<Canvas>();

        if (canvas != null)
        {
            enemyFloatingMessageTran = canvas.transform;
        }

        //フロート表示生成
        CreateEnemyFloatingDamage(damage);

        if (hp <= 0)
        {
            //enemyPos = GetEnemyPos();

            //アイテムドロップ
            itemGenerator.GenerateItem(gameObject.transform);

            Destroy(gameObject);

            //リストから削除
            gameManager.EnemyGenerator.enemiesList.Remove(this);

            //倒した敵の数をカウントアップ
            int killCount = gameManager.AddKillEnemyCount();

            //もし必殺技を発動した回数が必殺技発動可能回数を上回ってなかったら
            if (uiManager.specialMoveCount < charaController.maxSpecialMoveCount)
            {
                //必殺技ゲージ更新
                uiManager.SetIntervalSpecialMove();
            }
        }
    }

    /// <summary>
    /// 敵の被ダメージのフロート表示生成
    /// </summary>
    private void CreateEnemyFloatingDamage(int point)
    {
        FloatingMessage floatingMessage = Instantiate(floatingMessagePrefab, enemyFloatingMessageTran, false);

        //生成したフロート表示の設定用メソッドを実行。引数として、バレットの攻撃力値とフロート表示の種類を指定して渡す
        floatingMessage.DisplayFloatingMessage(point, FloatingMessage.FloatingMessageType.Damage);
    }

    /// <summary>
    /// 移動の一時停止
    /// </summary>
    public void PauseMove()
    {
        if (navMeshAgent2D != null)  //これを書くことで、万が一navMeshAgent2Dがnullだった場合に、エラーを防げる
        {
            navMeshAgent2D.IsStopped = true;

            isPaused = true;
        }
    }

    /// <summary>
    /// 移動再開
    /// </summary>
    public void ResumeMove()
    {
        if (navMeshAgent2D != null)
        {
            navMeshAgent2D.IsStopped = false;

            isPaused = false;
        }
    }

    /// <summary>
    /// アニメーション一時停止
    /// </summary>
    public void PauseAnimation()
    {
        if (anim != null)
        {
            anim.enabled = false;

            isPaused = true;
        }
    }

    /// <summary>
    /// アニメーション再開
    /// </summary>
    public void ResumeAnimation()
    {
        if (anim != null)
        {
            anim.enabled = true;

            isPaused = false;
        }
    }

    /// <summary>
    /// 現在のエネミーの位置を返す(アイテム生成時用　アイテム生成位置に使用する)
    /// </summary>
    /// <returns></returns>
    //private Vector2 GetEnemyPos()
    //{
    //    Debug.Log("transformの値：" + this.transform);

    //    return this.transform.position;
    //}
}
