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

    private GameManager gameManager;

    [SerializeField] private Transform floatingDamageTran;

    [SerializeField] private FloatingMessage floatingMessagePrefab;

    private ItemGenerator itemGenerator;

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
            //Debug.Log("Updateメソッドがreturnされました");

            return;
        }

        navMeshAgent2D.destination = charaController.transform.position;  //destination = 目的地
        //Debug.Log("デバック : " + charaController);

        ChangeAnimDirection();
    }

    /// <summary>
    /// 設定
    /// </summary>
    /// <param name="enemyData"></param>
    private void SetUpEnemy(EnemyDataSO.EnemyData enemyData)
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

    /// <summary>
    /// 攻撃を受けた際のダメージ処理
    /// </summary>
    /// <param name="damage"></param>
    public void Damage(int damage)
    {
        hp -= damage;
        Debug.Log("Enemyがダメージを受けました");

        //フロート表示生成
        CreateFloatingMessage(damage);

        if (hp <= 0)
        {
            Destroy(gameObject);

            //アイテムドロップ
            itemGenerator.GenerateItem();
        }

        //TODO 破壊する時は乱数でアイテムを落とすか落とさないか確認する　落とす時はGenerateメソッドをよぶ
    }
    /// <summary>
    /// フロート表示の生成
    /// </summary>
    public void CreateFloatingMessage(int point)
    {
        FloatingMessage floatingMessage = Instantiate(floatingMessagePrefab, floatingDamageTran, false);

        //生成したフロート表示の設定用メソッドを実行。引数として、バレットの攻撃力値とフロート表示の種類を指定して渡す
        floatingMessage.DisplayFloatingMessage(point, FloatingMessage.FloatingMessageType.Damage);
    }
}
