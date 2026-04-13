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
        /*Disable toolbar button for now
        InputManager.Instance.Toolbar += TurningOnAndOffToolbar;*/
    }

    private void OnDisable()
    {
        /*Disable toolbar button for now
        if (InputManager.Instance != null)
        {
            InputManager.Instance.Toolbar -= TurningOnAndOffToolbar;
        }*/
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _notesPanel.SetActive(_isNotesActive);
        _toolBarObj.SetActive(_isToolbarActive);

        // Raising the z-index
        Vector3 canvasPos = (Vector3)GetComponent<RectTransform>().localPosition;
        canvasPos.z = 20f;
        
        // Enable toolbar + notes by default
        TurningOnAndOffToolbar();
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

        if(_isToolbarActive)
            TurnOnNotes();
        else
            TurnOffNotes();
    }
}
