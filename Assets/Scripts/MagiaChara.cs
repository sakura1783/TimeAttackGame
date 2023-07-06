using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class MagiaChara : MonoBehaviour
{
    private GameManager gameManager;

    private NavMeshAgent2D navMeshAgent2D;

    private Animator anim;

    [SerializeField] private EnemyController target;  //確認用にSerializeFieldをつけているので問題がなければSerializeFieldは消す

    private bool isCountingUp = false;

    //キャラの各情報
    private int attackPoint;
    private float interval;

    //弾の各情報
    private float radius;
    [SerializeField] private float speed;

    [SerializeField] private LayerMask enemyLayer;

    [SerializeField] private MagiaBullet bulletPrefab;

    public void SetUpMagiaChara(GameManager gameManager, CharaDataSO.CharaData charaData)
    {
        this.gameManager = gameManager;

        TryGetComponent(out anim);

        if (navMeshAgent2D == null)
        {
            gameObject.AddComponent<NavMeshAgent2D>();
        }
    }

    void Update()
    {
        if (navMeshAgent2D == null)
        {
            return;
        }

        //エネミーのListの中から一番近い敵を探す
        if (target == null)
        {
            float nearestDistance = float.MaxValue;

            foreach (EnemyController enemy in gameManager.EnemyGenerator.enemiesList)
            {
                float distance = Vector3.Distance(transform.position, enemy.transform.position);

                if (nearestDistance > distance)
                {
                    nearestDistance = distance;

                    target = enemy;
                }
            }
        }
        //上で探した一番近い敵の座標に向かって移動する
        navMeshAgent2D.destination = target.transform.position;

        ChangeAnimDirection();
    }

    /// <summary>
    /// アニメーション変更
    /// </summary>
    private void ChangeAnimDirection()
    {
        Vector2 direction = (gameManager.EnemyController.transform.position - transform.position).normalized;

        anim.SetFloat("X", direction.x);
        anim.SetFloat("Y", direction.y);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (!isCountingUp)
        {
            if (col.gameObject.CompareTag("Enemy"))
            {
                isCountingUp = true;

                StartCoroutine(PrepareAttack());
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (isCountingUp)
        {
            isCountingUp = false;
        }
    }

    /// <summary>
    /// 攻撃準備
    /// </summary>
    /// <returns></returns>
    private IEnumerator PrepareAttack()
    {
        float timer = 0;

        while (isCountingUp)
        {
            timer += Time.deltaTime;

            if (timer > interval)
            {
                DetectEnemiesInRange();

                timer = 0;
            }

            yield return null;
        }
    }

    private void Attack()
    {
        //gameManager.EnemyController.Damage(attackPoint);
    }

    /// <summary>
    /// 攻撃範囲内に敵がいるか判定し、存在する場合は最も近い敵を特定
    /// </summary>
    private void DetectEnemiesInRange()
    {
        //範囲内の全ての敵のコライダーを取得
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, enemyLayer);

        //範囲内に敵が見つからない場合、処理しない
        if (hitColliders.Length == 0)
        {
            return;
        }

        //最も近い敵とその距離の二乗を保存する変数
        Collider2D nearestEnemy = null;
        float nearestDistanceSqr = float.MaxValue;  //Sqrはmagnitudeの略

        //配列内の各敵に対して、距離の二乗を計算し、最も近い敵を見つける
        for (int i = 0; i < hitColliders.Length; i++)
        {
            //キャラの位置と配列に入っている敵の位置を計算して、平方根にする
            //Vector3.Distanceで2点間の距離を算出する場合、ルートの計算に時間がかかるので、単純に距離の遠近を比較したい場合はsqrMagnitudeを利用し、2乗値で比較するようにすると処理が高速に行える。
            float distanceSqr = (transform.position - hitColliders[i].transform.position).sqrMagnitude;

            //今回の距離と、現在までの最も近い距離を比較
            if (distanceSqr < nearestDistanceSqr)
            {
                nearestDistanceSqr = distanceSqr;
                nearestEnemy = hitColliders[i];
            }
        }

        //最も近い敵に対して弾を発射する
        Vector2 direction = (nearestEnemy.transform.position - transform.position).normalized;

        GenerateBullet(direction);

        //弾の方向にアニメ同期
        anim.SetFloat("X", direction.x);
        anim.SetFloat("Y", direction.y);
    }

    /// <summary>
    /// 弾を発射
    /// </summary>
    /// <param name="direction"></param>
    /// <returns></returns>
    private void GenerateBullet(Vector2 direction)
    {
        MagiaBullet bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);

        bullet.Shoot(speed, direction);

        gameManager.UiManager.SetIntervalAttackTime(interval);
    }

    //TODO もし実装するなら　必殺技ゲージが溜まったら、自動で必殺技を行う(かつ、敵キャラが一体でも生成されている場合)
}
