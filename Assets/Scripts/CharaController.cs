using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharaController : MonoBehaviour
{
    private Animator anim;

    [SerializeField] private float speed = 5f;

    private float limitPosX = 10.45f;
    private float limitPosY = 4.55f;

    [SerializeField] private GameManager gameManager;

    [SerializeField] private EnemyController enemy;

    [SerializeField] private float timer = 0;

    [SerializeField] private int attackPoint;
    [SerializeField] private float intervalAttackTime;

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

        StartCoroutine(PrepareAttack());
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

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == "Enemy")
        {
            Destroy(col.gameObject);

            //enemyGenerator.enemiesList.Remove();

            gameManager.killEnemyCount++;

            gameManager.GameClear();
        }
    }

    private IEnumerator PrepareAttack()
    {
        timer += Time.deltaTime;

        if (timer >= intervalAttackTime && Input.GetKey(KeyCode.A))
        {
            Attack();

            timer = 0;
        }

        yield return null;
    }

    private void Attack()
    {
        Debug.Log("攻撃");

        //TODO if (コライダーの範囲内に敵がいたら)
        //enemy.Damage(attackPoint);
    }
}
