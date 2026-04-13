using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using UnityEngine.EventSystems;

public class WrenchMinigameController : MonoBehaviour
{
    [Header("UI Section")]
    [SerializeField] GameObject _startBtn;
    [SerializeField] GameObject _gamePartOne;
    [SerializeField] GameObject _gamePartTwo;
    [SerializeField] GameObject _winText;
    [SerializeField] float _closeMinigameTimer = 10f;

    [Header("Debug Section")]
    [SerializeField] GameObject _goodPipeLoc;
    [SerializeField] GameObject _goodPipe;
    [SerializeField] GameObject _badPipe;
    [SerializeField] GameObject _pipeSlot;

    bool _isGameActive = false;


    private void OnEnable()
    {
        PipeSlot.OnPartOneComplete += MoveToPartTwo;
        WrenchPartTwoManager.OnPartTwoComplete += Win;

        _isGameActive = true;
        ResetGame();
    }

    private void OnDisable()
    {
        PipeSlot.OnPartOneComplete -= MoveToPartTwo;
        WrenchPartTwoManager.OnPartTwoComplete -= Win;

        _isGameActive = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _startBtn.SetActive(true);
        _gamePartOne.SetActive(false);
        _gamePartTwo.SetActive(false);
        _winText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    void MoveToPartTwo()
    {
        _gamePartTwo.SetActive(true);
    }

    public void TurnOnMinigame()
    {
        if (ToolManager.Instance.GetTool() == Tools.Wrench)
        {
            _gamePartOne.SetActive(true);
            _startBtn.SetActive(false);
        } 
    }

    void Win()
    {
        StartCoroutine(TurnOnWinText());
    }

    IEnumerator TurnOnWinText()
    {
        _winText.SetActive(true);
        Cursor.lockState = CursorLockMode.Locked;

        yield return new WaitForSeconds(_closeMinigameTimer);

        _gamePartOne.SetActive(false);
        _gamePartTwo.SetActive(false);
        _winText.SetActive(false);
        Cursor.lockState = CursorLockMode.None;
        gameObject.SetActive(false);
        
        MinigameManager.Instance.FinishCurrentMinigame();
    }

    void ResetGame()
    {
        _startBtn.SetActive(true);
        _gamePartOne.SetActive(false);
        _gamePartTwo.SetActive(false);
        _winText.SetActive(false);

        _badPipe.GetComponent<RectTransform>().anchoredPosition = _pipeSlot.GetComponent<RectTransform>().anchoredPosition;
        _goodPipe.GetComponent<RectTransform>().anchoredPosition = _goodPipeLoc.GetComponent<RectTransform>().anchoredPosition;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(_goodPipeLoc.transform.position, 1f);
    }
}
