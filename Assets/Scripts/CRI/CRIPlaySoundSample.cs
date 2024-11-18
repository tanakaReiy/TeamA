using Alchemy.Inspector;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static CriWare.CriProfiler;

public class CRIPlaySoundSample : MonoBehaviour
{
    /// <summary>
    /// CueSheet�̐ݒ�
    /// <br>���ꂼ��ɑΉ��������̂�AtomBrowser����T���ĕ�����Ŏw�肷�邱��</br>
    /// </summary>
    [LabelText("BGM��CueSheet")]
    [SerializeField] string _bgmCueSheet;
    [LabelText("SE��CueSheet")]
    [SerializeField] string _seCueSheet;
    [LabelText("Voice��CueSheet")]
    [SerializeField] string _voiceCueSheet;

    [Title("3D�T�E���h�̈ʒu�Q�Ɨp�I�u�W�F�N�g")]
    [SerializeField] Transform _3dSoundPlayPosition;

    private void Start()
    {
        if(_3dSoundPlayPosition == null)
        {
            _3dSoundPlayPosition = this.transform;
        }
    }

    [Title("�ʏ������p")]
    [Button]
    private void OnInitialize()
    {
        CRIAudioManager.Initialize();
    }

    [Title("BGM�̋@�\�������֐��Q")]
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


    [Title("SE�̋@�\�������֐��Q")]
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


    [Title("Voice�̋@�\�������֐��Q")]
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


    [Title("�x���ŗ�����@�\�������֐��Q")]
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
