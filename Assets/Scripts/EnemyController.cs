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

    private GameManager gameManager;

    public void SetUpEnemyController(GameManager gameManager)
    {
        this.gameManager = gameManager;

        this.charaController = this.gameManager.charaController;
        Debug.Log("デバック2 : " + this.charaController);

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

            Debug.Log("if文が動きました");
        }

        //各値を設定
        hp = maxHp;

        Debug.Log("SetUpEnemyControllerメソッドが動きました");
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
    }
}
