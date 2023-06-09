using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    private Animator anim;

    private NavMeshAgent2D navMeshAgent2D;

    [SerializeField] private CharaController charaController;

    void Start()
    {
        TryGetComponent(out anim);
        TryGetComponent(out navMeshAgent2D);
    }

    void Update()
    {
        navMeshAgent2D.destination = charaController.transform.position;  //destination = 目的地

        ChangeAnimDirection();
    }

    //TODO エネミーから見たキャラクターの位置によってアニメーション変更

    private void ChangeAnimDirection()
    {
        Vector2 direction = (charaController.transform.position - transform.position).normalized;

        anim.SetFloat("X", direction.x);
        anim.SetFloat("Y", direction.y);
    }
}
