using UnityEngine;

public class ToolManager : Singleton<ToolManager>
{
    Tools _currentTool;

    private void Awake()
    {
        base.Awake();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _currentTool = Tools.RegularMouse;    
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Tools GetTool() => _currentTool;
    public void SetTool(Tools newTool)
    {
      _currentTool = newTool;  
      PlayToolSound();
    }  
        

    void PlayToolSound()
    {
        switch(_currentTool)
        {
            case Tools.RegularMouse:
            // Add Audio Code
                break;
            case Tools.Screwdriver:
            // Add Audio Code
                break;
            case Tools.MagnifyingGlass:
            // Add Audio Code
                break;
            case Tools.InsulationCan:
            // Add Audio Code
                break;
            case Tools.Blowdoor:
            // Add Audio Code
                break;
        }
    }
}
