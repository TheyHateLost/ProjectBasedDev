using UnityEngine;
//using UnityEngine.UIElements;

public class GameplayUIActions : MonoBehaviour
{
    [Header("Notes Section")]
    [SerializeField] GameObject _notesPanel;

    [Header("Tools Section")]
    [SerializeField] GameObject _toolBarObj;

    [Header("Leftmost Settings")]
    [SerializeField] GameObject _leftMost;
    [SerializeField] float _leftMostRadius;

    bool _isToolbarActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _notesPanel.SetActive(false);
        _toolBarObj.SetActive(_isToolbarActive);
    }

    // Update is called once per frame
    void Update()
    {
        TurningOnAndOffToolbar();
    }

    public void ChangeCursorIcon(ToolSO icon)
    {
        Vector2 cursorHotspot = icon.cursorTexture != null ? new Vector2(icon.cursorTexture.width /2, icon.cursorTexture.height / 2) : Vector2.zero;
        Cursor.SetCursor(icon.cursorTexture ?? null, cursorHotspot, CursorMode.Auto);
    }

    public void TestBtn() => Debug.Log("God bless this works");

    public void TurnOnNotes() => _notesPanel.SetActive(true);
    public void TurnOffNotes() => _notesPanel.SetActive(false);
    public void TurningOnAndOffToolbar()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            _isToolbarActive = !_isToolbarActive;
            _toolBarObj.SetActive(_isToolbarActive);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(_leftMost.transform.position, _leftMostRadius);
    }
}
