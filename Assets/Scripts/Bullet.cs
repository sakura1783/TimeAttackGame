using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private int damage;
    public int Damage => damage;  //プロパティのget省略　ラムダ式

    private readonly float destroyTime = 1.5f;  //readonlyにすることで、値を変更できなくなる

    /// <summary>
    /// 弾の制御
    /// </summary>
    /// <param name="damage"></param>
    /// <param name="speed"></param>
    /// <param name="direction"></param>
    public void Shoot(float speed, Vector2 direction)
    {
        //this.damage = damage;  //TODO 必要ない場合は消す

        if (TryGetComponent(out Rigidbody2D rb))
        {
            rb.AddForce(direction * speed);
        }

        Destroy(gameObject, destroyTime);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == "Enemy")
        {
            Destroy(gameObject);

            //TODO もしここにDamageメソッドを書く場合
            //if (TryGetComponent(out EnemyController enemyController))
            //{
            //    enemyController.Damage();
            //}
        }
    }
}
