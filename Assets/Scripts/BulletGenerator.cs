using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

//Physics2D.OverlapCircleNonAlloc
//https://docs.unity3d.com/ja/2018.4/ScriptReference/Physics2D.OverlapCircleNonAlloc.html

public class BulletGenerator : MonoBehaviour
{
    [SerializeField] private Bullet bulletPrefab;

    [SerializeField] private int damage = 2;
    [SerializeField] private float speed = 1000.0f;
    [SerializeField] private float interval = 2.0f;
    public float Interval => interval;
    // <= DataSOから情報をもらう

    [SerializeField] private float radius = 2.0f;  // <= AttackRangeSizeSOから情報をもらう

    [SerializeField] private LayerMask enemyLayer;  //LayerMask設定をインスペクターで行う　コードを書くより[SerializeField]属性でインスペクターから設定を行なった方が便利

    [SerializeField] private bool isDrawGizmoOn;  //デバック用の機能のオンオフ切り替えを自分で作る

    private bool isShooting;  //弾の生成制御　撃っているかどうか

    private Animator anim;

    private UIManager uiManager;

    void Start()
    {
        //TODO Startメソッド内に書いてあるものはあとでSetUpBullet内に記述する

        TryGetComponent(out anim);
    }

    public void SetUpBulletGenerator(CharaController charaController, AttackRangeSizeSO.AttackRangeSize attackRangeSize, UIManager uiManager)
    {
        this.uiManager = uiManager;

        //各値を設定
        damage = charaController.attackPoint;
        interval = charaController.intervalAttackTime;
        this.radius = attackRangeSize.radius;

        uiManager.SetIntervalAttackTime(interval);
    }

    void Update()
    {
        if (isShooting)
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.A))
        {
            DetectEnemiesInRange();
        }
    }

    /// <summary>
    /// 攻撃範囲内に敵がいるか判定し、存在する場合はプレイヤーに最も近い敵を特定
    /// </summary>
    private void DetectEnemiesInRange()
    {
        ////OverlapCircleNonAllocの場合(ただし、将来的に非推奨)
        ////範囲内の全ての敵のコライダーを取得するための配列を用意
        //Collider2D[] hitColliders = new Collider2D[10];  //TODO []内に適切な値を入れる　今は仮の値
        //int numColliders = Physics2D.OverlapCircleNonAlloc(transform.position, radius, hitColliders, enemyLayer);

        ////攻撃範囲内に敵が見つからない場合、処理しない
        //if (numColliders == 0)
        //{
        //    return;
        //}

        //OverlapCircleAllの場合
        //範囲内の全ての敵のコライダーを取得
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);

        //攻撃範囲内に敵が見つからない場合、処理しない
        if (hitColliders.Length == 0)
        {
            Debug.Log("敵が見つかりません");
            return;
        }
        Debug.Log("敵を発見 : " + hitColliders.Length + "体");

        //最も近い敵とその距離の二乗を保存する変数
        Collider2D nearestEnemy = null;
        float nearestDistanceSqr = float.MaxValue;  //Sqrはmagnitudeの略　float.MaxValueはfloat型の最も大きい値(カーソルを合わせてみればわかる)

        //配列内の各敵に対して距離の二乗を計算し、最も近い敵を見つける(OverlapCircleNonAllocの場合はnumCollidersを使う)
        for (int i = 0; i < hitColliders.Length; i++)
        {
            //プレイヤーの位置と配列に入っている敵の位置を計算して、平方根にする
            float distanceSqr = (transform.position - hitColliders[i].transform.position).sqrMagnitude;
            //distanceSqr = (transform.position - hitColliders[i].transform.position).magnitude;
            //distanceSqr = Vector2.Distance(transform.position - hitColliders[i].transform.position);  //この3つは同じ処理だが、上に行くにつれ処理速度が速い

            //今回の距離と、現在までの最も近い距離を比較
            if (distanceSqr < nearestDistanceSqr)
            {
                nearestDistanceSqr = distanceSqr;
                nearestEnemy = hitColliders[i];
            }
        }

        //上記をLinqにした場合(変数は減り、for文も少なくなるため、可読性は高くなるが、負荷が上がる)
        //Collider2D nearestEnemy =
        //    hitColliders
        //    .OrderBy(hit => (transform.position - hit.transform.position).sqrMagnitude)
        //    .FirstOrDefault();  //hitColliders[0]

        //if (nearestEnemy == null)
        //{
        //    Debug.Log("敵が見つかりません");
        //    return;
        //}

        Debug.Log("最も近い敵が見つかりました : " + nearestEnemy.gameObject.name);

        //最も近い敵に対して弾を発射する
        isShooting = true;

        Vector2 direction = (nearestEnemy.transform.position - transform.position).normalized;

        StartCoroutine(GenerateBullet(direction));

        //弾の方向にアニメ同期
        anim.SetFloat("X", direction.x);
        anim.SetFloat("Y", direction.y);
    }

    /// <summary>
    /// 弾を発射
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private IEnumerator GenerateBullet(Vector2 direction)
    {
        Bullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        bullet.Shoot(speed, direction);

        uiManager.SetIntervalAttackTime(interval);

        yield return new WaitForSeconds(interval);

        isShooting = false;
    }

    /// <summary>
    /// 攻撃範囲の可視化
    /// </summary>
    void OnDrawGizmos()
    {
        if (!isDrawGizmoOn)
        {
            return;
        }

        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(transform.position, radius);
    }

    //TODO もし実装するなら　弾が攻撃範囲外に出た場合の処理
}
