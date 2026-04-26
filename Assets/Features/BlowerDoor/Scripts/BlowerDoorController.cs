using UnityEngine;

public enum Signal
{
    Good,
    Bad,
    Pending
}
public class BlowerDoorController : MonoBehaviour
{
    [Header("Gameobject Settings")]
    [SerializeField] GameObject _signal;
    [SerializeField] GameObject _airPressureChecker;

    [Header("Materials Settings")]
    #region MATERIAL
    [SerializeField] Material _goodSignalMat;
    [SerializeField] Material _badSignalMat;
    Material _originalMat;
    #endregion
    #region RENDERER
    Renderer _blowerDoorRenderer;
    Renderer _airPressureCheckerRenderer;
    Renderer _signalRenderer;
    #endregion
    // Debug
    [SerializeField] bool _signalDebug = true;

    [Header("Timer Settings")]
    [SerializeField] float _timer;
    bool _isTimerActive = false;
    float _currentTimer = 0f;

    Signal _currentSignal;

    private void Awake()
    {
        _blowerDoorRenderer = GetComponent<Renderer>();
        _signalRenderer = _signal.GetComponent<Renderer>();
        _airPressureCheckerRenderer = _airPressureChecker.GetComponent<Renderer>();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _originalMat = _blowerDoorRenderer.material;
        _isTimerActive = true;
        _currentSignal = Signal.Pending;
    }

    // Update is called once per frame
    void Update()
    {
        Timer();
    }

    void Timer()
    {
        if (_isTimerActive)
        {
            _currentTimer += Time.deltaTime;
            Debug.Log(_currentTimer);
        }

        if (_currentTimer >= _timer)
        {
            _isTimerActive = false;
            _currentTimer = 0f;
            _signalRenderer.material = _signalDebug ? _goodSignalMat : _badSignalMat;
        }
    }

    public Signal GetCurrentSignal() => _currentSignal;
}
