using System;
using Sirenix.OdinInspector;
using UnityEngine;

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
        Ray mouseRay = _camera.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(mouseRay, out RaycastHit hit, _raycastMaxDistance, _selectableLayerMask);
        
        SelectableObject hitSelectableObject = hit.collider == null ? null : hit.collider.GetComponent<SelectableObject>();
        if (hitSelectableObject != CurrentlySelectedObject)
        {
            // deselect prev
            if(CurrentlySelectedObject != null)
                CurrentlySelectedObject.ResetMaterials();
            
            CurrentlySelectedObject = hitSelectableObject;
            if(CurrentlySelectedObject != null)
                CurrentlySelectedObject.ChangeMaterials(_selectedMaterial);
        }
    }
}