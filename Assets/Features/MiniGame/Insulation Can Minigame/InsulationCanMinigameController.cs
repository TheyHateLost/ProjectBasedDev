using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InsulationCanMinigameController : MonoBehaviour
{
    [Header("UI Section")]
    [SerializeField] GameObject _startBtn;
    [SerializeField] GameObject _actualGame;
    [SerializeField] GameObject _winText;
    [SerializeField] float _closeMinigameTimer = 3f;
    bool _isGameActive = false;

    [Header("Debug Section")]
    [SerializeField] GameObject[] _holes;

    #region GAMEPLAY
    private Queue<int> _holesLeftQueue;
    private bool _didEventStart;
    #endregion

    private void OnEnable()
    {
        HoleController.OnHoleComplete += UpdateCompletedHoles;
        _isGameActive = true;

        ResetGame();
    }

    private void OnDisable()
    {
        HoleController.OnHoleComplete -= UpdateCompletedHoles;
        _isGameActive = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _startBtn.SetActive(true);
        _actualGame.SetActive(false);
        _winText.SetActive(false);

        _holesLeftQueue = new Queue<int>();
        _didEventStart = false;
        // Rewrite this if we plan on having n (random number) of holes
        _holesLeftQueue.Enqueue(0);
        _holesLeftQueue.Enqueue(0);
        _holesLeftQueue.Enqueue(0);
        _holesLeftQueue.Enqueue(0);

        foreach (var hole in _holes)
        {
            hole.GetComponent<Image>().color = Color.white;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if ((_holesLeftQueue.Count == 0) && !_didEventStart)
        {
            StartCoroutine(TurnOnWinText());
            _didEventStart = true;
        }
    }

    public void TurnOnMinigame()
    {
        if (ToolManager.Instance.GetTool() == Tools.InsulationCan)
        {
            _actualGame.SetActive(true);
            _startBtn.SetActive(false);
        }
    }

    void UpdateCompletedHoles()
    {
        _holesLeftQueue.Dequeue();
    }

    IEnumerator TurnOnWinText()
    {
        _winText.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;

        yield return new WaitForSeconds(_closeMinigameTimer);

        _actualGame.SetActive(false);
        _winText.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        gameObject.SetActive(false);
    }

    void ResetGame()
    {
        if (_isGameActive)
        {
            _startBtn.SetActive(true);
            _actualGame.SetActive(false);
            _winText.SetActive(false);

            _holesLeftQueue = new Queue<int>();
            _didEventStart = false;
            // Rewrite this if we plan on having n (random number) of holes
            _holesLeftQueue.Enqueue(0);
            _holesLeftQueue.Enqueue(0);
            _holesLeftQueue.Enqueue(0);
            _holesLeftQueue.Enqueue(0);

            foreach (var hole in _holes)
            {
                hole.GetComponent<Image>().color = Color.white;
            }
        }
    }
}
