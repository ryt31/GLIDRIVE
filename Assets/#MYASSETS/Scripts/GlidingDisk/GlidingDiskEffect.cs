using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlidingDiskEffect : MonoBehaviour
{
    private SkinnedMeshRenderer mesh;
    private Collider collider;
    private void Start()
    {
        mesh = GetComponentInChildren<SkinnedMeshRenderer>();
        collider = GetComponent<Collider>();
    }

    public void Appear()
    {
        collider.enabled = true;
        mesh.enabled = true;
    }

    public void Disappear()
    {
        collider.enabled = false;
        mesh.enabled = false;
    }
    
}
