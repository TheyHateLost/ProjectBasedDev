using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class BTUResultsUI : MonoBehaviour
{
    [SerializeField, Required] private BTUCalculator _calculator;
    [SerializeField, Required] private TMP_Text _contentText;

    private void Update()
    {
        _contentText.text = $"BTU: {_calculator.totalBuildingBTU}";
    }
}