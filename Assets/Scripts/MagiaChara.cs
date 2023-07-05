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

    public void SetUpMagiaChara(GameManager gameManager)
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
                //TODO 攻撃

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
        //Collider2D hitColliders = Physics2D.OverlapCircleAll(transform.position, radius, 
    }

    //TODO 攻撃処理　BulletGenerator, Bulletの処理を書く　Updateでintervalを経過したら自動で攻撃

    //TODO もし実装するなら　必殺技ゲージが溜まったら、自動で必殺技を行う(かつ、敵キャラが一体でも生成されている場合)
}
