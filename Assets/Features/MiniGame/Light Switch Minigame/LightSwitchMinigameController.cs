using System.Collections;
using AYellowpaper.SerializedCollections;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class LightSwitchMinigameController : MonoBehaviour
{
    [Header("UI Section")]
    #region GameObjects
    [SerializeField] GameObject _startBtn;
    [SerializeField] GameObject _actualGame;
    [SerializeField] GameObject _backgroundPanelObj;
    #endregion
    [SerializeField] float _closeMinigameTimer = 3f;

    [SerializeField] private Transform _switchContainerTransform;
    [field: SerializeField, ReadOnly] public List<SwitchController> Switches { get; private set; } = new();
    
    void Start()
    {
        _startBtn.SetActive(true);
        _actualGame.SetActive(false);

        Switches = _switchContainerTransform.GetComponentsInChildren<SwitchController>().ToList();
        foreach(SwitchController controller in Switches)
            controller.Init(this);
    }

    public void TurnOnMinigame()
    {
        if (ToolManager.Instance.GetTool() == Tools.RegularMouse)
        {
            _actualGame.SetActive(true);
            _startBtn.SetActive(false);
        }
    }

    public void CheckIfAllSwitchesAreOn()
    {
        if (Switches.Count > 0 && Switches.All(controller => controller.CurrentState == LightSwithState.On))
        {
            StartCoroutine(OnAllSwitchesAreOn());
        }
    }

    private IEnumerator OnAllSwitchesAreOn()
    {
        Cursor.lockState = CursorLockMode.Locked;

        yield return new WaitForSeconds(_closeMinigameTimer);

        _actualGame.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        gameObject.SetActive(false);
        
        MinigameManager.Instance.FinishCurrentMinigame();
    }
}
