using UnityEngine;
using UnityEngine.AI;

public class NavMeshAgent2D : MonoBehaviour
{
    [Header("Steering")]
    public float speed = 1.0f;
    public float stoppingDistance = 0;

    [HideInInspector]//常にUnityエディタから非表示
    private Vector2 trace_area = Vector2.zero;
    [SerializeField] private EnemyDataSO.EnemyData enemyData;

    //エネミーを一時停止・再開するための変数
    private bool isStopped = false;
    public bool IsStopped { get; set; }

    public Vector2 destination
    {
        get { return trace_area; }
        set
        {
            trace_area = value;
            Trace(transform.position, value);
        }
    }
    public bool SetDestination(Vector2 target)
    {
        destination = target;
        return true;
    }

    private void Trace(Vector2 current, Vector2 target)
    {
        //もし、移動停止中なら以下の移動処理をしない
        if (isStopped)
        {
            return;
        }

        if (Vector2.Distance(current, target) <= stoppingDistance)
        {
            return;
        }

        // NavMesh に応じて経路を求める
        NavMeshPath path = new NavMeshPath();
        NavMesh.CalculatePath(current, target, NavMesh.AllAreas, path);

        Vector2 corner = path.corners[0];

        if (Vector2.Distance(current, corner) <= 0.05f)
        {
            if (path.corners.Length < 2)
            {
                corner = path.corners[0];

                return;
            }

            corner = path.corners[1];
        }

        transform.position = Vector2.MoveTowards(current, corner, speed * Time.deltaTime);
    }

    public void SetUpNavMeshAgent2D()
    {
        speed = enemyData.moveSpeed;
    }
}
