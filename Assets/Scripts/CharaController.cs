using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaController : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private float speed = 5f;

    private float limitPosX = 13.65f;
    private float limitPosY = 7.55f;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private EnemyController enemy;

    public int attackPoint;
    public float intervalAttackTime;
    public int hp;

    public void SetUpCharaController(GameManager gameManager)
    {
        TryGetComponent(out anim);

        Debug.Log("1 : " + anim);

        this.gameManager = gameManager;
    }

    void Update()
    {
        if (gameManager == null)
        {
            return;
        }

        Move();
    }

    private void Move()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            anim.SetFloat("X", 0f);
            anim.SetFloat("Y", 1f);

            transform.position += transform.up * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            anim.SetFloat("X", 0f);
            anim.SetFloat("Y", -1f);

            transform.position -= transform.up * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            anim.SetFloat("X", -1f);
            anim.SetFloat("Y", 0f);

            transform.position -= transform.right * speed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            anim.SetFloat("X", 1f);
            anim.SetFloat("Y", 0f);

            transform.position += transform.right * speed * Time.deltaTime;
        }

        //TODO 止まっている時はアニメーションを止める(その時向いている方向で)

        float posX = Mathf.Clamp(transform.position.x, -limitPosX, limitPosX);
        float posY = Mathf.Clamp(transform.position.y, -limitPosY, limitPosY);

        transform.position = new Vector2(posX, posY);
    }

    //private void OnTriggerEnter2D(Collider2D col)
    //{
    //    if (col.tag == "Enemy")
    //    {
    //        //Destroy(col.gameObject);

    //        //enemyGenerator.enemiesList.Remove();

    //        //gameManager.killEnemyCount++;

    //        //gameManager.GameClear();
    //    }
    //}

    //private IEnumerator PrepareAttack()
    //{
    //    timer += Time.deltaTime;

    //    if (timer >= intervalAttackTime && Input.GetKey(KeyCode.A))
    //    {
    //        Attack();

    //        timer = 0;
    //    }

    //    yield return null;
    //}

    //private void Attack()
    //{
    //    //Debug.Log("攻撃");

    //    ////LayerMask.NameToLayerで、指定した名前のレイヤーの番号(整数値)を取得する。
    //    //int enemyLayer = LayerMask.NameToLayer("Enemy");
    //    //Debug.Log("レイヤー番号 : " + enemyLayer);

    //    ////OverlapCircleColliderで指定された範囲内の指定されたレイヤーを見つける。今回の場合は、(中心位置、円の半径、レイヤー番号(この場合はEnemy))
    //    //Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, 2, enemyLayer);
    //    ////Debug.Log("ヒットしたコライダー : " + hitColliders);  // <= 配列は文字列に直接変換できないので、代わりに配列の要素数などを表示するようにするといい。
    //    //Debug.Log("ヒットしたコライダーの数 : " + hitColliders.Length);

    //    ////範囲内のコライダーを順にチェック
    //    //foreach (Collider2D col in hitColliders)
    //    //{
    //    //    Debug.Log("ヒットしたコライダー : " + col.gameObject.name);

    //    //    if (TryGetComponent(out enemy))
    //    //    {
    //    //        Debug.Log("ダメージを与えます");
    //    //        enemy.Damage(2);  //TODO 引数にプレイヤーの攻撃力を渡す
    //    //    }
    //    //}
    //}

    //private void OnTriggerStay2D(Collider2D col)
    //{
    //    タグをEnemyかどうか比較。戻り値はtrue / false
    //    if (col.CompareTag("Enemy"))
    //    {
    //        if (col.TryGetComponent(out enemy))
    //        {
    //            Debug.Log("ヒットした敵 : " + col.gameObject.name);

    //            enemy.Damage(2);
    //        }
    //    }
    //}
}
