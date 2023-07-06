using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagiaBullet : MonoBehaviour
{
    private readonly float destroyTime = 1.5f;  //readonlyにすることで、値を変更できなくなる

    private EnemyController enemyController;

    private int attackPoint;

    /// <summary>
    /// 弾の制御
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="direction"></param>
    public void Shoot(float speed, Vector2 direction, int attackPoint)
    {
        this.attackPoint = attackPoint;

        if (TryGetComponent(out Rigidbody2D rb))
        {
            rb.AddForce(direction * speed);
        }

        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out enemyController))
        {
            Destroy(gameObject);

            //敵の被ダメージ処理
            enemyController.Damage(attackPoint);
        }
    }
}
