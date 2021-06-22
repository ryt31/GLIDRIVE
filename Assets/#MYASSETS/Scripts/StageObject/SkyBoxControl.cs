using System.Collections;
using System.Collections.Generic;
using Glidrive.Manager;
using UnityEngine;
using UniRx;

public class SkyBoxControl : MonoBehaviour
{
    [SerializeField]
    private Material daySun;
    [SerializeField]
    private Material night;
    private void Start()
    {
        var currentScene = SceneStateManager.Instance.CurrentScene;
        
        currentScene
            .Subscribe(state =>
            {
                switch (state)
                {
                    case SceneStateType.Title:
                        ChangeSkyBox(daySun);
                        break;
                    case SceneStateType.Result:
                        ChangeSkyBox(night);
                        break;
                }
            }).AddTo(gameObject);
    }

    private void ChangeSkyBox(Material material)
    {
        RenderSettings.skybox = material;
    }
}
