using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[CreateAssetMenu(fileName = "AttackRangeSizeSO", menuName = "Create AttackRangeSizeSO")]
public class AttackRangeSizeSO : ScriptableObject
{
    public List<AttackRangeSize> attackRangeSizeList = new List<AttackRangeSize>();

    [Serializable]
    public class AttackRangeSize
    {
        public AttackRangeType attackRangeType;
        public float radius;
    }
}
