using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Torch : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> childList;

    void Start()
    {
        foreach(var c in childList)
        {
            c.SetActive(false);
        }
    }

    public void turnOnLight()
    {
        foreach (var c in childList)
        {
            c.SetActive(true);
        }
    }
}
