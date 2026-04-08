using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoleController : MonoBehaviour, IPointerClickHandler
{
    public static event Action OnHoleComplete = delegate { };

    static int _currentNumberOfClicks;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentNumberOfClicks = 0;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Hole is being clicked on");
        eventData.pointerClick.GetComponent<Image>().color = Color.black;
        OnHoleComplete.Invoke();
    }
}
