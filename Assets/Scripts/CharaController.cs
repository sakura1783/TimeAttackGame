using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaController : MonoBehaviour
{
    //アニメーション関係
    private Animator anim;
    //private string overrideClipName = "Chara_0";  //これで合ってる？
    //private AnimatorOverrideController overrideController;

    [SerializeField] private float speed = 5f;

    private float limitPosX = 13.65f;
    private float limitPosY = 7.55f;

    [SerializeField] private GameManager gameManager;

    public CharaType charaType;
    public int attackPoint;
    public float intervalAttackTime;
    public int maxHp = 10;
    public int hp;  //HPはどのキャラも同じ値
    //[SerializeField] private AttackRangeType attackRangeType;
    public int maxSpecialMoveCount;
    public int intervalKillCountSpecialMove;
    public int durationSpecialMove;
    //[SerializeField] private AnimatorOverrideController charaAnim;  //キャラのAnimatorController

    private CharaDataSO.CharaData charaData;
    public CharaDataSO.CharaData CharaData => charaData;

    private BulletGenerator bulletGenerator;
    //public BulletGenerator BulletGenerator => bulletGenerator;

    [SerializeField] private FloatingMessage floatingMessagePrefab;

    [SerializeField] private Transform charaFloatingMessageTran;

    public void SetUpCharaController(GameManager gameManager, CharaDataSO.CharaData charaData, UIManager uiManager)
    {
        hp = maxHp;

        this.charaData = charaData;

        //各値の設定
        charaType = this.charaData.charaType;
        attackPoint = this.charaData.attackPower;
        intervalAttackTime = this.charaData.intervalAttackTime;
        //attackRangeType = this.charaData.attackRangeType;
        maxSpecialMoveCount = this.charaData.maxSpecialMoveCount;
        intervalKillCountSpecialMove = this.charaData.intervalKillCountSpecialMove;
        durationSpecialMove = this.charaData.durationSpecialMove;
        //charaAnim = this.charaData.charaAnim;

        if (TryGetComponent(out anim))
        {
            SetUpAnimation();
        }

        //ライフゲージの設定
        GameObject sliderLifeGauge = transform.GetChild(1).GetChild(0).gameObject;
        if (sliderLifeGauge.TryGetComponent(out PlayerLifeGauge playerLifeGauge))
        {
            playerLifeGauge.SetUpLifeGauge(this);
        }

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
        if (gameManager.isGameOver)
        {
            return;
        }

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
        hp -= damage;
        hp = Mathf.Clamp(hp, 0, maxHp);

        //フロート表示を行うCharaの子のCanvasのTransformを取得
        Canvas canvas = GetComponentInChildren<Canvas>();

        if (canvas != null)
        {
            charaFloatingMessageTran = canvas.transform;  //これを書かないと生成する位置がまだ指定されていないので、canvasの子として生成されなくなり、見えない
        }

        CreateCharaFloatingDamage(damage);

        if (hp <= 0 && !gameManager.isGameOver)
        {
            gameManager.GameOver();
        }
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

    /// <summary>
    /// アイテム(Hurt)取得時のフロート表示生成
    /// </summary>
    /// <param name="curePoint"></param>
    public void CreateFloatingRecovery(int curePoint)
    {
        //0番目の子のCanvasゲームオブジェクトを取得
        GameObject canvas = transform.GetChild(0).gameObject;

        //上で取得したcanvas変数を、TryGetComponentして、それがCanvas型だったらcanvasTran変数に入れ、それのtransformをcharaFloatingMessageTranに代入する
        if (canvas.TryGetComponent(out Canvas canvasTran))
        {
            charaFloatingMessageTran = canvasTran.transform;  //ここでcharaFloatingMessageTranに位置の情報を入れないと、canvasの子としてフロート表示が生成されなくなり、見えないので注意。
        }

        FloatingMessage floatingMessage = Instantiate(floatingMessagePrefab, charaFloatingMessageTran, false);

        floatingMessage.DisplayFloatingMessage(curePoint, FloatingMessage.FloatingMessageType.Recovery);
    }

    /// <summary>
    /// AnimatorControllerをAnimatorOverrideControllerを利用して変更
    /// </summary>
    private void SetUpAnimation()
    {
        //このキャラのCharaData内にアニメーション用のデータがあるか確認する
        if (charaData.charaOverrideController != null)
        {
            //アニメーションのデータがある場合には、アニメーションを上書きする
            anim.runtimeAnimatorController = charaData.charaOverrideController;
        }
    }

    ///// <summary>
    ///// 
    ///// </summary>
    //private void SetUpAnimation()
    //{
    //    if (TryGetComponent(out anim))
    //    {
    //        //新しくAnimatorOverrideControllerのインスタンスを作成し、そのruntimeAnimatorController変数に、現在のAnimatorControllerのruntimeAnimatorController変数を代入する。
    //        //上書きする方(overrideController.runtimeAnimatorController)にも現在の情報(anim.runtimeAnimatorController)を入れておく必要があるため。
    //        overrideController = new AnimatorOverrideController();
    //        overrideController.runtimeAnimatorController = anim.runtimeAnimatorController; //コントローラーを切り替えするには、Animator.RuntimeAnimatorControllerを使う。

    //        //現在のAnimatorControllerのruntimeAnimatorControllerに対して新しく作成したAnimatorOverrideControllerを代入して更新する。この時点で、一旦、まったく同じ内容の情報で更新が入る。
    //        //この処理を行っておくことで、AnimatorControllerが再生処理を行う時、AnimatorOverrideControllerの設定を利用することができる状態になる。
    //        anim.runtimeAnimatorController = overrideController;

    //        //AnimatorStateInfo型の配列を用意し、anim.layerCount変数だけ要素番号を取得しておく。
    //        AnimatorStateInfo[] layerInfo = new AnimatorStateInfo[anim.layerCount]; //AnimatorStateInfo(現在または次のステートに関する情報), Animator.layerCount(コントローラー内のレイヤーの数を返す。(Animatorビュー内左上のタブLayers))

    //        for (int i = 0; i < anim.layerCount; i++)
    //        {
    //            //GetCurrentAnimatorStateInfoでAnimatorの現在の状態からデータを取得する(例えば、速度、長さ、名前、その他の変数など)。それを用意しておいた空の状態のlayerInfo配列に代入して保持しておく。
    //            layerInfo[i] = anim.GetCurrentAnimatorStateInfo(i);
    //        }

    //        //overrideController変数はAnimatorクラスが用意しているDictionary型の変数。Keyはstring型で定義されているので、文字列の値を指定することでValueの値を更新できる。
    //        //今回はstring型のoverrideClipName変数を定義してあるので、この値をKeyとして指定し、charaData.charaAnim変数の値をValueとして代入処理している。
    //        //この処理により、overrideControllerに設定しているアニメの情報を、今回利用するキャラ専用のアニメの情報に更新する。
    //        overrideController[overrideClipName] = charaData.charaAnim;

    //        //220行目でoverrideControllerの内容を更新し、現在利用しているキャラのデータのアニメ情報に更新して置き換えたので、それを再度、runtimeAnimatorControllerに適用し、更新したアニメの情報を設定する。
    //        //overrideControllerに情報を設定しただけでは反映されないため、設定後、この更新の処理を行うことで反映・適用される。
    //        anim.runtimeAnimatorController = overrideController;

    //        ////ここからの処理はなくてもアニメの変更手続き自体は終了しているが、更新したアニメがすぐに適用されなかったり、途中だったアニメが最初から再生されてしまうと違和感を感じるため、以下の処理を書いている。

    //        //このUpdateメソッドはAnimatorクラスに用意されているメソッド(通常のUpdateメソッドではない)。
    //        //アニメーションの更新設定を反映させる処理。この処理を入れておくことで設定した新しいアニメが即座に適用される。
    //        anim.Update(0.0f);

    //        //アニメーションの状態情報を使用し、アニメーターの各レイヤーで再生中のアニメーションを復元する。(アニメーターのレイヤーがわからない場合は、Animator layerで調べると記事が出てきます)
    //        for (int i = 0; i < anim.layerCount; i++)
    //        {
    //            //layerInfo[i].fullPathHash変数は、再生中のアニメーションのフルパス(ファイルなどの所在(ステートの階層構造))のハッシュ値(元データから特定の計算により求められた適当な値)(int型)を取得し、
    //            //i変数はレイヤーのインデックス(0の場合、BaseLayer)、layerInfo[i].normalizedTime変数はアニメーションの再生時間を正規化した値を表す。
    //            //これらの情報を利用して、各レイヤーで再生中のアニメーションを指定した時間位置から再開する。
    //            anim.Play(layerInfo[i].fullPathHash, i, layerInfo[i].normalizedTime);
    //        }
    //    }
    //}

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
