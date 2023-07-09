using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SceneType
{
    Home,
    Battle,
    GameOver,
    GameClear,
}

public class SceneStateManager : MonoBehaviour
{
    public static SceneStateManager instance;

    [SerializeField] private Fade fade;  //FadeCanvasゲームオブジェクト

    [SerializeField] private float fadeDuration = 1.0f;  //フェードの時間


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
    /// 引数で指定したシーンへのシーン遷移の準備
    /// </summary>
    /// <param name="sceneType"></param>
    public void PrepareLoadNextScene(SceneType sceneType)
    {
        //FadeCanvasの情報があるかないかを判断して、トランジションの機能を使うか、使わないかを切り替える
        if (!fade)
        {
            //ない場合、今までと同じようにすぐにシーン遷移
            StartCoroutine(LoadNextScene(sceneType));
        }
        else
        {
            //ある場合、fadeDuration分の時間をかけてフェードインを行ってから、シーン遷移
            fade.FadeIn(fadeDuration, () => { StartCoroutine(LoadNextScene(sceneType)); });
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="sceneType"></param>
    /// <returns></returns>
    private IEnumerator LoadNextScene(SceneType sceneType)
    {
        SceneManager.LoadScene(sceneType.ToString());

        //フェードインしている場合は
        if (fade)
        {
            //読み込んだ新しいシーンの情報を取得
            Scene scene = SceneManager.GetSceneByName(sceneType.ToString());

            //シーンの読み込み終了を待つ
            yield return new WaitUntil(() => scene.isLoaded);

            //シーンの読み込みが終了してからフェードアウトして、場面転換を完了する
            fade.FadeOut(fadeDuration);
        }
    }
}
