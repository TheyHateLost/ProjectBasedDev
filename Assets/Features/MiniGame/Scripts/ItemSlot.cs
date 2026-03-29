using UnityEngine;
using System;
using UnityEngine.EventSystems;

public class ItemSlot : MonoBehaviour, IDropHandler
{
    public static event Action OnWin = delegate { };
    public void OnDrop(PointerEventData eventData)
    {
        //Debug.Log("OnDrop");
        if (eventData.pointerDrag != null)
        {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
            CheckPipe(eventData);
        }
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void CheckPipe(PointerEventData eventData)
    {
        GameObject pipeData = eventData.pointerDrag.gameObject;

        if (pipeData.CompareTag("BadPipe"))
        {
            Debug.LogWarning("Bad Pipe");
        }
        else if (pipeData.CompareTag("GoodPipe"))
        {
            OnWin.Invoke();
        }
    }
}
