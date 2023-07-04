using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor.Animations;

public enum CharaType
{
    Yellow,
    Black,
    Pink,
}

[CreateAssetMenu(fileName = "CharaDataSO", menuName = "Create CharaDataSO")]
public class CharaDataSO : ScriptableObject
{
    public List<CharaData> charaDatasList = new List<CharaData>();

    /// <summary>
    /// キャラの詳細データ
    /// </summary>
    [System.Serializable]
    public class CharaData
    {
        public string charaName;
        public int charaNo;
        public CharaType charaType;

        public int attackPower;
        public AttackRangeType attackRangeType;
        public float intervalAttackTime;

        //必殺技
        public string specialMoveName;
        public int maxSpecialMoveCount;
        public int intervalKillCountSpecialMove;  //必殺技発動に必要な敵Kill数
        public int durationSpecialMove;  //継続時間

        [Multiline] public string discription;

        public AnimatorController charaAnim;
    }
}
