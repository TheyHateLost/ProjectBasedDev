using UnityEngine;

public class ToolManager : Singleton<ToolManager>
{
    Tools _currentTool;
    [Header("Tools Audio")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip regularMouseSound;
    [SerializeField] private AudioClip ScrewdriverSound;
    [SerializeField] private AudioClip MagnifyingGlassSound;
    [SerializeField] private AudioClip InsulationCanSound; 
    [SerializeField] private AudioClip BlowdoorSound; 

 
    

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
        AudioClip clip = null;
        switch(_currentTool)
        {
            case Tools.RegularMouse:
            clip = regularMouseSound;
                break;
            case Tools.Screwdriver:
            clip = ScrewdriverSound;
                break;
            case Tools.MagnifyingGlass:
            clip = MagnifyingGlassSound;
                break;
            case Tools.InsulationCan:
            clip = InsulationCanSound;
                break;
            case Tools.Blowdoor:
            clip = BlowdoorSound;
                break;
        }
         if (clip != null && _audioSource != null)
            _audioSource.PlayOneShot(clip);
    }
}
