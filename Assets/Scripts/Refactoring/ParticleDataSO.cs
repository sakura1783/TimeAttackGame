using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ParticleDataSO", menuName = "Create ParticleDataSO")]
public class ParticleDataSO : ScriptableObject
{
    public List<ParticleData> particleDataList = new();

    /// <summary>
    /// パーティクル用のデータ
    /// </summary>
    [System.Serializable]
    public class ParticleData
    {
        public ParticleName particleName;
        public ParticleSystem particlePrefab;
    }
}
