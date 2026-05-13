using Sirenix.OdinInspector;
using UnityEngine;
using System;

public class NotesHandler : MonoBehaviour
{
    [Header("Notes Section")]
    [SerializeField, Required] GameObject _prerequisiteNotesObj;
    [SerializeField, Required] GameObject _personalNotesObj;

    public static event Action OnDiscoveryUpdate = delegate { };


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TurnPreresquiteNotesOn();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnPersonalNotesOn()
    {
        _personalNotesObj.SetActive(true);
        _prerequisiteNotesObj.SetActive(false);
    }

    public void TurnPreresquiteNotesOn()
    {
        _personalNotesObj.SetActive(false);
        _prerequisiteNotesObj.SetActive(true);
    }
}
