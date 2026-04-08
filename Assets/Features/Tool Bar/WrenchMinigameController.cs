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


    private void OnEnable()
    {
        PipeSlot.OnPartOneComplete += MoveToPartTwo;
        WrenchPartTwoManager.OnPartTwoComplete += Win;
    }

    private void OnDisable()
    {
        PipeSlot.OnPartOneComplete -= MoveToPartTwo;
        WrenchPartTwoManager.OnPartTwoComplete -= Win;
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
        } else
        {
            Debug.LogError("Wrong Tool. The correct tool is Wrench");
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
    }
}
