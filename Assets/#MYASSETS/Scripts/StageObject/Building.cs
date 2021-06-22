using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;

public class Building : MonoBehaviour, IDarkable, IDiskEnergyChangeable
{
    public void FadeIn()
    {
    }

    public void FadeOut()
    {
    }
    
    /// <summary>
    ///     エネルギー減少量を返す
    /// </summary>
    /// <returns>エネルギー減少量</returns>
    public float DecreaseEnergy()
    {
        return 100.0f;
    }
}
