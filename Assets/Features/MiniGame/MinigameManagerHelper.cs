using UnityEngine;

public class MinigameManagerHelper : MonoBehaviour
{
    public void FinishCurrentMinigame()
    {
        MinigameManager.Instance.FinishCurrentMinigame();
    }
}