using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataBaseManager : MonoBehaviour
{
    public static DataBaseManager instance;

    public CharaDataSO charaDataSO;
    public EnemyDataSO enemyDataSO;
    public AttackRangeSizeSO attackRangeSizeSO;

    void Awake()
    {
        if (instance == null)
        {
            instance = this;

            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// AttackRangeSize取得用のメソッド
    /// </summary>
    /// <param name="searchAttackRangeType"></param>
    /// <returns></returns>
    public AttackRangeSizeSO.AttackRangeSize GetAttackRangeSize(AttackRangeType searchAttackRangeType)
    {
        foreach (AttackRangeSizeSO.AttackRangeSize attackRangeSize in attackRangeSizeSO.attackRangeSizeList)
        {
            if (attackRangeSize.attackRangeType == searchAttackRangeType)
            {
                return attackRangeSize;
            }
        }

        return null;

        //return attackRangeSizeSO.attackRangeSizeList.Find(attackRangeSize => attackRangeSize.attackRangeType == searchAttackRangeType);
    }
}
