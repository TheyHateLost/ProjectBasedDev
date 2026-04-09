using UnityEngine;

public class HintDiscoveryController : MonoBehaviour, IDiscoveryController
{
    [SerializeField] GameObject _selectedMinigame;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UseHint(ToolSO selectedTool)
    {
        if (ToolManager.Instance.GetTool() == selectedTool.GetTool())
        {
            // Possbily a minigame object pool that turns on a minigame based on a selected hint
            TurnOnMinigame();   
        }
    }

    void TurnOnMinigame() => _selectedMinigame.SetActive(true);
}
