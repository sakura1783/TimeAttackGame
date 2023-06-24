using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    private CharaController chara;

    public ItemType itemType;

    //アイテム効果
    [SerializeField] private int curePoint;

    public enum ItemType
    {
        Hurt,
    }

    public void SetUpItem(GameManager gameManager)
    {
        chara = gameManager.CharaController;
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            ApplyItemEffect(itemType);

            Destroy(gameObject);
        }
    }

    /// <summary>
    /// アイテム効果を適用する
    /// </summary>
    /// <param name="itemType"></param>
    private void ApplyItemEffect(ItemType itemType)
    {
        switch (itemType)
        {
            case ItemType.Hurt:
                chara.hp += curePoint;
                break;
        }
    }
}
