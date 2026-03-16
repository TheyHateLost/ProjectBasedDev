using UnityEngine;

public class CheckLeftMost : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D hit = Physics2D.OverlapPoint(mousePos);
        if (hit != null)
        {
            // Mouse is over this 2D object
            Debug.Log("Mouse is hovering over");
        }
    }
}
