using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
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
    LightSwithState _currentState = LightSwithState.Off;
    #endregion

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _lightPanel_Image = _lightPanelObj.GetComponent<Image>();
        _lightPanel_Image.color = ON_COLOR;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void FlipSwitch()
    {
        switch(_currentState)
        {
            case LightSwithState.On:
                TurnOff();
                break;
            case LightSwithState.Off:
                TurnOn();
                break;
        }

        //Debug.Log(LightSwitchMinigameController.SwitchesDict[this.gameObject]);
        TurnOffNearbySwitch();
    }

    void TurnOffNearbySwitch()
    {
        int selectedIndex = LightSwitchMinigameController.SwitchesDict[this.gameObject];

        if (selectedIndex == 0)
        {
            LightSwitchMinigameController.ListOfSwitches[selectedIndex + 1].GetComponent<SwitchController>().TurnOff();
            return;
        }

        if (selectedIndex == LightSwitchMinigameController.ListOfSwitches.Count-1)
        {
            LightSwitchMinigameController.ListOfSwitches[selectedIndex - 1].GetComponent<SwitchController>().TurnOff();
            return;
        }

        // Top Switch
        LightSwitchMinigameController.ListOfSwitches[selectedIndex + 1].GetComponent<SwitchController>().TurnOff();
        // Bottom Switch
        LightSwitchMinigameController.ListOfSwitches[selectedIndex - 1].GetComponent<SwitchController>().TurnOff();
    }

    public void TurnOn()
    {
        _currentState = LightSwithState.On;
        _lightPanel_Image.color = ON_COLOR;
    }
    public void TurnOff()
    {
        _currentState = LightSwithState.Off;
        _lightPanel_Image.color = OFF_COLOR;
    }


}
/*
 * Notes: 
 * Works. The first button is bugged to not work. Fix that
 * 
*/
