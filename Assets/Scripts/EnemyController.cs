using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Animator anim;

    private NavMeshAgent2D navMeshAgent2D;

    [SerializeField] private CharaController charaController;

    [SerializeField] private EnemyDataSO.EnemyData enemyData;

    [SerializeField] private int maxHp;
    [SerializeField] private int hp;
    private int itemDropRate;
    [SerializeField] private int attackPower;
    [SerializeField] private float interval;

    private GameManager gameManager;

    private ItemGenerator itemGenerator;

    private bool isCountingUp = false;

    public void SetUpEnemyController(GameManager gameManager)
    {
        this.gameManager = gameManager;

        this.charaController = this.gameManager.charaController;

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

        itemGenerator = gameManager.ItemGenerator;

        //各値を設定
        hp = maxHp;
    }

    void Update()
    {
        if (navMeshAgent2D == null)
        {
            return;
        }

        navMeshAgent2D.destination = charaController.transform.position;  //destination = 目的地

        ChangeAnimDirection();
    }

    /// <summary>
    /// 設定
    /// </summary>
    /// <param name="enemyData"></param>
    private void SetUpEnemyController(EnemyDataSO.EnemyData enemyData)
    {

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

            if (timer >= interval)
            {
                Attack();

                timer = 0;
            }

            yield return null;
        }
    }

    /// <summary>
    /// 攻撃
    /// </summary>
    private void Attack()
    {
        Debug.Log("敵の攻撃");

        //TODO プレイヤーダメージ処理
        charaController.ReceiveDamage(attackPower);
    }

    /// <summary>
    /// 攻撃を受けた際のダメージ処理
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage)
    {
        hp -= damage;
        Debug.Log("Enemyがダメージを受けました");

        //フロート表示生成
        gameManager.CreateFloatingMessage(damage);

        if (hp <= 0)
        {
            Destroy(gameObject);

            //アイテムドロップ　ItemGeneratorのGenerateItemメソッドを実行
            itemGenerator.GenerateItem();
        }
    }
    
}
