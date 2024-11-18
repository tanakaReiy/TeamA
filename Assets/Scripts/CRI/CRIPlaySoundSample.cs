using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CriWare.CriProfiler;

public class CRIPlaySoundSample : MonoBehaviour
{
    /// <summary>
    /// CueSheetの設定
    /// <br>それぞれに対応したものをAtomBrowserから探して文字列で指定すること</br>
    /// </summary>
    [LabelText("BGMのCueSheet")]
    [SerializeField] string _bgmCueSheet;
    [LabelText("SEのCueSheet")]
    [SerializeField] string _seCueSheet;
    [LabelText("VoiceのCueSheet")]
    [SerializeField] string _voiceCueSheet;

    [Title("3Dサウンドの位置参照用オブジェクト")]
    [SerializeField] Transform _3dSoundPlayPosition;

    private void Start()
    {
        if(_3dSoundPlayPosition == null)
        {
            _3dSoundPlayPosition = this.transform;
        }
    }

    [Title("個別初期化用")]
    [Button]
    private void OnInitialize()
    {
        CRIAudioManager.Initialize();
    }

    [Title("BGMの機能を試す関数群")]
    [Button]
    private void OnPlayBGM(string CueName)
    {
        CRIAudioManager.BGM.Play(_bgmCueSheet, CueName);
    }
    [Button]
    private void OnChangeBGMVolume(float volume)
    {
        CRIAudioManager.BGM.SetVolume(volume);
    }


    [Title("SEの機能を試す関数群")]
    [Button]
    private void OnChangeSEVolume(float volume)
    {
        CRIAudioManager.BGM.SetVolume(volume);
    }
    [Button]
    private void OnPlaySE3D(string CueName)
    {
        CRIAudioManager.SE.Play3D(_3dSoundPlayPosition.position, _seCueSheet, CueName);
    }


    [Title("Voiceの機能を試す関数群")]
    [Button]
    private void OnPlayVoice(string CueName)
    {
        CRIAudioManager.VOICE.Play(_voiceCueSheet, CueName);
    }
    [Button]
    private void OnChangeVoiceVolume(float volume)
    {
        CRIAudioManager.BGM.SetVolume(volume);
    }


    [Title("遅延で流せる機能を試す関数群")]
    [Button]
    private void PlaySoundsWaitable(List<(SoundType soundType, string cueName, float waitTime)> soundData)
    {
        foreach(var sound in soundData)
        {
            try
            {
                switch(sound.soundType)
                {
                    case (SoundType.BGM):
                        CRIAudioManager.BGM.Play(_bgmCueSheet, sound.cueName, sound.waitTime);
                        break;
                    case (SoundType.SE):
                        CRIAudioManager.SE.Play(_seCueSheet, sound.cueName, sound.waitTime);
                        break;
                    case (SoundType.VOICE):
                        CRIAudioManager.VOICE.Play(_voiceCueSheet, sound.cueName, sound.waitTime);
                        break;
                }
            }
            catch
            {
                Debug.Log($"Exception  occurred.  CueName [{sound.cueName}]");
            }
        }
    }
}
