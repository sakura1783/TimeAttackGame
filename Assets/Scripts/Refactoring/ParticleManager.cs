using System.Collections.Generic;
using UnityEngine;

//パーティクルのスクリプタブル・オブジェクトを作る。それをシングルトンとして扱うことで、パーティクルを各子クラスに紐付けしなくても、変更や修正がしやすくなる

/// <summary>
/// パーティクルの種類
/// </summary>
public enum ParticleName
{
    Rovescio,
    Mezzanotte,
    Magia,
}

/// <summary>
/// パーティクル用のデータ
/// </summary>
[System.Serializable]
public class ParticleData
{
    public ParticleName particleName;
    public ParticleSystem particlePrefab;
}

/// <summary>
/// ParticleDataをまとめた、スクリプタブル・オブジェクト
/// </summary>
[CreateAssetMenu(fileName = "ParticleDataSO", menuName = "Create ParticleDataSO")]
public class ParticleDataSO : ScriptableObject
{
    public List<ParticleData> particleDataList = new();
}

/// <summary>
/// ParticleDataSOを管理するマネージャー
/// </summary>
public class ParticleManager : MonoBehaviour
{
    public static ParticleManager instance;

    [SerializeField] private ParticleDataSO particleDataSO;


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
    /// 利用したいパーティクルを探してもどす
    /// </summary>
    /// <param name="searchParticleName"></param>
    /// <returns></returns>
    public ParticleSystem GetParticleFromName(ParticleName searchParticleName)
    {
        return particleDataSO.particleDataList.Find(particleData => particleData.particleName == searchParticleName).particlePrefab;
    }
}
