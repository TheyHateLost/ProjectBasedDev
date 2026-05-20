using System;
using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RoomSwitcherUI : MonoBehaviour
{
    [SerializeField, Required] private BuildingGenerator _buildingGenerator;
    [SerializeField, Required] private TMP_Text _roomNameText;
    [SerializeField, Required] private Button _prevButton;
    [SerializeField, Required] private Button _nextButton;

    private void Awake()
    {
        MinigameManager.Instance.OnMinigameStarted.AddListener(OnMinigameStarted);
        MinigameManager.Instance.OnMinigameFinished.AddListener(OnMinigameFinished);
    }

    private void OnDestroy()
    {
        if (MinigameManager.Instance != null)
        {
            MinigameManager.Instance.OnMinigameStarted.RemoveListener(OnMinigameStarted);
            MinigameManager.Instance.OnMinigameFinished.RemoveListener(OnMinigameFinished);
        }
    }

    private void OnMinigameStarted()
    {
        EnableRoomSwitching(false);
    }

    private void OnMinigameFinished()
    {
        EnableRoomSwitching(true);
    }

    private void Start()
    {
        EnableRoomSwitching(true);
        
        _prevButton.onClick.AddListener(OnPreviousButtonPressed);
        _nextButton.onClick.AddListener(OnNextButtonPressed);
    }

    public void OnPreviousButtonPressed()
    {
        _buildingGenerator.ShowPreviousRoom();
    }

    public void OnNextButtonPressed()
    {
        _buildingGenerator.ShowNextRoom();
    }

    private void Update()
    {
        var room = _buildingGenerator.CurrentRooms[_buildingGenerator.CurrentRoomIndex];
        _roomNameText.text = CustomUtils.SplitPascalCase(room.Type.ToString());
    }

    private void EnableRoomSwitching(bool isEnabled)
    {
        if (_buildingGenerator.CurrentRooms.Count <= 1)
        {
            _prevButton.gameObject.SetActive(false);
            _nextButton.gameObject.SetActive(false);
            return;
        }
        
        _prevButton.gameObject.SetActive(isEnabled);
        _nextButton.gameObject.SetActive(isEnabled);
    }
}