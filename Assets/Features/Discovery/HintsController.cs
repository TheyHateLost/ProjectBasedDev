using Sirenix.OdinInspector;
using TMPro;
using UnityEngine;

public class HintsController : MonoBehaviour
{
    [SerializeField, Required] private TMP_InputField _prerequisiteNotesText;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void AddingHint(DiscoveryTextSO discoverText)
    {
        _prerequisiteNotesText.text += (discoverText.text);
    }
}
