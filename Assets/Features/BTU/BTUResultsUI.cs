using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class BTUResultsUI : MonoBehaviour
{
    [SerializeField, Required] private BTUCalculator _calculator;

    [Header("User Input")]
    [SerializeField, Required] private GameObject _userInputPanel;
    [SerializeField, Required] private TMP_InputField _userInputField;
    [SerializeField, Required] private TMP_Text _errorText;
    [SerializeField, Required] private TMP_Text _lengthText;
    [SerializeField, Required] private TMP_Text _widthText;
    [SerializeField, Required] private TMP_Text _heightText;
    [SerializeField, Required] private TMP_Text _glazeText;
    
    [Header("Results")]
    [SerializeField, Required] private GameObject _resultsPanel;
    [SerializeField, Required] private TMP_Text _minigameTimeText;
    [SerializeField, Required] private TMP_Text _percentErrorText;
    [SerializeField, Required] private TMP_Text _userBtuText;
    [SerializeField, Required] private TMP_Text _correctBtuText;

    private float _userInputBTU;
    
    public void Reveal()
    {
        // show user input
        _userInputPanel.SetActive(true);
        _errorText.text = "";
        _resultsPanel.SetActive(false);

        _lengthText.text = $"Length: {_calculator.roomLength}";
        _widthText.text = $"Width: {_calculator.roomWidth}";
        _heightText.text = $"Height: {_calculator.roomHeight}";
        _glazeText.text = $"Glaze: {_calculator.roomGlaze}";
        
        // reveal panel
        gameObject.SetActive(true);
    }

    public void TrySubmitUserBTU()
    {
        string trimmedInputText = _userInputField.text.Trim();
        bool isNumeric = float.TryParse(trimmedInputText, out float userBTU);
        if (!isNumeric)
        {
            _errorText.text = "Please enter a valid number!";
            return;
        }
        
        // store user input
        _userInputBTU = userBTU;
        
        // show results for the room
        _userInputPanel.gameObject.SetActive(false);
        _resultsPanel.SetActive(true);

        _userBtuText.text = $"Your BTU: {_userInputBTU:F2}";
        _correctBtuText.text = $"Correct BTU: {_calculator.firstRoomBTU:F2}";
        float percentError = ((_userInputBTU - _calculator.firstRoomBTU) / _calculator.firstRoomBTU) * 100f;
        _percentErrorText.text = $"Percent Error: {percentError:+0.00;-0.00}%";
        _minigameTimeText.text = $"Minigame Time: {CustomUtils.FormatTimeMMSS(123)}";
    }
}