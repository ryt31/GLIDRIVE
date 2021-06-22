using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Glidrive.Manager;
using UniRx;

public class MenuGameManagerPresenter : MonoBehaviour
{
    [SerializeField] private BackWindow back;
    [SerializeField] private GlidingDiskEffect gEffect;
    [SerializeField] private GameObject raserPointer;
    private void Start()
    {
        var currentScene = SceneStateManager.Instance.CurrentScene;

        currentScene
            .Where(state => state.Equals(SceneStateType.Main))
            .Subscribe(_ =>
            {
                back.FadeOut();
                gEffect.Appear();
                raserPointer.SetActive(false);
            });
    }
}
