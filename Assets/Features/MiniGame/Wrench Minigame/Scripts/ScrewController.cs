using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class ScrewController : MonoBehaviour, IPointerClickHandler
{
    public static event Action OnScrewComplete = delegate { };
    [SerializeField] int _numOfClicks = 4;
    static int _currNumOfClicks;

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
        _currNumOfClicks++;
        Debug.Log(_currNumOfClicks);

        if (_currNumOfClicks % _numOfClicks == 0)
        {
            eventData.pointerClick.GetComponent<Image>().color = Color.darkGreen;
            OnScrewComplete.Invoke();
        }
    }
}
