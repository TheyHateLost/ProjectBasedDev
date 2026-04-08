using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class PipeSlot : MonoBehaviour, IDropHandler
{
    public static event Action OnPartOneComplete = delegate { };

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnDrop(PointerEventData eventData)
    {
        // EventData.pointerDrag is like the collider obj, allows u to grab the info from the object
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            CheckPipe(eventData);
        }
    }

    void CheckPipe(PointerEventData eventData)
    {
        GameObject pipeData = eventData.pointerDrag.gameObject;

        if (pipeData.CompareTag("GoodPipe")) OnPartOneComplete.Invoke();
    }
}
