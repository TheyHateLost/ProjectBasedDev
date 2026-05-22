using Sirenix.OdinInspector;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class NotesHandler : MonoBehaviour
{
    [Header("Notes Section")]
    [SerializeField, Required] GameObject _prerequisiteNotesObj;
    [SerializeField, Required] GameObject _personalNotesObj;

    [SerializeField] DiscoveryTextSO _alwaysOnScreenHint;
    TMP_InputField _prerequisiteNotes;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        TurnPreresquiteNotesOn();
        _prerequisiteNotes = GameObject.Find("PrerequisiteNotesInputField").GetComponent<TMP_InputField>();
        _prerequisiteNotes.text = _alwaysOnScreenHint.text;
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

    public string GetAlwaysOnScreenHint() => _alwaysOnScreenHint.text;
}
