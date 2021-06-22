using System.Collections;
using System.Collections.Generic;
using Glidrive.Manager;
using UnityEngine;
using UniRx;
using UniRx.Triggers;

public class ResultScreen : MonoBehaviour
{
    [SerializeField]
    private GameObject[] objects;
    
    private void Start()
    {
        var currentScene = SceneStateManager.Instance.CurrentScene;
        currentScene
            .Subscribe(state =>
            {
                switch (state)
                {
                    case SceneStateType.Result:
                        foreach (var o in objects)
                        {
                            o.SetActive(true);
                        }
                        break;
                }
            });
    }
}
