using Unity.VisualScripting;
using UnityEngine;

public class CheckLeftMost : MonoBehaviour
{
    [SerializeField] LayerMask _selectedLayer;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {   
        
    }

    // Update is called once per frame
    void Update()
    {
        var mousePos = GetMousePos();
        RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.down, 1f, _selectedLayer);
        Debug.Log(hit.collider != null);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, GetComponent<CircleCollider2D>().radius * 1.85f);
    }

    Vector3 GetMousePos()
    {
        Vector3 mouse = Input.mousePosition; // raw mouse position on the entire screen
        mouse.z = 10f;
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(mouse);
        return mousePos;
    }
}
