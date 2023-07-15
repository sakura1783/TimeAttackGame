using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class EnemyGenerator : MonoBehaviour
{
    [SerializeField] private EnemyController enemyPrefab;
    //public EnemyController EnemyPrefab => enemyPrefab;

    [SerializeField] private Vector2[] generatePos;

    [SerializeField] private int intervalGenerateTime;

    [SerializeField] private GameManager gameManager;

    //private int generateEnemyCount = 0;

    public List<EnemyController> enemiesList = new List<EnemyController>();

    //[SerializeField] private CharaController charaController;

    //private EnemyController generatedEnemy;
    //public EnemyController GeneratedEnemy => generatedEnemy;

    //public int maxGenerateEnemyCount;  //生成する敵の数

    //[SerializeField] private int maxGenerateCountEnemy0;
    //[SerializeField] private int maxGenerateCountEnemy1;  //敵0と1を生成する最大数

    //[SerializeField] private int generateCountEnemy0;
    //[SerializeField] private int generateCountEnemy1;  //敵0と1を生成した数

    //private int generateEnemyNo;


    //void Start()
    //{
    //    StartCoroutine(GenerateEnemy());
    //}

    //private IEnumerator GenerateEnemy()
    //{
    //    while (maxGenerateEnemyCount > generateEnemyCount)
    //    {
    //        //配列の中からエネミーを生成する位置をランダムに一つ選ぶ
    //        var generateTran = Random.Range(0, generatePos.Length);

    //        yield return new WaitForSeconds(intervalGenerateTime);

    //        generatedEnemy = Instantiate(enemyPrefab, generatePos[generateTran], Quaternion.identity);

    //        //今後のためにGameManagerにEnemyController型の情報を渡す
    //        gameManager.EnemyController = generatedEnemy;

    //        //TODO ObjAttackRangeゲームオブジェクト生成

    //        generateEnemyCount++;

    //        enemiesList.Add(generatedEnemy);

    //        //敵の種類をランダムに決定
    //        //int enemyNo = Random.Range(0, DataBaseManager.instance.enemyDataSO.enemyDatasList.Count);
    //        int randomNo = Random.Range(0, 100);
    //        Debug.Log("ランダムな値：" + randomNo);

    //        //もしEnemy0が生成できなくなったら、強制的にEnemy1を生成する
    //        if (generateCountEnemy0 >= maxGenerateCountEnemy0)
    //        {
    //            randomNo = Random.Range(60, 100);

    //            Debug.Log("Enemy0が生成できないため、強制的にEnemy1を生成します");
    //        }
    //        //もしEnemy1が生成できなくなったら、強制的にEnemy0を生成する
    //        if (generateCountEnemy1 >= maxGenerateCountEnemy1)
    //        {
    //            randomNo = Random.Range(0, 60);

    //            Debug.Log("Enemy1が生成できないため、強制的にEnemy0を生成します");
    //        }

    //        if (randomNo <= 59)
    //        {
    //            if (generateCountEnemy0 < maxGenerateCountEnemy0)
    //            {
    //                generateEnemyNo = 0;

    //                generateCountEnemy0++;

    //                Debug.Log("生成する敵は" + generateEnemyNo + "です");
    //            }
    //        }
    //        else
    //        {
    //            if (generateCountEnemy1 < maxGenerateCountEnemy1)
    //            {
    //                generateEnemyNo = 1;

    //                generateCountEnemy1++;

    //                Debug.Log("生成する敵は" + generateEnemyNo + "です");
    //            }
    //        }

    //        //enemyPrefab.SetUpEnemyController(charaController);  // <= これは間違い。
    //        generatedEnemy.SetUpEnemyController(gameManager, DataBaseManager.instance.enemyDataSO.enemyDatasList[generateEnemyNo]);  //Enemyのプレハブではなく、生成したEnemyの情報に命令を出す。
    //    }
    //}


    [System.Serializable]
    public class GenerateData
    {
        public int enemyNo;
        public int maxGenerateCount;
        public int generateRate;  //重みづけされた敵の生成値
    }

    [SerializeField] private List<GenerateData> generateDataList = new();


    [System.Serializable]
    public class GenerateEnemy
    {
        public int enemyNo;  //Key
        public int generateCount;  //Value
    }

    private Dictionary<int, int> generateCountByEnemyType = new();

    //生成する全敵の合計数を算出したプロパティ
    public int MaxGenerateEnemyCount => generateDataList.Sum(generateData => generateData.maxGenerateCount);

    private int totalGenerateRate = 0;

    /// <summary>
    /// Startメソッドの代わり
    /// </summary>
    public void SetUpEnemyGenerator()
    {
        //各生成数の初期化
        InitializeGenerateCounts();

        //敵の生成確率の合計値を算出
        CalculateTotalGenerateRate();  //Calculate = 計算する

        //生成時間の計測処理を開始
        StartCoroutine(GenerateTimer());
    }

    /// <summary>
    /// 各生成数の初期化
    /// </summary>
    private void InitializeGenerateCounts()
    {
        //これは使わない(enemyNoがiの値になることが前提となっているため。もし2や3などのenemyNoを持つ敵だけを生成したい場合、思い通りの挙動にならない。)
        //for (int i = 0; i < generateDataList.Count; i++)
        //{
        //    generateCountByEnemyType[i] = 0;
        //}

        //Linqで書いた場合
        //generateCountByEnemyType = Enumerable.Range(0, generateDataList.Count).ToDictionary(i => i, _ => 0);  //i => iでiの値をそのままKeyにする。_ => 0で、全てのValueの値を0にする

        //データをクリア
        generateCountByEnemyType.Clear();

        //enemyDataListを走査(スキャン)しながら各要素(GenerateData)のenemyNoをキー、Valueを0として初期設定　上の処理とは違い、enemyNoは連番でなくてもいい(EnemyDataが増えたら、その番号を自由に設定できる)
        foreach (var generateData in generateDataList)
        {
            generateCountByEnemyType[generateData.enemyNo] = 0;
        }

        //Linqで書いた場合
        //generateCountByEnemyType = generateDataList.ToDictionary(generateData => generateData.enemyNo, _ => 0);

        Debug.Log("生成数初期化");
    }

    /// <summary>
    /// 最大生成数を超えていない敵の生成確率の合計値を算出
    /// </summary>
    private void CalculateTotalGenerateRate()
    {
        //foreachを使う場合、変数を初期化してから使う(前の値がクリアされないため)
        totalGenerateRate = 0;

        //foreachループを使って、generateDataListの各要素(GenerateData)にアクセス
        foreach (var generateData in generateDataList)
        {
            //generateCountByEnemyTypeディクショナリから対応する生成数を取得
            //TryGetValueは第一引数には取得したいKey、第二引数には取得できた場合にValueの値を格納するための変数をoutキーワードを使用して指定
            if (generateCountByEnemyType.TryGetValue(generateData.enemyNo, out int generateCount) && generateCount < generateData.maxGenerateCount)
            {
                //最大生成数に達していないなら、generateRateをtotalGenerateRateに加算
                totalGenerateRate += generateData.generateRate;
            }
        }

        //Linqで書いた場合(totalGenerateRateに直接代入して前の値がクリアされるので、事前に0にする必要なし)
        //indexでKeyを指定し、Valueである現在の生成数と、その敵の最大生成数を比較し、まだ最大生成数に達していない全GenerateDataを抽出
        //totalGenerateRate += generateDataList
        //    .Where((generateData, index) => generateCountByEnemyType[index] < generateData.maxGenerateCount)  //条件に合致したGenerateDataだけ見つける
        //    .Sum(generateData => generateData.generateRate);  //抽出されたGenerateDataにあるgenerateRateを合計する

        //文字列補間式を使った記述方法
        Debug.Log($"生成合計値：{totalGenerateRate}");
    }

    /// <summary>
    /// 生成時間の計測
    /// </summary>
    /// <returns></returns>
    private IEnumerator GenerateTimer()
    {
        //現在までに生成した数のカウント用
        int generatedCount = 0;

        //生成可能な数と現在の生成数とを比較し、生成可能な場合のみ処理する
        while (MaxGenerateEnemyCount > generatedCount)
        {
            yield return new WaitForSeconds(intervalGenerateTime);

            SpawnEnemy();

            //Dictionary内の全てのValueを合計して、現在の生成総数を取得
            generatedCount = generateCountByEnemyType.Values.Sum();

            Debug.Log($"現在の生成総数：{generatedCount}");
        }

        Debug.Log("全てのエネミーを生成しました");
    }

    /// <summary>
    /// 敵の生成
    /// </summary>
    private void SpawnEnemy()
    {
        ////ここから////

        //配列の中から敵を生成する位置をランダムに選ぶ
        var generateTran = Random.Range(0, generatePos.Length);

        //敵の生成をしてリストに追加
        EnemyController generatedEnemy = Instantiate(enemyPrefab, generatePos[generateTran], Quaternion.identity);
        enemiesList.Add(generatedEnemy);

        ////ここまで変更なし////

        //生成可能な敵の生成率の再計算
        CalculateTotalGenerateRate();

        //ランダムな敵の種類を決める
        int randomNo = Random.Range(0, totalGenerateRate);
        Debug.Log($"ランダムな値：{randomNo}");

        //取得したランダムな値をGetRandomEnemyTypeの引数に渡して、どの敵を生成するか、敵の番号を取得
        int generateEnemyNo = GetRandomEnemyType(randomNo);

        if (generateEnemyNo == -1)
        {
            Debug.Log("生成する敵がいません");
        }

        //生成した種類の敵だけカウントアップ
        generateCountByEnemyType[generateEnemyNo]++;
        Debug.Log($"生成した敵の種類は{generateEnemyNo}番です");

        generatedEnemy.SetUpEnemyController(gameManager, DataBaseManager.instance.enemyDataSO.enemyDatasList[generateEnemyNo]);
    }

    /// <summary>
    /// ランダムな敵の確定
    /// </summary>
    /// <param name="randomNo"></param>
    /// <returns></returns>
    private int GetRandomEnemyType(int randomNo)
    {
        int cumulativeChance = 0;  //重み付け用の値

        foreach (var generateData in generateDataList)
        {
            //generateDataのenemyNoとgenerateCountByEnemyTypeディクショナリのキーを対応させることで、正しい敵の生成数を取得し、最大数と比較
            if (generateCountByEnemyType[generateData.enemyNo] < generateData.maxGenerateCount)
            {
                //生成数を超えていない場合に限り、重み付け用の値を加算
                cumulativeChance += generateData.generateRate;

                //ランダムな値が重み付けの値を超えていない場合
                if (randomNo < cumulativeChance)
                {
                    //敵の種類確定
                    return generateData.enemyNo;
                }
            }
        }

        return -1;  //生成不可

        //Linqの場合
        //int cumulativeChance = 0;

        //return generateDataList
        //    .Where(generateData => generateCountByEnemyType[generateData.enemyNo] < generateData.maxGenerateCount)  //最大数を超えていないGenerateDataのみを抽出
        //    .FirstOrDefault(generateData =>  //最初に該当したGenerateDataを取得。見つからない時にはnullを取得
        //    {
        //        cumulativeChance += generateData.generateRate;
        //        return randomNo < cumulativeChance;
        //    })
        //    ?.enemyNo ?? -1;  //FirstOrDefaultの結果がnullの場合には、?.演算子により、null安全参照が行われ、nullに対してenemyNoを呼び出すことを回避
        //                      //かつ、?? -1によりFirstOrDefaultの結果がnullの場合には、??演算子によってデフォルト値として-1を返す
    }
}
