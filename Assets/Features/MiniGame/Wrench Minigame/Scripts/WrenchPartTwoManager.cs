using UnityEngine;
using System;
using System.Collections.Generic;

public class WrenchPartTwoManager : MonoBehaviour
{
    public static event Action OnPartTwoComplete = delegate { };

    private bool _isComplete;
    private int _numOfCompletedScrews;
    private Queue<int> _screwsLeftQueue;
    private bool _didEventStart;

    const int TOTAL_NUMBER_OF_SCREWS = 4;

    private void OnEnable()
    {
        ScrewController.OnScrewComplete += IncrementCompletedScrews;
    }

    private void OnDisable()
    {
        ScrewController.OnScrewComplete -= IncrementCompletedScrews;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _isComplete = false;
        _didEventStart = false;
        _numOfCompletedScrews = 0;

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

    void IncrementCompletedScrews()
    {
        Debug.Log("Added another screw");
        _screwsLeftQueue.Dequeue();
    }
}
