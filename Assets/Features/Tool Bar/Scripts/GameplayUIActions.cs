using UnityEngine;
//using UnityEngine.UIElements;

public class GameplayUIActions : MonoBehaviour
{
    [Header("Notes Section")]
    [SerializeField] GameObject _notesPanel;

    [Header("Tools Section")]
    [SerializeField] GameObject _toolBarObj;

    bool _isToolbarActive = false;
    bool _isNotesActive = false;

    private void OnEnable()
    {
        InputManager.Instance.Toolbar += TurningOnAndOffToolbar;
    }

    private void OnDisable()
    {
        InputManager.Instance.Toolbar -= TurningOnAndOffToolbar;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _notesPanel.SetActive(_isNotesActive);
        _toolBarObj.SetActive(_isToolbarActive);
    }

    public void ChangeCursorIcon(ToolSO icon)
    {
        Vector2 cursorHotspot = icon.cursorTexture != null ? new Vector2(icon.cursorTexture.width /2, icon.cursorTexture.height / 2) : Vector2.zero;
        Cursor.SetCursor(icon.cursorTexture ?? null, cursorHotspot, CursorMode.Auto);
        ToolManager.Instance.SetTool(icon.GetTool());
    }

    public void TurnOnNotes() => _notesPanel.SetActive(true);
    public void TurnOffNotes() => _notesPanel.SetActive(false);

    void TurningOnAndOffToolbar()
    {
        _isToolbarActive = !_isToolbarActive;
        _toolBarObj.SetActive(_isToolbarActive);
    }

}
