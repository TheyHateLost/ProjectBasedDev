using System;
using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class ThermostatMinigameController : MonoBehaviour
{
    public enum State
    {
        PoweredOn,
        ScrewPuzzle,
        WireMatching,
        SystemTest,
        Animation
    }
    [field: SerializeField, ReadOnly] public State CurrentState { get; private set; }
    [SerializeField] private float _closeAfterFinishDuration = 1f;
    
    [Header("Powered On")]
    [SerializeField, Required] private TMP_Text _powerSwitchStatusText;
    
    [Header("Screw Puzzle")]
    [SerializeField, Required] private Image _screwImage;
    [SerializeField, Required] private Image _oldThermostatImage;
    [SerializeField] private int _maxScrewCount = 4;
    [SerializeField, ReadOnly] private int _currentScrewCount;
    [SerializeField] private float _screwFinishAnimDuration = 0.5f;
    
    [Header("Wire Matching")]
    [SerializeField, Required] private Image _newThermostatImage;
    [SerializeField, Required] private Transform _socketWireContainer;
    [SerializeField, Required] private Transform _newThermostatWireContainer;
    [SerializeField, Required] private Image _newThermostatScrew;
    [SerializeField, ReadOnly] private UILineRenderer _selectedLine;
    [SerializeField, ReadOnly] private int _totalWireCount;
    [SerializeField, ReadOnly] private List<UILineRenderer> _completedWires = new();
    [SerializeField] private float _wireFinishAnimDuration = 0.5f;
 
    private void Start()
    {
        CurrentState = State.PoweredOn;

        _powerSwitchStatusText.text = "Power: On";
        
        // move new thermostat out of screen
        _newThermostatImage.transform.localPosition = _newThermostatImage.transform.localPosition.WithY(-Screen.height);
        // scramble wire matching order
        _socketWireContainer.ShuffleChildren();
        _newThermostatWireContainer.ShuffleChildren();
        _totalWireCount = _newThermostatWireContainer.childCount;
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case State.PoweredOn:
                break;
            case State.ScrewPuzzle:
                break;
            case State.WireMatching:

                if (_selectedLine == null)
                    return;

                RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _selectedLine.rectTransform,
                    Input.mousePosition,
                    null,
                    out Vector2 localPoint
                );

                _selectedLine.points = new List<Vector2>()
                {
                    Vector2.zero,
                    localPoint
                };
                _selectedLine.SetVerticesDirty();
                
                break;
            case State.SystemTest:
                break;
        }
    }

    public void TurnOffPower()
    {
        if (CurrentState != State.PoweredOn)
            return;
        
        _powerSwitchStatusText.text = "Power: Off";
        
        CurrentState = State.ScrewPuzzle;
    }
    
    public void UnscrewScrew()
    {
        if (CurrentState != State.ScrewPuzzle)
            return;

        if (ToolManager.Instance.GetTool() != Tools.Screwdriver)
            return;
        
        _currentScrewCount++;
        _screwImage.transform.localRotation = Quaternion.Euler(0, 0, _currentScrewCount * 90f);
        if (_currentScrewCount >= 4)
        {
            CurrentState = State.Animation;
            
            _screwImage.color = Color.red;
            _newThermostatImage.gameObject.SetActive(true);
            _newThermostatImage.transform.DOLocalMoveY(0f, _screwFinishAnimDuration).SetEase(Ease.OutCubic);
            _oldThermostatImage.transform.DOLocalMoveY(-Screen.height, _screwFinishAnimDuration)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
            {
                CurrentState = State.WireMatching;
            });
        }
    }

    public void PressNewThermostatWireButton(Image wireImage)
    {
        if (_selectedLine != null)
            return;
        
        UILineRenderer line = wireImage.GetComponentInChildren<UILineRenderer>();
        if (_completedWires.Contains(line))
            return;
        
        _selectedLine = line;
    }

    public void PressSocketWireButton(Image wireImage)
    {
        if (_selectedLine == null)
            return;

        if (_selectedLine.transform.parent.gameObject.name != wireImage.gameObject.name)
            return;
        
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            _selectedLine.rectTransform,
            wireImage.transform.position,
            null,
            out Vector2 localPoint
        );
        _selectedLine.points = new List<Vector2>()
        {
            Vector2.zero,
            localPoint
        };
        _selectedLine.SetVerticesDirty();

        _completedWires.Add(_selectedLine);
        _selectedLine = null;

        if (_completedWires.Count >= _totalWireCount)
        {
            CurrentState = State.Animation;
            
            _newThermostatWireContainer.DOScaleX(0f, _wireFinishAnimDuration).SetEase(Ease.OutCubic);
            _newThermostatImage.transform.DOMoveX(_socketWireContainer.parent.position.x, _wireFinishAnimDuration)
                .SetEase(Ease.OutCubic)
                .OnComplete(() =>
                {
                    _newThermostatScrew.gameObject.SetActive(true);
                    CurrentState = State.SystemTest;
                });
        }
    }
    
    public void TurnOnPower()
    {
        if (CurrentState != State.SystemTest)
            return;
        
        _powerSwitchStatusText.text = "Power: On";
        
        CurrentState = State.Animation;
        StartCoroutine(OnFinishMinigame());
    }
    
    private IEnumerator OnFinishMinigame()
    {
        Cursor.lockState = CursorLockMode.Locked;

        yield return new WaitForSeconds(_closeAfterFinishDuration);
        
        Cursor.lockState = CursorLockMode.None;
        
        MinigameManager.Instance.FinishCurrentMinigame();
    }
}
