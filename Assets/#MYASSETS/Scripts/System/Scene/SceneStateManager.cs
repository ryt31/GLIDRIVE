using System;
using System.Collections;
using UniRx;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Glidrive.Manager
{
    public class SceneStateManager : ManagerBase<SceneStateManager>
    {
        // 現在のシーン
        [SerializeField]
        private SceneStateReactiveProperty currentScene = new SceneStateReactiveProperty(SceneStateType.Title);
        public IReadOnlyReactiveProperty<SceneStateType> CurrentScene => currentScene;

        private void Start()
        {
            DontDestroyOnLoad(gameObject);
        }
        
        public void TransitionToTitle()
        {
            currentScene.Value = SceneStateType.Title;
        }
        
        public void TransitionToMainGame()
        {
            currentScene.Value = SceneStateType.Main;
        }

        public void TransitionToResult()
        {
            currentScene.Value = SceneStateType.Result;
        }
    }
}