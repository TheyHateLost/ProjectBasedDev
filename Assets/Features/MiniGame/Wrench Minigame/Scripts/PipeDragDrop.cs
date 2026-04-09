using UnityEngine;
using UnityEngine.EventSystems;

public class PipeDragDrop : MonoBehaviour, IPointerDownHandler, IBeginDragHandler, IEndDragHandler, IDragHandler
{
    [SerializeField] Canvas _canvas;
    [SerializeField, Range(0.1f, 1f)] float _transparencyLevel = 0.6f;

    RectTransform _rectTransform;
    CanvasGroup _canvasGroup;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (ToolManager.Instance.GetTool() == Tools.Wrench)
        {
            _canvasGroup.blocksRaycasts = false;
            _canvasGroup.alpha = _transparencyLevel;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (ToolManager.Instance.GetTool() == Tools.Wrench)
        {
            _rectTransform.anchoredPosition += (eventData.delta / _canvas.scaleFactor);
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        const float FULLY_VISIBLE = 1f;

        _canvasGroup.alpha = FULLY_VISIBLE;
        _canvasGroup.blocksRaycasts = true;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        //Debug.Log("OnPointerDown");
    }

}
