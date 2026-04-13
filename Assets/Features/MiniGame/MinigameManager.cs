using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class MinigameManager : Singleton<MinigameManager>
{
    [field: SerializeField, ReadOnly] public GameObject ActiveMinigameObject { get; private set; }
    [field: SerializeField, ReadOnly] public Appliance ActiveMinigameAppliance { get; private set; }
    public bool IsMinigameActive => ActiveMinigameAppliance != null && ActiveMinigameObject != null;
    
    [field: SerializeField, Required] public Material ApplianceFinishedMaterial { get; private set; } 

    [field: SerializeField] public UnityEvent OnMinigameStarted { get; private set; } = new();
    [field: SerializeField] public UnityEvent OnMinigameFinished { get; private set; } = new();

    public void StartMinigame(Appliance appliance, GameObject minigameObject)
    {
        if (IsMinigameActive)
        {
            Debug.LogWarning($"There is an active minigame already.");
            return;
        }
        
        ActiveMinigameAppliance = appliance;
        ActiveMinigameObject = Instantiate(minigameObject);
        
        OnMinigameStarted?.Invoke();
    }

    public void FinishCurrentMinigame()
    {
        if (!IsMinigameActive)
            return;
        
        ActiveMinigameAppliance.FinishMinigame();
        ActiveMinigameAppliance = null;
        ActiveMinigameObject = null;
        
        OnMinigameFinished?.Invoke();
    }
}
