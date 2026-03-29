using System;
using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class MinigameController : MonoBehaviour
{
    [Header("UI Section")]
    [SerializeField] GameObject startBtn;
    [SerializeField] GameObject actualGame;
    [SerializeField] GameObject winText;
    [SerializeField] float closeMinigameTimer = 10f;

    bool gameActive = false;


    private void OnEnable()
    {
        ItemSlot.OnWin += Win;
    }

    private void OnDisable()
    {
        ItemSlot.OnWin -= Win;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startBtn.SetActive(true);
        actualGame.SetActive(false);
        winText.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void TurnOnMinigame()
    {
        actualGame.SetActive(true);
        startBtn.SetActive(false);

        gameActive = true;
    }

    void Win()
    {
        StartCoroutine(TurnOnWinText());
    }

    IEnumerator TurnOnWinText()
    {
        winText.SetActive(true);

        yield return new WaitForSeconds(closeMinigameTimer);

        actualGame.SetActive(false);
        winText.SetActive(false);
    }
}
