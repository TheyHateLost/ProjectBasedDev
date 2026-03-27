using UnityEngine;

public class MinigameController : MonoBehaviour
{
    [Header("UI Section")]
    [SerializeField] GameObject startBtn;
    [SerializeField] GameObject actualGame;

    bool gameActive = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        startBtn.SetActive(true);
        actualGame.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void TurnOnMinigame()
    {
        actualGame.SetActive(true);
        startBtn.SetActive(false);

        gameActive = true;
    }
    //public void TurnOffMinigame() => _miniGameObj.SetActive(false);
}
