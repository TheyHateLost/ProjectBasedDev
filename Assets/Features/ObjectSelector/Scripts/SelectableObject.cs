using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SelectableObject : MonoBehaviour
{
    private Dictionary<Renderer, Material[]> _originalMaterials = new Dictionary<Renderer, Material[]>();
    
    private void Awake()
    {
        GetOriginalMaterials();
    }

    private void GetOriginalMaterials()
    {
        foreach (var r in GetComponentsInChildren<Renderer>())
        {
            _originalMaterials.Add(GetComponent<Renderer>(), GetComponent<Renderer>().sharedMaterials);
        }
    }

    public void ResetMaterials()
    {
        foreach (var kvp in _originalMaterials)
        {
            kvp.Key.SetMaterials(kvp.Value.ToList());
        }
    }
    
    public void ChangeMaterials(Material newMaterial)
    {
        foreach (var kvp in _originalMaterials)
        {
            kvp.Key.SetMaterials(new List<Material>(){newMaterial});
        }
    }
}