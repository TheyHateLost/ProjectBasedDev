using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.EventSystems;

public class ObjectSelector : MonoBehaviour
{
    private Camera _camera;

    [SerializeField] private LayerMask _selectableLayerMask;
    [SerializeField] private float _raycastMaxDistance = 100f;
    [field: SerializeField, ReadOnly] public SelectableObject CurrentlySelectedObject { get; private set; }
    [SerializeField, Required] private Material _selectedMaterial;

    private void Awake()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        ScanForSelections();
        HandleMouseClick();
    }

    private void ScanForSelections()
    {
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
        {
            DeselectCurrent();
            return;
        }

        Ray mouseRay = _camera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(mouseRay, out RaycastHit hit, _raycastMaxDistance, _selectableLayerMask);

        SelectableObject hitSelectableObject = hit.collider == null ? null : hit.collider.GetComponent<SelectableObject>();
        if (hitSelectableObject != CurrentlySelectedObject)
        {
            DeselectCurrent();
            CurrentlySelectedObject = hitSelectableObject;
            
            // if valid
            if (CurrentlySelectedObject != null && CurrentlySelectedObject.enabled)
            {
                CurrentlySelectedObject.ChangeMaterials(_selectedMaterial);
            }
        }
    }

    private void HandleMouseClick()
    {
        if (!Input.GetKeyDown(KeyCode.Mouse0))
            return;

        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        if (CurrentlySelectedObject == null)
            return;

        CurrentlySelectedObject.ClickObject();
    }

    private void DeselectCurrent()
    {
        if (CurrentlySelectedObject == null)
            return;

        CurrentlySelectedObject.ResetMaterials();
        CurrentlySelectedObject = null;
    }
}