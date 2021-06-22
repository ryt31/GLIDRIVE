using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ObstacleBase : MonoBehaviour
{
    protected Coroutine coroutine = null;

    public void Respawn()
    {
        if (coroutine == null)
        {
            coroutine = StartCoroutine(RespawnCoroutine());
        }
    }

    protected abstract IEnumerator RespawnCoroutine();
}
