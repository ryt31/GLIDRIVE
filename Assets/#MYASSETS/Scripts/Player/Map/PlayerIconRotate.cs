using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class PlayerIconRotate : MonoBehaviour
{
    public Camera _camera;

    void Update()
    {
        this.transform.rotation = Quaternion.Euler(270.0f, _camera.transform.rotation.eulerAngles.y, 0.0f);
    }
}
