using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEditor.Search;
using UnityEngine;
using UnityEngine.UI;

public enum LightSwithState
{
    On,
    Off
}

public class SwitchController : MonoBehaviour
{
    #region GameObjects
    [SerializeField] GameObject _lightPanelObj;
    #endregion
    #region Colors
    readonly Color ON_COLOR = Color.greenYellow;
    readonly Color OFF_COLOR = Color.darkGreen;
    #endregion
    #region Components
    Image _lightPanel_Image;
    #endregion
    #region State
    [field: SerializeField] public LightSwithState CurrentState { get; private set; } = LightSwithState.Off;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _lightPanel_Image = _lightPanelObj.GetComponent<Image>();
        _lightPanel_Image.color = CurrentState == LightSwithState.On ? ON_COLOR : OFF_COLOR;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FlipSwitch()
    {
        switch (CurrentState)
        {
            case LightSwithState.On:
                TurnOff();
                break;
            case LightSwithState.Off:
                TurnOn();
                break;
        }

        TurnOffNearbySwitch();
    }

    void TurnOffNearbySwitch()
    {
        int selectedIndex = LightSwitchMinigameController.SwitchesDict[this.gameObject];

        if (selectedIndex == 0)
        {
            SwitchController belowMostTop = LightSwitchMinigameController.ListOfSwitches[1].GetComponent<SwitchController>();
            OppositeFlip(belowMostTop);
            return;
        }

        if (selectedIndex == LightSwitchMinigameController.ListOfSwitches.Count - 1)
        {
            SwitchController aboveMostBottom = LightSwitchMinigameController.ListOfSwitches[LightSwitchMinigameController.ListOfSwitches.Count - 2].GetComponent<SwitchController>();
            OppositeFlip(aboveMostBottom);
            return;
        }

        SwitchController topSwitch = LightSwitchMinigameController.ListOfSwitches[selectedIndex - 1].GetComponent<SwitchController>() ?? null;
        SwitchController bottomSwitch = LightSwitchMinigameController.ListOfSwitches[selectedIndex + 1].GetComponent<SwitchController>() ?? null;

        OppositeFlip(topSwitch);
        OppositeFlip(bottomSwitch);
    }

    public void TurnOn()
    {
        CurrentState = LightSwithState.On;
        _lightPanel_Image.color = ON_COLOR;
    }
    public void TurnOff()
    {
        CurrentState = LightSwithState.Off;
        _lightPanel_Image.color = OFF_COLOR;
    }

    public void OppositeFlip(SwitchController obj)
    {
        if (CurrentState == LightSwithState.On)
        {
            obj.TurnOff();
        }
        else
        {
            obj.TurnOn();
        }
    }
}
