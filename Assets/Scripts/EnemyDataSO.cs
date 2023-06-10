using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "EnemyDataSO", menuName = "Create EnemyDataSO")] 
public class EnemyDataSO : ScriptableObject
{
    public List<EnemyData> enemyDatasList = new List<EnemyData>();

    /// <summary>
    /// 敵の詳細データ
    /// </summary>
    [Serializable]
    public class EnemyData
    {
        public int enemyNo;

        public int maxHp;
        public int attackPower;
        public float intervalAttackTime;
        public float moveSpeed;
        public AttackRangeType attackRangeType;

        public int addSpecialMovePoint;

        //追加するものがあれば追加する
    }
}
