using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : SingletonMonoBehaviour<AudioManager>
{

    public static AudioManager instance = null;

    // ボリュームの保存とデフォルト値
    private const string BGM_VOL_KEY = "BGM_VOL_KEY";
    private const string SE_VOL_KEY = "SE_VOL_KEY";
    private const string VOICE_VOL_KEY = "VOICE_VOL_KEY";
    private const float BGM_VOL_DEF = 1.0f;
    private const float SE_VOL_DEF = 0.7f;
    private const float VOICE_VOL_DEF = 1.0f;

    // オーディオファイルのパス
    private const string BGM = "Audio/BGM";
    private const string SE = "Audio/SE";
    private const string VOICE = "Audio/MikuVoice";

    // BGMがフェードするのにかかる時間
    public const float BGM_FadeSpeedRateHigh = 0.9f;
    public const float BGM_FadeSpeedRateLow = 0.3f;
    private float _bgmFadeSpeedRate = BGM_FadeSpeedRateHigh;

    // 次流すBGM,SEの名前
    private string _nextBGM;
    private string _nextSE;
    private string _nextVOICE;

    // BGMがフェードアウト中かどうか
    private bool _isFade = false;

    // BGM,SE用に分けてオーディオソースを持つ
    private AudioSource _bgmSource;
    private List<AudioSource> _seSourceList;
    private List<AudioSource> _voiceSourceList;
    private const int seSourceNum = 15;

    // 全オーディオソースの保持
    private Dictionary<string, AudioClip> _bgmDic, _seDic, _voiceDic;



    // Use this for initialization

    private void Awake()
    {
        // インスタンスが存在するとき
        if (instance != null)
        {
            // これを破棄する
            Destroy(this.gameObject);
            return;
        }
        // インスタンスが存在しない時
        else if (instance == null)
        {
            // これをインスタンスとする
            instance = this;
        }
        // シーンをまたいでも消去されないようにする
        DontDestroyOnLoad(this.gameObject);

        // オーディオリスナー及びオーディオソースをSE+1(BGMの分)作成
        gameObject.AddComponent<AudioListener>();
        for (int i = 0; i < seSourceNum + 1; i++)
        {
            gameObject.AddComponent<AudioSource>();
        }
        // 作成したオーディオソースを取得して各変数に設定、ボリュームも設定
        AudioSource[] audioSourceArray = GetComponents<AudioSource>();
        _seSourceList = new List<AudioSource>();
        _voiceSourceList = new List<AudioSource>();

        for (int i = 0; i < audioSourceArray.Length; i++)
        {
            audioSourceArray[i].playOnAwake = false;

            if (i == 0)
            {
                audioSourceArray[i].loop = true;
                _bgmSource = audioSourceArray[i];
                _bgmSource.volume = PlayerPrefs.GetFloat(BGM_VOL_KEY, BGM_VOL_DEF);
            }
            else
            {
                _seSourceList.Add(audioSourceArray[i]);
                audioSourceArray[i].volume = PlayerPrefs.GetFloat(SE_VOL_KEY, SE_VOL_DEF);
                _voiceSourceList.Add(audioSourceArray[i]);
                audioSourceArray[i].volume = PlayerPrefs.GetFloat(VOICE_VOL_KEY, VOICE_VOL_DEF);
            }
        }
        // リソースフォルダから全SEとBGMのファイルを読み込み,セット
        _bgmDic = new Dictionary<string, AudioClip>();
        _seDic = new Dictionary<string, AudioClip>();
        _voiceDic = new Dictionary<string, AudioClip>();

        object[] bgmList = Resources.LoadAll(BGM);
        object[] seList = Resources.LoadAll(SE);
        object[] voiceList = Resources.LoadAll(VOICE);

        foreach (AudioClip bgm in bgmList)
        {
            _bgmDic[bgm.name] = bgm;
        }
        foreach (AudioClip se in seList)
        {
            _seDic[se.name] = se;
        }
        foreach (AudioClip voice in voiceList)
        {
            _voiceDic[voice.name] = voice;
        }
    }
    void Start()
    {

    }

    // 音量の変更
    // BGMとSEのボリュームを別々に変更、保存
    public void ChangeVol(float bgmVol, float seVol, float voiceVol)
    {
        _bgmSource.volume = bgmVol;
        foreach (AudioSource seSource in _seSourceList)
        {
            seSource.volume = seVol;
        }
        foreach (AudioSource voiceSource in _voiceSourceList)
        {
            voiceSource.volume = voiceVol;
        }
        PlayerPrefs.SetFloat(BGM_VOL_KEY, bgmVol);
        PlayerPrefs.SetFloat(SE_VOL_KEY, seVol);
        PlayerPrefs.SetFloat(VOICE_VOL_KEY, voiceVol);
    }

    // ここからSEの処理
    // 指定したファイル名のSEを流す。第二引数のdelayに指定した時間だけ、再生までの間隔をあける

    public void PlaySE(string seName, float delay = 0)
    {
        if (!_seDic.ContainsKey(seName))
        {
            Debug.Log(seName + "という名前のSEがありません");
            return;
        }
        _nextSE = seName;
        Invoke("DelayPlaySE", delay);
    }
    public void DelayPlaySE()
    {
        foreach (AudioSource seSource in _seSourceList)
        {
            if (!seSource.isPlaying)
            {
                seSource.PlayOneShot(_seDic[_nextSE] as AudioClip);
                return;
            }
        }
    }

    // ここからボイスの処理
    public void PlayVoice(string voiceName, float delay = 0)
    {
        if (!_voiceDic.ContainsKey(voiceName))
        {
            Debug.Log(voiceName + "という名前のボイスがありません");
        }
        _nextVOICE = voiceName;
        Invoke("DelayPlayVoice", delay);
    }
    public void DelayPlayVoice()
    {
        foreach (AudioSource voiceSource in _voiceSourceList)
        {
            if (!voiceSource.isPlaying)
            {
                voiceSource.PlayOneShot(_voiceDic[_nextVOICE] as AudioClip);
                return;
            }
        }
    }

    // ここからBGMの処理
    // 指定したファイル名のBGMを流す。なお、すでに流れている場合は前の曲をﾌｪｰﾄﾞｱｳﾄさせてから流す
    // 第二引数のfadeSpeedRateに指定した割合でﾌｪｰﾄﾞｱｳﾄするスピードが変わる

    public void PlayBGM(string bgmName, float fadeSpeedRate = BGM_FadeSpeedRateHigh)
    {
        if (!_bgmDic.ContainsKey(bgmName))
        {
            Debug.Log(bgmName + "という名前のBGMがありません");
            return;
        }

        if (!_bgmSource.isPlaying)
        {
            _nextBGM = "";
            _bgmSource.clip = _bgmDic[bgmName] as AudioClip;
            _bgmSource.Play();
        }
        else if (_bgmSource.clip.name != bgmName)
        {
            _nextBGM = bgmName;
            FadeOutBGM(fadeSpeedRate);
        }
    }

    public void StopBGM()
    {
        _bgmSource.Stop();
    }

    // 現在流れている曲をﾌｪｰﾄﾞｱｳﾄさせる
    // fadeSpeedRateに指定した割合でﾌｪｰﾄﾞｱｳﾄするスピードがかわる
    public void FadeOutBGM(float fadeSpeedRate = BGM_FadeSpeedRateLow)
    {
        _bgmFadeSpeedRate = fadeSpeedRate;
        _isFade = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (!_isFade)
        {
            return;
        }
        // 徐々にボリュームを下げていき、ボリュームが0になったらボリュームを元に戻して次の曲に進む
        _bgmSource.volume -= Time.deltaTime * _bgmFadeSpeedRate;
        if (_bgmSource.volume <= 0)
        {
            _bgmSource.Stop();
            _bgmSource.volume = PlayerPrefs.GetFloat(BGM_VOL_KEY, BGM_VOL_DEF);
            _isFade = false;

            if (!string.IsNullOrEmpty(_nextBGM))
            {
                PlayBGM(_nextBGM);
            }
        }
    }

}
