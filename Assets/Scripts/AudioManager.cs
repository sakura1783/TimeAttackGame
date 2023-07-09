using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

//public enum AudioType
//{
//    Home,
//    Battle,
//    GameOver,
//    GameClear,
//}

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;

    [SerializeField] private AudioSource[] audioSources;

    private AudioSource playingAudio;

    //private int playCount;


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
    /// BGM再生　BGM再生時はこのメソッドを呼び出す
    /// </summary>
    public void PreparePlayBGM(int index)
    {
        StartCoroutine(PlayBGM(index));
    }

    /// <summary>
    /// BGM再生
    /// </summary>
    /// <param name="index"></param>
    /// <returns></returns>
    private IEnumerator PlayBGM(int index)
    {
        //再生前に別の曲が流れている場合
        //if (index != 0)
        //{
        //    //徐々にボリュームを下げる
        //    audioSources[index - 1].DOFade(0, 0.75f);
        //}
        //if (index == 3)
        //{
        //    audioSources[index - 2].DOFade(0, 0.75f);
        //}

        //別の曲が流れている場合、徐々にボリュームを下げる
        foreach (AudioSource audioSource in audioSources)
        {
            if (audioSource.isPlaying)
            {
                playingAudio = audioSource;

                playingAudio.DOFade(0, 0.75f);
            }
        }

        //前の曲のボリュームが下がるのを待つ
        yield return new WaitForSeconds(0.45f);

        //新しい指定された曲を再生
        audioSources[index].Play();

        audioSources[index].DOFade(1, 0.75f);

        ////前に流れていたBGMを停止
        //if (index != 0)
        //{
        //    //前に流れていた曲のボリュームが0になったら
        //    yield return new WaitUntil(() => audioSources[index - 1].volume == 0);

        //    //再生を停止
        //    audioSources[index - 1].Stop();
        //}

        if (playingAudio != null)
        {
            //前に流れていた曲のボリュームが0になったらそのBGMの再生を停止
            yield return new WaitUntil(() => playingAudio.volume == 0);
            playingAudio.Stop();
        }
    }
}
