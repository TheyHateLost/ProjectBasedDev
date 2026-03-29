using UnityEngine;

public class WrenchMinigameController : MonoBehaviour
{
    [Header("Pipe Section")]
    [SerializeField] GameObject _goodPipe;
    [SerializeField] GameObject _badPipe;
    [SerializeField] float _raycastMaxDistance = 100f;

    [Header("Tool Section")]
    [SerializeField] ScriptableObject _chosenTool;

    private Camera _camera;
    private bool _isDragging;

    private void Awake()
    {
        _isDragging = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (_isDragging)
        {
            transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }
    }

    private void OnMouseDown()
    {
        Debug.Log("works");
        _isDragging = true;
    }

    private void OnMouseUp()
    {
        _isDragging = false;
    }
}
