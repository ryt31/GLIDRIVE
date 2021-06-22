using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stand : MonoBehaviour
{
    public Vector3 CalcStandPosition(Vector3 position)
    {
        return position - new Vector3(0.10f, -0.04f, -0.04f);
    }

    public void SetStandPosition(Vector3 position)
    {
        this.transform.position = CalcStandPosition(position);
    }
}
