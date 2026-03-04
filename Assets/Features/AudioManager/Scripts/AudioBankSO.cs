using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioBank", menuName = "AudioBank")]
public class AudioBankSO : ScriptableObject
{
    [SerializeField, SerializedDictionary("Audio ID", "Audio Clip")]
    private SerializedDictionary<ObjectKey, AudioClip> _bank;

    public Dictionary<ObjectKey, AudioClip> Bank => _bank;
}