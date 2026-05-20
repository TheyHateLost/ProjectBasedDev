using System;
using AYellowpaper.SerializedCollections;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class MinigameManager : Singleton<MinigameManager>
{
    [field: SerializeField, ReadOnly] public BuildingGenerator BuildingGenerator { get; private set; } 
    
    [field: SerializeField, ReadOnly] public GameObject ActiveMinigameObject { get; private set; }
    [field: SerializeField, ReadOnly] public Appliance ActiveMinigameAppliance { get; private set; }
    public bool IsMinigameActive => ActiveMinigameAppliance != null && ActiveMinigameObject != null;
    
    [field: SerializeField, Required] public Material ApplianceFinishedMaterial { get; private set; } 

    [field: SerializeField] public UnityEvent OnMinigameStarted { get; private set; } = new();
    [field: SerializeField] public UnityEvent OnMinigameFinished { get; private set; } = new();
    [field: SerializeField] public UnityEvent<int> OnBuildingRepairFinished { get; private set; } = new();
    [field: SerializeField] public UnityEvent OnBuildingRepairResumed { get; private set; } = new();
    
    [field: SerializeField, SerializedDictionary]
    public SerializedDictionary<int, int> RoomMinigamesRemaining { get; private set; } = new();
    
    [field: SerializeField, SerializedDictionary]
    public SerializedDictionary<int, float> RoomMinigameTimers { get; private set; } = new();

    public void AssignBuildingGenerator(BuildingGenerator generator)
    {
        BuildingGenerator = generator;
    }
    
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
        Destroy(ActiveMinigameObject.gameObject);
        ActiveMinigameObject = null;

        RoomMinigamesRemaining[BuildingGenerator.CurrentRoomIndex]--;
        OnMinigameFinished?.Invoke();

        if ( RoomMinigamesRemaining[BuildingGenerator.CurrentRoomIndex] <= 0)
        {
            FinishBuildingRepair();
        }
    }
    
    [Button("Cheat Finish Building Repair", ButtonSizes.Large)]
    private void FinishBuildingRepair()
    {
        OnBuildingRepairFinished?.Invoke(BuildingGenerator.CurrentRoomIndex);
    }

    public void RegisterAppliance(int buildingRoomIndex)
    {
        if(!RoomMinigamesRemaining.ContainsKey(buildingRoomIndex))
            RoomMinigamesRemaining.Add(buildingRoomIndex, 0);
        RoomMinigamesRemaining[buildingRoomIndex]++;
    }

    private void Update()
    {
        HandleMinigameTimer();
    }

    private void HandleMinigameTimer()
    {
        if (!IsMinigameActive)
            return;

        if (!RoomMinigameTimers.ContainsKey(BuildingGenerator.CurrentRoomIndex))
            RoomMinigameTimers.Add(BuildingGenerator.CurrentRoomIndex, 0);
        
        RoomMinigameTimers[BuildingGenerator.CurrentRoomIndex] += Time.deltaTime;
    }

    public float GetCurrentRoomMinigameTimer()
    {
        if (!RoomMinigameTimers.ContainsKey(BuildingGenerator.CurrentRoomIndex))
            return 0;
        
        return RoomMinigameTimers[BuildingGenerator.CurrentRoomIndex];
    }

    public bool AreAllRoomMinigamesFinished()
    {
        if (RoomMinigamesRemaining.Count != BuildingGenerator.CurrentRooms.Count)
            return false;

        foreach (int minigamesRemaining in RoomMinigamesRemaining.Values)
        {
            if (minigamesRemaining > 0)
                return false;
        }

        return true;
    }
    
    public void ResumeBuildingRepair() => OnBuildingRepairResumed?.Invoke();
}
