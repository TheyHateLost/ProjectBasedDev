using UnityEngine;

public class MouseInteraction : MonoBehaviour
{
    [SerializeField] LayerMask _layerMask;

    private void OnEnable()
    {
        InputManager.Instance.LeftClick += TestingLeftClick;
        InputManager.Instance.ScrollWheel += TestingScrollWheel;
    }

    private void OnDisable()
    {
        if (InputManager.Instance != null)
        {
            InputManager.Instance.LeftClick -= TestingLeftClick;
            InputManager.Instance.ScrollWheel -= TestingScrollWheel;
        }
    }


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    void TestingLeftClick()
    {
        Debug.Log("Testing Left Click");
    }

    void TestingScrollWheel()
    {
        Debug.Log("Testing Scroll Wheel");
    }
}
