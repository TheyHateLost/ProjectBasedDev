using UnityEngine;

public class ToolManager : Singleton<ToolManager>
{
    Tools _currentTool;
    [Header("Tools Audio")]
    [SerializedField] private AudioSource _audioSource;
    [SerializedField] private AudioClip regularMouseSound;
    [SerializedField] private AudioClip ScrewdriverSound;
    [SerializedField] private AudioClip MagnifyingGlassSound;
    [SerializedField] private AudioClip InsulationCanSound; 
    [SerailzedField] private AudioClip BlowdoorSound; 

 
    

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
            clip = regularMouseSound;
                break;
            case Tools.Screwdriver:
            clip = ScrewdriverSound 
                break;
            case Tools.MagnifyingGlass:
            clip = MagnifyingGlassSound
                break;
            case Tools.InsulationCan:
            clip = InsulationCanSound
                break;
            case Tools.Blowdoor:
            clip = BlowdoorSound
                break;
            if (clip != null && _audioSource != null)
            _audioSource.PlayOneShot(clip);
        }
    }
}
