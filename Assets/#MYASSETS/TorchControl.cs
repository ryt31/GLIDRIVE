using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TorchControl : MonoBehaviour,ICollisionRunnable
{
    [SerializeField]
    private List<Torch> torches;

    void Start()
    {
        
    }

    public void CollisionRun()
    {
        foreach (var t in torches)
        {
            t.turnOnLight();
        }
    }
}
