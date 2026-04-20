using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class MinigameTimeText : MonoBehaviour
{
    [SerializeField, Required] private TMP_Text _text;

    private void Update()
    {
        if (MinigameManager.Instance == null)
            return;
        
        _text.text = $"Time Spent: {CustomUtils.FormatTimeMMSS(MinigameManager.Instance.TotalMinigameTimer)}";
    }
}
