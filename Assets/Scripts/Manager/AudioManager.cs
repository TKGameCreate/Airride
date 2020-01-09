using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    //インスタンスを取得
    public static AudioManager Instance { get; private set; }

    #region SerializeField
    [SerializeField] private AudioSource bgmSource = null;
    [SerializeField] private AudioSource seSource = null;
    [SerializeField] private List<AudioClip> bgmList = new List<AudioClip>();
    [SerializeField] private List<AudioClip> systemSEList = new List<AudioClip>();
    #endregion

    #region Awake
    private void Awake()
    {
        #region シングルトン処理
        if (Instance == null)
        {
            Instance = this;
            PlaySceneBGM();
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Instance.PlaySceneBGM();
            Destroy(gameObject);
        }
        #endregion
    }
    #endregion

    #region method
    private void PlaySceneBGM()
    {
        int sceneNo = AirrideSceneManager.Instance.SceneIndex();
        bgmSource.clip = bgmList[sceneNo];
        bgmSource.Play();
    }

    public void PlayBGM(AudioClip bgm)
    {
        bgmSource.clip = bgm;
        bgmSource.Play();
    }

    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PauseBGM()
    {
        bgmSource.Pause();
    }

    public void UnPauseBGM()
    {
        bgmSource.UnPause();
    }

    public void PlaySE(AudioClip se)
    {
        seSource.PlayOneShot(se);
    }
    #endregion
}