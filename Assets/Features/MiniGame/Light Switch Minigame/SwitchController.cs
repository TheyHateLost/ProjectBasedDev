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
    private LightSwitchMinigameController _minigameController;
    
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

    public void Init(LightSwitchMinigameController minigameController)
    {
        _minigameController = minigameController;
    }
    
    void Start()
    {
        _lightPanel_Image = _lightPanelObj.GetComponent<Image>();
        _lightPanel_Image.color = CurrentState == LightSwithState.On ? ON_COLOR : OFF_COLOR;
    }

    public void FlipSwitch()
    {
        //Debug.Log($"Flipped switch index {_minigameController.Switches.FindIndex(controller => controller == this)}. Previous state: {CurrentState}");
        
        OppositeFlip(this);
        TurnOffNearbySwitch();
        _minigameController.CheckIfAllSwitchesAreOn();
    }

    void TurnOffNearbySwitch()
    {
        int selectedIndex = _minigameController.Switches.FindIndex(controller => controller == this);

        if (selectedIndex == 0)
        {
            SwitchController belowMostTop = _minigameController.Switches[1];
            OppositeFlip(belowMostTop);
            return;
        }

        if (selectedIndex == _minigameController.Switches.Count - 1)
        {
            SwitchController aboveMostBottom = _minigameController.Switches[_minigameController.Switches.Count - 2];
            OppositeFlip(aboveMostBottom);
            return;
        }

        SwitchController topSwitch = _minigameController.Switches[selectedIndex - 1];
        SwitchController bottomSwitch =_minigameController.Switches[selectedIndex + 1];

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
        if (obj.CurrentState == LightSwithState.On)
        {
            //Debug.Log($"Turned off switch index {_minigameController.Switches.FindIndex(controller => controller == obj)}. Previous state: {obj.CurrentState}");
            obj.TurnOff();
        }
        else
        {
            //Debug.Log($"Turned on switch index {_minigameController.Switches.FindIndex(controller => controller == obj)}. Previous state: {obj.CurrentState}");
            obj.TurnOn();
        }
    }
}
