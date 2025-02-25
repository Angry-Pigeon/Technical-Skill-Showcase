using System;
using System.Collections;
using System.Collections.Generic;
using Parabox.CSG;
using Sirenix.OdinInspector;
using UnityEngine;

public class RuntimeHoleSubtractionPB : MonoBehaviour
{
    [Header("Assign in Inspector")]
    [Tooltip("The ground object (with a MeshFilter) from which the hole will be subtracted.")]
    public GameObject groundObject;
    
    [Tooltip("The hole object (e.g., a cylinder) that defines the volume to subtract.")]
    public GameObject holeObject;
    
    [Tooltip("The object that will display the resulting mesh (should have a MeshFilter and MeshCollider).")]
    public GameObject resultObject;
    
    [Header("Performance Settings")]
    [Tooltip("Time interval in seconds between CSG updates.")]
    public float updateInterval = 0.5f;

    // Timer to control update intervals.
    private float timer = 0f;


    void Update()
    {
        timer += Time.deltaTime;
        if (timer < updateInterval)
            return;
        timer = 0f;
        
        // Cut();
        
    }

    private void LateUpdate()
    {
        Cut();
    }

    private void Cut()
    {
        Model result = CSG.Subtract(groundObject, holeObject);
        // Model result = CSG.Subtract(holeObject, groundObject);
        // CSG.Intersect()
        
        if (result == null)
        {
            Debug.LogWarning("CSG operation failed.");
            return;
        }
        
        MeshFilter filter = resultObject.GetComponent<MeshFilter>();
        MeshRenderer renderer = resultObject.GetComponent<MeshRenderer>();
        MeshCollider collider = resultObject.GetComponent<MeshCollider>();
        
        filter.mesh = result.mesh;  
        renderer.sharedMaterials = result.materials.ToArray();
        collider.sharedMesh = result.mesh;
        
        
    }
}
