using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaController : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private float speed = 5f;

    private float limitPosX = 13.65f;
    private float limitPosY = 7.55f;

    [SerializeField] private GameManager gameManager;

    public int attackPoint;
    public float intervalAttackTime;
    public int hp = 10;  //HPはどのキャラも同じ値
    //[SerializeField] private AttackRangeType attackRangeType;
    [SerializeField] private int maxSpecialMoveCount;
    public int intervalKillCountSpecialMove;
    [SerializeField] private int durationSpecialMove;

    private CharaDataSO.CharaData charaData;

    private BulletGenerator bulletGenerator;
    //public BulletGenerator BulletGenerator => bulletGenerator;

    [SerializeField] private FloatingMessage floatingMessagePrefab;

    [SerializeField] private Transform charaFloatingMessageTran;

    public void SetUpCharaController(GameManager gameManager, CharaDataSO.CharaData charaData, UIManager uiManager)
    {
        this.charaData = charaData;

        //各値の設定  //TODO 他に設定項目があれば追加する
        attackPoint = this.charaData.attackPower;
        intervalAttackTime = this.charaData.intervalAttackTime;
        //attackRangeType = this.charaData.attackRangeType;
        maxSpecialMoveCount = this.charaData.maxSpecialMoveCount;
        intervalKillCountSpecialMove = this.charaData.intervalKillCountSpecialMove;
        durationSpecialMove = this.charaData.durationSpecialMove;

        TryGetComponent(out anim);

        if (TryGetComponent(out bulletGenerator))
        {
            AttackRangeSizeSO.AttackRangeSize attackRangeSize = DataBaseManager.instance.GetAttackRangeSize(charaData.attackRangeType);

            bulletGenerator.SetUpBulletGenerator(this, attackRangeSize, uiManager);
            //bulletGenerator.SetUpBulletGenerator(this, DataBaseManager.instance.GetAttackRangeSize(charaData.attackRangeType));
        }

        this.gameManager = gameManager;
    }

    void Update()
    {
        if (gameManager == null)
        {
            return;
        }

        Move();
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            anim.SetFloat("X", 0f);
            anim.SetFloat("Y", 1f);

            transform.position += transform.up * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            anim.SetFloat("X", 0f);
            anim.SetFloat("Y", -1f);

            transform.position -= transform.up * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            anim.SetFloat("X", -1f);
            anim.SetFloat("Y", 0f);

            transform.position -= transform.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            anim.SetFloat("X", 1f);
            anim.SetFloat("Y", 0f);

            transform.position += transform.right * speed * Time.deltaTime;
        }

        //TODO 止まっている時はアニメーションを止める(その時向いている方向で)

        float posX = Mathf.Clamp(transform.position.x, -limitPosX, limitPosX);
        float posY = Mathf.Clamp(transform.position.y, -limitPosY, limitPosY);

        transform.position = new Vector2(posX, posY);
    }

    /// <summary>
    /// 攻撃(ダメージ)を受けた際の処理
    /// </summary>
    /// <param name="damage"></param>
    public void ReceiveDamage(int damage)
    {
        Debug.Log("プレイヤーがダメージを受けました");

        hp -= damage;

        //フロート表示を行うCharaの子のCanvasのTransformを取得
        Canvas canvas = GetComponentInChildren<Canvas>();

        if (canvas != null)
        {
            charaFloatingMessageTran = canvas.transform;
        }

        CreateCharaFloatingDamage(damage);

        //TODO HPが0になったらキャラをDestroyする。その際、Missingエラーが出るのでそこも処理する
        //if (hp <= 0)
        //{
        //    Destroy(gameObject);

        //    Debug.Log("Game Over...");

        //    //TODO ゲームオーバー処理　メソッドを作る
        //}
    }

    /// <summary>
    /// プレイヤーの被ダメージのフロート表示生成
    /// </summary>
    /// <param name="point"></param>
    private void CreateCharaFloatingDamage(int point)
    {
        FloatingMessage floatingMessage = Instantiate(floatingMessagePrefab, charaFloatingMessageTran, false);

        //生成したフロート表示の設定用メソッドを実行。引数として、バレットの攻撃力値とフロート表示の種類を設定して渡す
        floatingMessage.DisplayFloatingMessage(point, FloatingMessage.FloatingMessageType.Damage);
    }

    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (col.tag == "Enemy")
    //    {
    //        //Destroy(col.gameObject);

    //        //enemyGenerator.enemiesList.Remove();

    //        //gameManager.killEnemyCount++;

    //        //gameManager.GameClear();
    //    }
    //}

    //private IEnumerator PrepareAttack()
    //{
    //    timer += Time.deltaTime;

    //    if (timer >= intervalAttackTime && Input.GetKey(KeyCode.A))
    //    {
    //        Attack();

    //        timer = 0;
    //    }

    //    yield return null;
    //}

    //private void Attack()
    //{
    //    //Debug.Log("攻撃");

    //    ////LayerMask.NameToLayerで、指定した名前のレイヤーの番号(整数値)を取得する。
    //    //int enemyLayer = LayerMask.NameToLayer("Enemy");
    //    //Debug.Log("レイヤー番号 : " + enemyLayer);

    //    ////OverlapCircleColliderで指定された範囲内の指定されたレイヤーを見つける。今回の場合は、(中心位置、円の半径、レイヤー番号(この場合はEnemy))
    //    //Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 2, enemyLayer);
    //    ////Debug.Log("ヒットしたコライダー : " + hitColliders);  // <= 配列は文字列に直接変換できないので、代わりに配列の要素数などを表示するようにするといい。
    //    //Debug.Log("ヒットしたコライダーの数 : " + hitColliders.Length);

    //    ////範囲内のコライダーを順にチェック
    //    //foreach (Collider2D col in hitColliders)
    //    //{
    //    //    Debug.Log("ヒットしたコライダー : " + col.gameObject.name);

    //    //    if (TryGetComponent(out enemy))
    //    //    {
    //    //        Debug.Log("ダメージを与えます");
    //    //        enemy.Damage(2);  //TODO 引数にプレイヤーの攻撃力を渡す
    //    //    }
    //    //}
    //}

    //private void OnTriggerStay2D(Collider2D col)
    //{
    //    タグをEnemyかどうか比較。戻り値はtrue / false
    //    if (col.CompareTag("Enemy"))
    //    {
    //        if (col.TryGetComponent(out enemy))
    //        {
    //            Debug.Log("ヒットした敵 : " + col.gameObject.name);

    //            enemy.Damage(2);
    //        }
    //    }
    //}
}
