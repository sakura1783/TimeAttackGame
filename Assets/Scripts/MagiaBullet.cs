using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagiaBullet : MonoBehaviour
{
    private readonly float destroyTime = 1.5f;  //readonlyにすることで、値を変更できなくなる

    private EnemyController enemyController;

    /// <summary>
    /// 弾の制御
    /// </summary>
    /// <param name="speed"></param>
    /// <param name="direction"></param>
    public void Shoot(float speed, Vector2 direction)
    {
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

            //TODO 敵の被ダメージ処理　MagiaCharaの情報を持ってくる
            //enemyController.Damage();
        }
    }
}
