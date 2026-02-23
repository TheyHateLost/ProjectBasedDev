using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using UnityEngine;

[CreateAssetMenu(fileName = "AudioBank", menuName = "CharonsCorner/AudioBank")]
public class AudioBankSO : ScriptableObject
{
    [SerializeField, SerializedDictionary("Audio ID", "Audio Clip")]
    private SerializedDictionary<ObjectKey, AudioClip> _bank;

    public Dictionary<ObjectKey, AudioClip> Bank => _bank;
}