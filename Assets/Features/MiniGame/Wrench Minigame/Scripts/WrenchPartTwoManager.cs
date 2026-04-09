using UnityEngine;
using System;
using System.Collections.Generic;
using UnityEngine.UI;

public class WrenchPartTwoManager : MonoBehaviour
{
    public static event Action OnPartTwoComplete = delegate { };

    private Queue<int> _screwsLeftQueue;
    private bool _didEventStart;

    [Header("Debug Section")]
    [SerializeField] GameObject[] _screws;

    private void OnEnable()
    {
        ScrewController.OnScrewComplete += UpdateCompletedScrews;

        ResetGame();
    }

    private void OnDisable()
    {
        ScrewController.OnScrewComplete -= UpdateCompletedScrews;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _didEventStart = false;

        _screwsLeftQueue = new Queue<int>();
        _screwsLeftQueue.Enqueue(1);
        _screwsLeftQueue.Enqueue(1);
        _screwsLeftQueue.Enqueue(1);
        _screwsLeftQueue.Enqueue(1);
    }

    // Update is called once per frame
    void Update()
    {
        if ((_screwsLeftQueue.Count == 0) && !_didEventStart)
        {
            OnPartTwoComplete.Invoke();
            _didEventStart = true;
        }
    }

    void UpdateCompletedScrews()
    {
        _screwsLeftQueue.Dequeue();
    }

    void ResetGame()
    {
        _didEventStart = false;

        _screwsLeftQueue = new Queue<int>();
        _screwsLeftQueue.Enqueue(1);
        _screwsLeftQueue.Enqueue(1);
        _screwsLeftQueue.Enqueue(1);
        _screwsLeftQueue.Enqueue(1);

        foreach (var screw in _screws)
        {
            screw.GetComponent<Image>().color = Color.white;
        }
    }
}
