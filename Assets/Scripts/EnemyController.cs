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

    public void SetUpEnemyController(CharaController charaController)
    {
        this.charaController = charaController;

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

        ChangeAnimDirection();
    }

    /// <summary>
    /// 設定
    /// </summary>
    /// <param name="enemyData"></param>
    private void SetUpEnemy(EnemyDataSO.EnemyData enemyData)
    {

    }

    private void ChangeAnimDirection()
    {
        Vector2 direction = (charaController.transform.position - transform.position).normalized;

        anim.SetFloat("X", direction.x);
        anim.SetFloat("Y", direction.y);
    }
}
