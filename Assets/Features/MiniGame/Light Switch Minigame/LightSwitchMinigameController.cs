using AYellowpaper.SerializedCollections;
using NUnit.Framework;
using System.Collections.Generic;
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

    int _numOfSwitches;
    public static List<GameObject> ListOfSwitches { get; private set; }
    public static SerializedDictionary<GameObject, int> SwitchesDict { get; private set; }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _startBtn.SetActive(true);
        _actualGame.SetActive(false);

        _numOfSwitches = _backgroundPanelObj.transform.childCount;
        ListOfSwitches = new List<GameObject>();
        SwitchesDict = new();

        ListSetup();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOnMinigame()
    {
        if (ToolManager.Instance.GetTool() == Tools.RegularMouse)
        {
            _actualGame.SetActive(true);
            _startBtn.SetActive(false);
        }
    }

    void ListSetup()
    {
        for (int i = 0; i < _numOfSwitches; i++)
        {
            GameObject currChild = _backgroundPanelObj.transform.GetChild(i).gameObject;

            ListOfSwitches.Add(currChild);
            SwitchesDict.Add(currChild, i);
        }
    }
}
