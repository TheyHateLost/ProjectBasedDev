using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoleController : MonoBehaviour, IPointerClickHandler
{
    public static event Action OnHoleComplete = delegate { };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (ToolManager.Instance.GetTool() == Tools.InsulationCan)
        {
            eventData.pointerClick.GetComponent<Image>().color = Color.black;
            OnHoleComplete.Invoke();
        }
    }
}
