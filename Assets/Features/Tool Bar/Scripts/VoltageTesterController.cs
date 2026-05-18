using Sirenix.OdinInspector;
using Unity.VisualScripting;
using UnityEngine;

public class VoltageTesterController : MonoBehaviour
{
    public enum State
    {
        No_Feedback,
        Good_Feedback,
        Bad_Feedback
    }

    [Header("SO Section")]
    [SerializeField] ToolSO _noFeedback;
    [SerializeField] ToolSO _goodFeedback;
    [SerializeField] ToolSO _badFeedback;

    State _currentState;

    // Debugging Stuff


    private void OnEnable()
    {
        GameplayUIActions.OnVoltageTesterChange += SetStateOnSwitch;
    }

    private void OnDisable()
    {
        GameplayUIActions.OnVoltageTesterChange -= SetStateOnSwitch;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void SetStateOnSwitch()
    {
        if (ToolManager.Instance.GetTool() != Tools.VoltageTester) return;
        _currentState = State.No_Feedback;
    }

    [Button("Bad Feedback")]
    void DebugBadFeedback()
    {
        Vector2 cursorHotspot = _badFeedback.cursorTexture != null ? new Vector2(_badFeedback.cursorTexture.width / 2, _badFeedback.cursorTexture.height / 2) : Vector2.zero;
        Cursor.SetCursor(_badFeedback.cursorTexture ?? null, cursorHotspot, CursorMode.Auto);
    }

    [Button("Good Feedback")]
    void DebugGoodFeedback()
    {
        Vector2 cursorHotspot = _goodFeedback.cursorTexture != null ? new Vector2(_goodFeedback.cursorTexture.width / 2, _goodFeedback.cursorTexture.height / 2) : Vector2.zero;
        Cursor.SetCursor(_goodFeedback.cursorTexture ?? null, cursorHotspot, CursorMode.Auto);
    }

    [Button("No Feedback")]
    void DebugNoFeedback()
    {
        Vector2 cursorHotspot = _noFeedback.cursorTexture != null ? new Vector2(_noFeedback.cursorTexture.width / 2, _noFeedback.cursorTexture.height / 2) : Vector2.zero;
        Cursor.SetCursor(_noFeedback.cursorTexture ?? null, cursorHotspot, CursorMode.Auto);
    }
}
