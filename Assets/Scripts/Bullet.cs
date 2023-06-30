using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    //private int damage;
    //public int Damage => damage;  //プロパティのget省略　ラムダ式

    private readonly float destroyTime = 1.5f;  //readonlyにすることで、値を変更できなくなる

    private EnemyController enemyController;

    /// <summary>
    /// 弾の制御
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="speed"></param>
    /// <param name="direction"></param>
    public void Shoot(float speed, Vector2 direction)
    {
        //this.damage = damage;

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
            enemyController.Damage(enemyController.charaController.attackPoint);

            Debug.Log("敵が受けたダメージ：" + enemyController.charaController.attackPoint);
        }
    }
}
