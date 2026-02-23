using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : Singleton<AudioManager>
{
    [SerializeField] private AudioSource _musicSource;

    [field: Header("Mixers")]
    [field: SerializeField] public AudioMixer MasterMixer { get; private set; }
    [field: SerializeField] public AudioMixerGroup SfxMixer { get; private set; }
    [field: SerializeField] public AudioMixerGroup VoiceMixer { get; private set; }
    [field: SerializeField] public AudioMixerGroup MusicMixer { get; private set; }
    [SerializeField] private DefaultMixerTarget _defaultMixer = DefaultMixerTarget.None;

    public static readonly string MasterVolumeParam = "MasterVolume";
    public static readonly string SfxVolumeParam = "SFXVolume";
    public static readonly string VoiceVolumeParam = "VoiceVolume";
    public static readonly string MusicVolumeParam = "MusicVolume";

    [Space]
    [SerializeField, SerializedDictionary("Audio ID", "Audio Clip")]
    private AudioBankSO _soundBank;
    [SerializeField, SerializedDictionary("Audio ID", "Audio Clip")]
    private AudioBankSO _musicBank;

    private readonly Dictionary<ObjectKey, int> _lastPlayedFrame = new();

    public enum MixerTarget
    {
        None,
        Default,
        SFX,
        Voice
    }

    public enum DefaultMixerTarget
    {
        None = MixerTarget.None,
        SFX = MixerTarget.SFX,
        Voice = MixerTarget.Voice
    }
    
    public void Play(ObjectKey clipKey, MixerTarget mixerTarget, Vector3? position = null, float pitch = 1f, bool persistAcrossScenes = false)
    {
        // Prevent same sound from playing twice in the same frame
        int frame = Time.frameCount;
        if (_lastPlayedFrame.TryGetValue(clipKey, out int lastFrame) && lastFrame == frame)
            return;
        _lastPlayedFrame[clipKey] = frame;
        
        if (_soundBank.Bank.TryGetValue(clipKey, out AudioClip audioClip))
        {
            GameObject clipObject = new GameObject(clipKey.name, typeof(AudioDestroyer));
            if(persistAcrossScenes)
                DontDestroyOnLoad(clipObject);
            AudioSource source = clipObject.AddComponent<AudioSource>();
            if (position.HasValue)
            {
                clipObject.transform.position = position.Value;
                source.spatialBlend = 1;
                source.rolloffMode = AudioRolloffMode.Linear;
                source.maxDistance = 20f;
                source.dopplerLevel = 0f;
            }
            source.clip = audioClip;
            source.pitch = pitch;
            source.outputAudioMixerGroup = GetMixerGroup(mixerTarget);
            source.Play();
        }
        else
        {
            Debug.LogWarning($"AudioClip '{clipKey.name}' not found in sound bank");
        }
    }
    
    public void Play(ObjectKey clipKey, Vector3? position = null, float pitch = 1.0f)
    {
        Play(clipKey, MixerTarget.Default, position, pitch);
    }

    public void PlayAndFollow(ObjectKey clipKey, Transform target, MixerTarget mixerTarget)
    {
        if (_soundBank.Bank.TryGetValue(clipKey, out AudioClip audioClip))
        {
            GameObject clipObject = new GameObject(clipKey.name, typeof(AudioDestroyer));
            AudioSource source = clipObject.AddComponent<AudioSource>();
            FollowTarget followTarget = clipObject.AddComponent<FollowTarget>();
            source.spatialBlend = 1;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.maxDistance = 50f;
            source.dopplerLevel = 0f;
            source.clip = audioClip;
            source.outputAudioMixerGroup = GetMixerGroup(mixerTarget);
            followTarget.Init(target, FollowTarget.UpdateMode.Late);
            source.Play();
        }
        else
        {
            Debug.LogWarning($"AudioClip '{clipKey.name}' not found in sound bank");
        }
    }

    public void PlayMusic(ObjectKey musicKey)
    {
        if (musicKey == null)
            return;

        if (_musicBank.Bank.TryGetValue(musicKey, out AudioClip audioClip))
        {
            _musicSource.clip = audioClip;
            _musicSource.Play();
        }
        else
        {
            Debug.LogWarning($"AudioClip '{musicKey.name}' not present in music bank");
        }
    }

    public void PauseMusic() => _musicSource.Pause();

    public void UnpauseMusic() => _musicSource.UnPause();
    
    public void StopMusic()
    {
        _musicSource.Stop();
        _musicSource.clip = null;
    }
    
    private AudioMixerGroup GetMixerGroup(MixerTarget target)
    {
        if (target == MixerTarget.None) return null;
        if (target == MixerTarget.Default) return GetMixerGroup((MixerTarget)_defaultMixer);
        if (target == MixerTarget.SFX) return SfxMixer;
        if (target == MixerTarget.Voice) return VoiceMixer;
        throw new System.Exception("Invalid MixerTarget");
    }
    
    public static float ConvertFloatToDecibels(float value)
    {
        if (value == 0) return -80;
        return Mathf.Log10(value) * 20;
    }

    public static float ConvertDecibelsToFloat(float db)
    {
        if (db == -80) return 0;
        return Mathf.Pow(10, db / 20);
    }

    public float GetFloatNormalized(string param)
    {
        if (MasterMixer.GetFloat(param, out float v)) return ConvertDecibelsToFloat(v);
        return -1;
    }

    public static void SetMixerVolume(string mixerParamName, float volumeTarget)
    {
        Instance.MasterMixer.SetFloat(mixerParamName, ConvertFloatToDecibels(volumeTarget));
    }
}