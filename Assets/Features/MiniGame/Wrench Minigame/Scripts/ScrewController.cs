using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;
using TMPro;

public class ScrewController : MonoBehaviour, IPointerClickHandler
{
    public static event Action OnScrewComplete = delegate { };
    [SerializeField] int _numOfClicks = 4;
    static int _currNumOfClicks;
    bool _isClicked = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currNumOfClicks = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (ToolManager.Instance.GetTool() == Tools.Screwdriver)
        {
            _currNumOfClicks++;

            if (_currNumOfClicks % _numOfClicks == 0 && !_isClicked)
            {
                eventData.pointerClick.GetComponent<Image>().color = Color.darkGreen;
                OnScrewComplete.Invoke();
                _isClicked = true;
            }
        }
    }
}
