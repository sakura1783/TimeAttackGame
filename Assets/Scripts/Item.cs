using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    //private CharaController chara;

    public ItemType itemType;

    //アイテム効果
    [SerializeField] private int curePoint;

    /// <summary>
    /// アイテムの種類
    /// </summary>
    public enum ItemType
    {
        Hurt,
    }

    public void SetUpItem(GameManager gameManager)
    {
        //chara = gameManager.CharaController;

        //Debug.Log("chara1 : " + chara);
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.TryGetComponent(out CharaController chara)) // <= col.がないとItemクラスに対してTryGetComponentすることになるので、CharaController型の情報が取得できずchara変数には何も入らないので注意！
        {
            ApplyItemEffect(itemType, chara);

            Destroy(gameObject);
        }
    }

    /// <summary>
    /// アイテム効果を適用する
    /// </summary>
    /// <param name="itemType"></param>
    private void ApplyItemEffect(ItemType itemType, CharaController charaController)
    {
        switch (itemType)
        {
            case ItemType.Hurt:
                charaController.hp += curePoint;
                break;
        }
    }
}
